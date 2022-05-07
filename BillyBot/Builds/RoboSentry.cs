using SC2APIProtocol;
using Sharky;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;

namespace BillyBot.Builds;

public class RoboSentry : BaseBillyBotBuild
{
    public RoboSentry(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        AttackData.CustomAttackFunction = true;
        AttackData.UseAttackDataManager = false;
        MacroData.DesiredUpgrades[Upgrades.CHARGE] = true;
        MacroData.DesiredUpgrades[Upgrades.PSISTORMTECH] = true;
    }

    public override void OnFrame(ResponseObservation observation)
    {
        ChronoData.ChronodUnits = new()
        {
            UnitTypes.PROTOSS_IMMORTAL,
            UnitTypes.PROTOSS_OBSERVER,
            UnitTypes.PROTOSS_WARPPRISM,
        };
        
        BalancePylons();
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 3;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_OBSERVER] = 1;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_ROBOTICSFACILITY) > 0)
        {
            MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = 7;
            MacroData.DesiredTechCounts[UnitTypes.PROTOSS_TWILIGHTCOUNCIL] = 1;
        }

        AdeptHarass();
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_ROBOTICSFACILITY] = 1;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_WARPPRISM] = 1;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_IMMORTAL] = 10;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ZEALOT] = 10;

        if (UnitCountService.Completed(UnitTypes.PROTOSS_IMMORTAL) >= 2)
        {
            AttackData.Attacking = true;
            MacroData.DesiredTechCounts[UnitTypes.PROTOSS_TEMPLARARCHIVE] = 1;
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_HIGHTEMPLAR] = 3;
        }
    }
}