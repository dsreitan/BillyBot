using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;
using BillyBot.Protoss.MicroControllers;
using SC2APIProtocol;
using Sharky;
using Sharky.Chat;
using Sharky.DefaultBot;
using Sharky.MicroControllers;
using Sharky.MicroControllers.Protoss;
using Sharky.MicroTasks;
using Sharky.MicroTasks.Attack;
using Sharky.Pathing;
using Sharky.Proxy;
using Action = SC2APIProtocol.Action;

namespace BillyBot.Protoss.MicroTasks;

public class WarpPrismOffenseTask : MicroTask
{
    private readonly ActiveUnitData ActiveUnitData;
    private readonly AreaService AreaService;
    private readonly ChatService ChatService;
    private readonly DebugService DebugService;
    private readonly MapDataService MapDataService;
    private readonly IMicroController MicroController;
    private readonly ProxyLocationService ProxyLocationService;
    private readonly TargetingData TargetingData;
    private readonly TargetingService TargetingService;
    private readonly UnitDataService UnitDataService;
    private readonly WarpPrismMicroController WarpPrismMicroController;

    private float lastFrameTime;

    public WarpPrismOffenseTask(DefaultSharkyBot defaultSharkyBot, IMicroController microController, float priority, bool enabled = false)
    {
        var warpPrismMicroController = new OffensiveWarpPrismMicroController(defaultSharkyBot, defaultSharkyBot.SharkySimplePathFinder, MicroPriority.LiveAndAttack, false);

        TargetingData = defaultSharkyBot.TargetingData;
        ProxyLocationService = defaultSharkyBot.ProxyLocationService;
        MapDataService = defaultSharkyBot.MapDataService;
        DebugService = defaultSharkyBot.DebugService;
        UnitDataService = defaultSharkyBot.UnitDataService;
        ActiveUnitData = defaultSharkyBot.ActiveUnitData;
        AreaService = defaultSharkyBot.AreaService;
        ChatService = defaultSharkyBot.ChatService;
        TargetingService = defaultSharkyBot.TargetingService;

        MicroController = microController;
        WarpPrismMicroController = warpPrismMicroController;

        Priority = priority;
        Enabled = enabled;
        UnitCommanders = new();

        PickupRangeSquared = 25;
    }

    public List<DesiredUnitsClaim>? DesiredUnitsClaims { get; set; }

    private Point2D DefensiveLocation { get; set; }
    private Point2D LoadingLocation { get; set; }
    private int LoadingLocationHeight { get; set; }
    private Point2D DropLocation { get; set; }
    private Point2D TargetLocation { get; set; }
    private int DropLocationHeight { get; set; }
    private float InsideBaseDistanceSquared { get; set; }
    private int PickupRangeSquared { get; }
    private List<Point2D> DropArea { get; set; }

    public void SetDesiredUnitsClaims(List<DesiredUnitsClaim> desiredUnitsClaims)
    {
        DesiredUnitsClaims = desiredUnitsClaims;
    }

    public override void ClaimUnits(ConcurrentDictionary<ulong, UnitCommander> commanders)
    {
        foreach (var commander in commanders)
            if (!commander.Value.Claimed)
            {
                var unitType = commander.Value.UnitCalculation.Unit.UnitType;
                foreach (var desiredUnitClaim in DesiredUnitsClaims)
                    if ((uint) desiredUnitClaim.UnitType == unitType && UnitCommanders.Count(u => u.UnitCalculation.Unit.UnitType == (uint) desiredUnitClaim.UnitType) < desiredUnitClaim.Count)
                    {
                        commander.Value.Claimed = true;
                        UnitCommanders.Add(commander.Value);
                    }
            }
    }

