using System.Collections.Concurrent;
using Sharky.Builds.BuildingPlacement;
using Sharky.MicroTasks;
using Action = SC2APIProtocol.Action;

namespace BillyBot.Protoss.MicroTasks;

public class DtWarpInTask : MicroTask
{
    private readonly ActiveUnitData _activeUnitData;
    private readonly SharkyOptions _sharkyOptions;
    private readonly SharkyUnitData _sharkyUnitData;
    private readonly UnitCountService _unitCountService;
    private readonly WarpInPlacement _warpInPlacement;

    public DtWarpInTask(DefaultSharkyBot defaultSharkyBot, bool enabled = false)
    {
        _unitCountService = defaultSharkyBot.UnitCountService;
        _activeUnitData = defaultSharkyBot.ActiveUnitData;
        _sharkyOptions = defaultSharkyBot.SharkyOptions;
        _sharkyUnitData = defaultSharkyBot.SharkyUnitData;
        _warpInPlacement = (WarpInPlacement) defaultSharkyBot.WarpInPlacement;

        Enabled = enabled;
        UnitCommanders = new();
    }
    
    public override void ClaimUnits(ConcurrentDictionary<ulong, UnitCommander> commanders)
    {
        // set unitcommanders
    }

    public override IEnumerable<Action> PerformActions(int frame)
    {
        var actions = new List<Action>();
        if (_unitCountService.Completed(UnitTypes.PROTOSS_DARKSHRINE) == 0)
            return actions;

        var idleWarpGates = _activeUnitData.Commanders.Values.Where(c => c.UnitCalculation.Unit.UnitType == (uint) UnitTypes.PROTOSS_WARPGATE && c.WarpInOffCooldown(frame, _sharkyOptions.FramesPerSecond, _sharkyUnitData));
        if (idleWarpGates.Any() == false)
            return actions;

        var warpPrism = _activeUnitData.Commanders.Values.FirstOrDefault(c => c.UnitCalculation.Unit.UnitType is (uint) UnitTypes.PROTOSS_WARPPRISM or (uint) UnitTypes.PROTOSS_WARPPRISMPHASING);
        if (warpPrism == null)
            return actions;

        // cant find warpinlocation?
        var warpInLocation = _warpInPlacement.FindPlacementForPylon(warpPrism.UnitCalculation, 1);
        if (warpInLocation == null)
            return actions;

        foreach (var warpGate in idleWarpGates)
            actions.AddRange(warpGate.Order(frame, Abilities.TRAIN_DARKTEMPLAR, warpInLocation));

        return actions;
    }
}