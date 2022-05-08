using System.Collections.Concurrent;
using Sharky;
using Sharky.DefaultBot;
using Sharky.MicroTasks;
using Action = SC2APIProtocol.Action;

namespace BillyBot.Terran.MicroTasks;

public class VikingDropTask : MicroTask
{
    private readonly ActiveUnitData ActiveUnitData;
    private readonly BaseData BaseData;
    private readonly TargetingData TargetingData;

    public VikingDropTask(DefaultSharkyBot defaultSharkyBot, float priority, bool enabled = true)
    {
        BaseData = defaultSharkyBot.BaseData;
        ActiveUnitData = defaultSharkyBot.ActiveUnitData;
        TargetingData = defaultSharkyBot.TargetingData;

        UnitCommanders = new();
        Priority = priority;
        Enabled = enabled;
    }

    public override void ClaimUnits(ConcurrentDictionary<ulong, UnitCommander> commanders)
    {
        foreach (var commander in commanders.Where(c => !c.Value.Claimed && (c.Value.UnitCalculation.Unit.UnitType == (uint) UnitTypes.TERRAN_VIKINGFIGHTER || c.Value.UnitCalculation.Unit.UnitType == (uint) UnitTypes.TERRAN_MEDIVAC)))
        {
            commander.Value.Claimed = true;
            UnitCommanders.Add(commander.Value);
        }
    }

    public override IEnumerable<Action> PerformActions(int frame)
    {
        var actions = new List<Action>();

        var medivacs = UnitCommanders.Where(c => c.UnitCalculation.Unit.UnitType == (uint) UnitTypes.TERRAN_MEDIVAC);
        var assaulters = UnitCommanders.Where(c => c.UnitCalculation.Unit.UnitType == (uint) UnitTypes.TERRAN_VIKINGASSAULT);
        var fighters = UnitCommanders.Where(c => c.UnitCalculation.Unit.UnitType == (uint) UnitTypes.TERRAN_VIKINGFIGHTER);

        actions.AddRange(MorphFighters(fighters, frame));
        actions.AddRange(MedivacActions(medivacs, frame));
        actions.AddRange(AssaultActions(assaulters, frame));

        return actions;
    }

    private IEnumerable<Action> MorphFighters(IEnumerable<UnitCommander> fighters, int frame)
    {
        var actions = new List<Action>();

        foreach (var commander in fighters)
        {
            var action = commander.Order(frame, Abilities.MORPH_VIKINGASSAULTMODE);
            if (action != null) actions.AddRange(action);
        }

        return actions;
    }

    private IEnumerable<Action> MedivacActions(IEnumerable<UnitCommander> medivacs, int frame)
    {
        var actions = new List<Action>();

        foreach (var commander in medivacs)
            if (commander.UnitCalculation.Unit.Orders.Any(o => o.AbilityId == (uint) Abilities.UNLOADALLAT_MEDIVAC))
                continue;
            else
            {
                if (commander.UnitCalculation.NearbyAllies.Any(a => a.Unit.UnitType == (uint) UnitTypes.TERRAN_STARPORT))
                {
                    if (commander.UnitCalculation.Unit.CargoSpaceTaken < commander.UnitCalculation.Unit.CargoSpaceMax)
                    {
                        var nearestAssault = commander.UnitCalculation.NearbyAllies.FirstOrDefault(a => a.Unit.UnitType == (uint) UnitTypes.TERRAN_VIKINGASSAULT && !a.Loaded);
                        if (nearestAssault != null)
                        {
                            var action = commander.Order(frame, Abilities.LOAD, targetTag: nearestAssault.Unit.Tag);
                            if (action != null) actions.AddRange(action);
                        }
                    }
                    else
                    {
                        var dropBase = BaseData.EnemyBaseLocations.FirstOrDefault();
                        if (dropBase != null)
                        {
                            var action = commander.Order(frame, Abilities.UNLOADALLAT_MEDIVAC, dropBase.BehindMineralLineLocation);
                            if (action != null) actions.AddRange(action);
                        }
                    }
                }
                else
                {
                    var starport = ActiveUnitData.SelfUnits.FirstOrDefault(a => a.Value.Unit.UnitType == (uint) UnitTypes.TERRAN_STARPORT).Value;
                    if (starport != null)
                    {
                        var action = commander.Order(frame, Abilities.MOVE, targetTag: starport.Unit.Tag);
                        if (action != null) actions.AddRange(action);
                    }
                }
            }

        return actions;
    }

    private IEnumerable<Action> AssaultActions(IEnumerable<UnitCommander> assaulters, int frame)
    {
        var actions = new List<Action>();

        foreach (var commander in assaulters)
            if (!commander.UnitCalculation.NearbyAllies.Any(a => a.Unit.UnitType == (uint) UnitTypes.TERRAN_STARPORT))
            {
                var action = commander.Order(frame, Abilities.ATTACK, TargetingData.AttackPoint);
                if (action != null) actions.AddRange(action);
            }

        return actions;
    }
}