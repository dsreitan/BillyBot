using System.Collections.Concurrent;
using Sharky;
using Sharky.DefaultBot;
using Sharky.MicroTasks;
using Action = SC2APIProtocol.Action;

namespace BillyBot.Zerg.MicroTasks;

public class OverlordScoutTask : MicroTask
{
    private readonly BaseData BaseData;

    private readonly Random Random;

    public OverlordScoutTask(DefaultSharkyBot defaultSharkyBot, float priority, bool enabled = true)
    {
        BaseData = defaultSharkyBot.BaseData;

        UnitCommanders = new();
        Priority = priority;
        Enabled = enabled;

        Random = new();
    }

    public override void ClaimUnits(ConcurrentDictionary<ulong, UnitCommander> commanders)
    {
        foreach (var commander in commanders.Where(c => !c.Value.Claimed && c.Value.UnitCalculation.Unit.UnitType == (uint) UnitTypes.ZERG_OVERLORD && c.Value.UnitCalculation.Unit.BuildProgress == 1))
        {
            commander.Value.UnitRole = UnitRole.Scout;
            commander.Value.Claimed = true;
            UnitCommanders.Add(commander.Value);
        }
    }


    public override IEnumerable<Action> PerformActions(int frame)
    {
        var actions = new List<Action>();

        actions.AddRange(ScoutRandomBases(frame));

        return actions;
    }

    private IEnumerable<Action> ScoutRandomBases(int frame)
    {
        var actions = new List<Action>();

        foreach (var commander in UnitCommanders)
            if (commander.UnitCalculation.Unit.Orders.Count() == 0)
            {
                var randomBase = BaseData.BaseLocations[Random.Next(BaseData.BaseLocations.Count)];

                var action = commander.Order(frame, Abilities.MOVE, randomBase.MineralLineLocation);
                if (action != null) actions.AddRange(action);
            }

        return actions;
    }
}