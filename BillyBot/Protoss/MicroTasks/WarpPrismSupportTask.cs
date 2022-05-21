using System.Collections.Concurrent;
using BillyBot.Protoss.MicroControllers;
using Sharky.MicroTasks;
using Action = SC2APIProtocol.Action;

namespace BillyBot.Protoss.MicroTasks;

public class WarpPrismSupportTask : MicroTask
{
    private readonly WarpPrismSupportMicroController _warpPrismMicroController;

    public WarpPrismSupportTask(DefaultSharkyBot defaultSharkyBot, IEnumerable<UnitTypes> supportTargetTypes, bool enabled = false)
    {
        _warpPrismMicroController = new(defaultSharkyBot, defaultSharkyBot.SharkySimplePathFinder, MicroPriority.LiveAndAttack, false);
        _supportTargetTypes = supportTargetTypes;
        Enabled = enabled;
        UnitCommanders = new();
    }

    private IEnumerable<UnitTypes> _supportTargetTypes { get; }

    public override void ClaimUnits(ConcurrentDictionary<ulong, UnitCommander> commanders)
    {
        ClaimWarpPrism(commanders.Values);
        ClaimSupportTargets(commanders.Values);
    }

    private void ClaimSupportTargets(IEnumerable<UnitCommander> commanders)
    {
        if (!_supportTargetTypes.Any())
            return;

        var supportTargets = GetSupportTargets(commanders);
        if (supportTargets.All(x => x.Claimed))
            return;

        foreach (var supportTarget in supportTargets)
        {
            supportTarget.Claimed = true;
            UnitCommanders.Add(supportTarget);
        }
    }

    private IEnumerable<UnitCommander> GetSupportTargets(IEnumerable<UnitCommander> commanders)
    {
        
        return commanders.Where(c => _supportTargetTypes.Contains((UnitTypes) c.UnitCalculation.Unit.UnitType));
    }

    private void ClaimWarpPrism(IEnumerable<UnitCommander> commanders)
    {
        var warpPrism = GetWarpPrism(commanders);
        if (warpPrism == null)
            return;

        if (warpPrism.Claimed)
            return;

        warpPrism.Claimed = true;
        UnitCommanders.Add(warpPrism);
    }

    public override IEnumerable<Action> PerformActions(int frame)
    {
        var warpPrism = GetWarpPrism(UnitCommanders);
        if (warpPrism == null)
            return new List<Action>();

        if (warpPrism.LastAbility.Equals(Abilities.MORPH_WARPPRISMPHASINGMODE))
            return warpPrism.Order(frame, Abilities.MORPH_WARPPRISMTRANSPORTMODE);

        

        var supportTargets = GetSupportTargets(UnitCommanders);
        if (!supportTargets.Any())
        {
            return new List<Action>();
        }
        //TODO: set locations
        var target = new Point2D();
        var defensivePoint = new Point2D();
        var groupCenter = new Point2D();

        return _warpPrismMicroController.Support(warpPrism, supportTargets, target, defensivePoint, groupCenter, frame);
    }

    private UnitCommander? GetWarpPrism(IEnumerable<UnitCommander> unitCommanders)
    {
        return unitCommanders.FirstOrDefault(c => c.UnitCalculation.Unit.UnitType is (uint) UnitTypes.PROTOSS_WARPPRISM or (uint) UnitTypes.PROTOSS_WARPPRISMPHASING);
    }

    // private void SetLocations()
    // {
    //     if (DefensiveLocation == null)
    //     {
    //         DefensiveLocation = ProxyLocationService.GetCliffProxyLocation();
    //         TargetLocation = TargetingData.EnemyMainBasePoint;
    //
    //         var angle = Math.Atan2(TargetLocation.Y - DefensiveLocation.Y, DefensiveLocation.X - TargetLocation.X);
    //         var x = -6 * Math.Cos(angle);
    //         var y = -6 * Math.Sin(angle);
    //         LoadingLocation = new() {X = DefensiveLocation.X + (float) x, Y = DefensiveLocation.Y - (float) y};
    //         LoadingLocationHeight = MapDataService.MapHeight(LoadingLocation);
    //
    //         var loadingVector = new Vector2(LoadingLocation.X, LoadingLocation.Y);
    //         DropArea = AreaService.GetTargetArea(TargetLocation);
    //         var dropVector = DropArea.OrderBy(p => Vector2.DistanceSquared(new(p.X, p.Y), loadingVector)).First();
    //         x = -2 * Math.Cos(angle);
    //         y = -2 * Math.Sin(angle);
    //         DropLocation = new() {X = dropVector.X + (float) x, Y = dropVector.Y - (float) y};
    //         DropLocationHeight = MapDataService.MapHeight(DropLocation);
    //
    //         InsideBaseDistanceSquared = Vector2.DistanceSquared(new(LoadingLocation.X, LoadingLocation.Y), new(TargetLocation.X, TargetLocation.Y));
    //     }
    //
    //     DebugService.DrawSphere(new() {X = LoadingLocation.X, Y = LoadingLocation.Y, Z = 12}, 3, new() {R = 0, G = 0, B = 255});
    //     DebugService.DrawSphere(new() {X = DropLocation.X, Y = DropLocation.Y, Z = 12}, 3, new() {R = 0, G = 255, B = 0});
    // }
}