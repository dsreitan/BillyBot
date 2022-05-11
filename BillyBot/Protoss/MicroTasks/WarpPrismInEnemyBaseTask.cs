using System.Collections.Concurrent;
using System.Numerics;
using Sharky.MicroTasks;
using Sharky.Pathing;
using Sharky.Proxy;
using Action = SC2APIProtocol.Action;

namespace BillyBot.Protoss.MicroTasks;

public class WarpPrismInEnemyBaseTask : MicroTask
{
    private readonly ActiveUnitData _activeUnitData;

    private readonly AreaService AreaService;
    private readonly DebugService DebugService;
    private readonly MapDataService MapDataService;
    private readonly ProxyLocationService ProxyLocationService;
    private readonly TargetingData TargetingData;

    public WarpPrismInEnemyBaseTask(DefaultSharkyBot defaultSharkyBot, bool enabled = false)
    {
        _activeUnitData = defaultSharkyBot.ActiveUnitData;

        TargetingData = defaultSharkyBot.TargetingData;
        ProxyLocationService = defaultSharkyBot.ProxyLocationService;
        MapDataService = defaultSharkyBot.MapDataService;
        DebugService = defaultSharkyBot.DebugService;
        AreaService = defaultSharkyBot.AreaService;

        Enabled = enabled;
        UnitCommanders = new();
    }

    private Point2D DefensiveLocation { get; set; }
    private Point2D LoadingLocation { get; set; }
    private int LoadingLocationHeight { get; set; }
    private Point2D DropLocation { get; set; }
    private Point2D TargetLocation { get; set; }
    private int DropLocationHeight { get; set; }
    private float InsideBaseDistanceSquared { get; set; }
    private int PickupRangeSquared { get; }
    private List<Point2D> DropArea { get; set; }

    public override void ClaimUnits(ConcurrentDictionary<ulong, UnitCommander> commanders)
    {
        var warpPrism = GetWarpPrism(_activeUnitData.Commanders.Values);
        if (warpPrism == null)
            return;

        if (warpPrism.Claimed)
            return;

        warpPrism.Claimed = true;
        UnitCommanders.Add(warpPrism);
    }

    public override IEnumerable<Action> PerformActions(int frame)
    {
        var actions = new List<Action>();

        var warpPrism = GetWarpPrism(UnitCommanders);
        if (warpPrism == null)
            return actions;

        // check if the task should be disabled

        SetLocations();

        if (DropLocation == null) return warpPrism.Order(frame, Abilities.MOVE, DefensiveLocation);

        var isInWarpInLocation = Vector2.DistanceSquared(warpPrism.UnitCalculation.Position, new(DropLocation.X, DropLocation.Y)) < 10;
        if (!isInWarpInLocation) return warpPrism.Order(frame, Abilities.MOVE, DropLocation);

        return warpPrism.Order(frame, Abilities.MORPH_WARPPRISMPHASINGMODE);
    }

    private UnitCommander? GetWarpPrism(IEnumerable<UnitCommander> unitCommanders)
    {
        return unitCommanders.FirstOrDefault(c => c.UnitCalculation.Unit.UnitType is (uint) UnitTypes.PROTOSS_WARPPRISM or (uint) UnitTypes.PROTOSS_WARPPRISMPHASING);
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