    public override IEnumerable<Action> PerformActions(int frame)
    {
        SetLocations();

        var actions = new List<Action>();

        if (lastFrameTime > 5)
        {
            lastFrameTime = 0;
            return actions;
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        CheckComplete();

        var warpPrisms = UnitCommanders.Where(c => c.UnitCalculation.Unit.UnitType == (uint) UnitTypes.PROTOSS_WARPPRISM || c.UnitCalculation.Unit.UnitType == (uint) UnitTypes.PROTOSS_WARPPRISMPHASING);
        var attackers = UnitCommanders.Where(c => c.UnitCalculation.Unit.UnitType != (uint) UnitTypes.PROTOSS_WARPPRISM && c.UnitCalculation.Unit.UnitType != (uint) UnitTypes.PROTOSS_WARPPRISMPHASING);
        var droppedAttackers = attackers.Where(c => AreaService.InArea(c.UnitCalculation.Unit.Pos, DropArea));
        var unDroppedAttackers = attackers.Where(c => !AreaService.InArea(c.UnitCalculation.Unit.Pos, DropArea));

        if (warpPrisms.Count() > 0)
        {
            foreach (var commander in warpPrisms)
            {
                var action = OrderWarpPrism(commander, droppedAttackers, unDroppedAttackers, frame);
                if (action != null) actions.AddRange(action);
            }

            // move into the loading position
            foreach (var commander in unDroppedAttackers)
            {
                var action = commander.Order(frame, Abilities.ATTACK, LoadingLocation);
                if (action != null) actions.AddRange(action);
            }
            //actions.AddRange(MicroController.Retreat(unDroppedAttackers, LoadingLocation, null, frame));
        }
        else
        {
            if (droppedAttackers.Count() > 0)
            {
                // don't wait for another warp prism, just attack
                var groupPoint = TargetingService.GetArmyPoint(unDroppedAttackers);
                actions.AddRange(MicroController.Attack(unDroppedAttackers, TargetLocation, DefensiveLocation, groupPoint, frame));
            }
            else
            {
                // wait for a warp prism
                var groupPoint = TargetingService.GetArmyPoint(unDroppedAttackers);
                actions.AddRange(MicroController.Retreat(unDroppedAttackers, DefensiveLocation, groupPoint, frame));
            }
        }

        actions.AddRange(MicroController.Attack(droppedAttackers, TargetLocation, DefensiveLocation, null, frame));

        stopwatch.Stop();
        lastFrameTime = stopwatch.ElapsedMilliseconds;
        return actions;
    }

    private void CheckComplete()
    {
        if (MapDataService.SelfVisible(TargetLocation) && !ActiveUnitData.EnemyUnits.Any(e => Vector2.DistanceSquared(new(TargetLocation.X, TargetLocation.Y), e.Value.Position) < 100))
        {
            Disable();
            ChatService.SendChatType("WarpPrismElevatorTask-TaskCompleted");
        }
    }

    private List<Action> OrderWarpPrism(UnitCommander warpPrism, IEnumerable<UnitCommander> droppedAttackers, IEnumerable<UnitCommander> unDroppedAttackers, int frame)
    {
        var readyForPickup = unDroppedAttackers.Where(c => !c.UnitCalculation.Loaded && Vector2.DistanceSquared(c.UnitCalculation.Position, new(LoadingLocation.X, LoadingLocation.Y)) < 10);
        foreach (var pickup in readyForPickup)
        {
            if (warpPrism.UnitCalculation.Unit.UnitType == (uint) UnitTypes.PROTOSS_WARPPRISMPHASING) return warpPrism.Order(frame, Abilities.MORPH_WARPPRISMTRANSPORTMODE);

            if (warpPrism.UnitCalculation.Unit.CargoSpaceMax - warpPrism.UnitCalculation.Unit.CargoSpaceTaken >= UnitDataService.CargoSize((UnitTypes) pickup.UnitCalculation.Unit.UnitType) &&
                !warpPrism.UnitCalculation.Unit.Orders.Any(o => o.AbilityId == (uint) Abilities.UNLOADALLAT_WARPPRISM))
            {
                if (Vector2.DistanceSquared(warpPrism.UnitCalculation.Position, new(LoadingLocation.X, LoadingLocation.Y)) < PickupRangeSquared)
                    return warpPrism.Order(frame, Abilities.LOAD, null, pickup.UnitCalculation.Unit.Tag);
                return warpPrism.Order(frame, Abilities.MOVE, LoadingLocation);
            }
        }

        if (warpPrism.UnitCalculation.Unit.Passengers.Count() > 0)
        {
            if (AreaService.InArea(warpPrism.UnitCalculation.Unit.Pos, DropArea))
                return warpPrism.Order(frame, Abilities.UNLOADALLAT_WARPPRISM, null, warpPrism.UnitCalculation.Unit.Tag);
            return warpPrism.Order(frame, Abilities.UNLOADALLAT_WARPPRISM, DropLocation);
        }

        if (droppedAttackers.Count() > 0)
        {
            List<Action> action = null;
            WarpPrismMicroController.SupportArmy(warpPrism, TargetLocation, DropLocation, null, frame, out action, droppedAttackers.Select(c => c.UnitCalculation));
            return action;
        }

        if (unDroppedAttackers.Count() > 0)
        {
            List<Action> action = null;
            WarpPrismMicroController.SupportArmy(warpPrism, TargetLocation, DefensiveLocation, null, frame, out action, unDroppedAttackers.Select(c => c.UnitCalculation));
            return action;
        }

        return warpPrism.Order(frame, Abilities.MOVE, DefensiveLocation);
    }

    private void SetLocations()
    {
        if (DefensiveLocation == null)
        {
            DefensiveLocation = ProxyLocationService.GetCliffProxyLocation();
            TargetLocation = TargetingData.EnemyMainBasePoint;

            var angle = Math.Atan2(TargetLocation.Y - DefensiveLocation.Y, DefensiveLocation.X - TargetLocation.X);
            var x = -6 * Math.Cos(angle);
            var y = -6 * Math.Sin(angle);
            LoadingLocation = new() {X = DefensiveLocation.X + (float) x, Y = DefensiveLocation.Y - (float) y};
            LoadingLocationHeight = MapDataService.MapHeight(LoadingLocation);

            var loadingVector = new Vector2(LoadingLocation.X, LoadingLocation.Y);
            DropArea = AreaService.GetTargetArea(TargetLocation);
            var dropVector = DropArea.OrderBy(p => Vector2.DistanceSquared(new(p.X, p.Y), loadingVector)).First();
            x = -2 * Math.Cos(angle);
            y = -2 * Math.Sin(angle);
            DropLocation = new() {X = dropVector.X + (float) x, Y = dropVector.Y - (float) y};
            DropLocationHeight = MapDataService.MapHeight(DropLocation);

            InsideBaseDistanceSquared = Vector2.DistanceSquared(new(LoadingLocation.X, LoadingLocation.Y), new(TargetLocation.X, TargetLocation.Y));
        }

        DebugService.DrawSphere(new() {X = LoadingLocation.X, Y = LoadingLocation.Y, Z = 12}, 3, new() {R = 0, G = 0, B = 255});
        DebugService.DrawSphere(new() {X = DropLocation.X, Y = DropLocation.Y, Z = 12}, 3, new() {R = 0, G = 255, B = 0});
    }
}