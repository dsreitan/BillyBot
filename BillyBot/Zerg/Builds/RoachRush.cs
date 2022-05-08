using SC2APIProtocol;
using Sharky;
using Sharky.Builds.Zerg;
using Sharky.DefaultBot;

namespace BillyBot.Zerg.Builds;

public class RoachRush : ZergSharkyBuild
{
    public RoachRush(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        BuildOptions.StrictGasCount = true;

        MacroData.DesiredProductionCounts[UnitTypes.ZERG_HATCHERY] = 2;
        MacroData.DesiredUpgrades[Upgrades.ZERGMISSILEWEAPONSLEVEL1] = true;
        MacroData.DesiredUpgrades[Upgrades.GLIALRECONSTITUTION] = true;
    }

    public override void OnFrame(ResponseObservation observation)
    {
        var frame = (int) observation.Observation.GameLoop;
        var time = FrameToTimeConverter.GetTime(frame);

        if (time.TotalSeconds is > 100 and < 140)
        {
            BuildOptions.StrictWorkerCount = true;
            MacroData.DesiredUnitCounts[UnitTypes.ZERG_DRONE] = 0;
            MacroData.DesiredUnitCounts[UnitTypes.ZERG_ZERGLING] = 6;
        }
        else
        {
            BuildOptions.StrictWorkerCount = false;
            MacroData.DesiredUnitCounts[UnitTypes.ZERG_ZERGLING] = 0;
            MacroData.DesiredUnitCounts[UnitTypes.ZERG_QUEEN] = UnitCountService.EquivalentTypeCount(UnitTypes.ZERG_HATCHERY);
        }

        if (UnitCountService.EquivalentTypeCount(UnitTypes.ZERG_HATCHERY) > 1)
        {
            MacroData.DesiredGases = 1;
            MacroData.DesiredTechCounts[UnitTypes.ZERG_SPAWNINGPOOL] = 1;
        }

        if (UnitCountService.Completed(UnitTypes.ZERG_QUEEN) > 0) MacroData.DesiredMorphCounts[UnitTypes.ZERG_LAIR] = 1;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.ZERG_LAIR) > 0)
        {
            MacroData.DesiredUnitCounts[UnitTypes.ZERG_DRONE] = 25;
            MacroData.DesiredTechCounts[UnitTypes.ZERG_EVOLUTIONCHAMBER] = 1;
            MacroData.DesiredTechCounts[UnitTypes.ZERG_ROACHWARREN] = 1;
            MacroData.DesiredUnitCounts[UnitTypes.ZERG_ROACH] = 10;
            MacroData.DesiredGases = 2;
        }
    }

    public override bool Transition(int frame) => UnitCountService.Completed(UnitTypes.ZERG_ROACH) > 8;
}