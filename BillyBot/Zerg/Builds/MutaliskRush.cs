﻿using SC2APIProtocol;
using Sharky;
using Sharky.Builds.Zerg;
using Sharky.DefaultBot;

namespace BillyBot.Zerg.Builds;

public class MutaliskRush : ZergSharkyBuild
{
    public MutaliskRush(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        BuildOptions.StrictGasCount = true;

        MacroData.DesiredProductionCounts[UnitTypes.ZERG_HATCHERY] = 2;
    }

    public override void OnFrame(ResponseObservation observation)
    {
        if (UnitCountService.EquivalentTypeCount(UnitTypes.ZERG_HATCHERY) >= 2)
        {
            if (MacroData.DesiredTechCounts[UnitTypes.ZERG_SPAWNINGPOOL] < 1) MacroData.DesiredTechCounts[UnitTypes.ZERG_SPAWNINGPOOL] = 1;
        }

        if (UnitCountService.Count(UnitTypes.ZERG_SPAWNINGPOOL) > 0) BuildOptions.StrictGasCount = false;

        if (UnitCountService.Completed(UnitTypes.ZERG_SPAWNINGPOOL) > 0)
        {
            if (MacroData.DesiredUnitCounts[UnitTypes.ZERG_ZERGLING] < 6) MacroData.DesiredUnitCounts[UnitTypes.ZERG_ZERGLING] = 6;
            if (MacroData.DesiredUnitCounts[UnitTypes.ZERG_QUEEN] < UnitCountService.EquivalentTypeCount(UnitTypes.ZERG_HATCHERY)) MacroData.DesiredUnitCounts[UnitTypes.ZERG_QUEEN] = UnitCountService.EquivalentTypeCount(UnitTypes.ZERG_HATCHERY);

            if (MacroData.DesiredMorphCounts[UnitTypes.ZERG_LAIR] < 1) MacroData.DesiredMorphCounts[UnitTypes.ZERG_LAIR] = 1;

            MacroData.DesiredDefensiveBuildingsAtEveryMineralLine[UnitTypes.ZERG_SPORECRAWLER] = 1;
            MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.ZERG_SPINECRAWLER] = 1;
        }

        if (UnitCountService.Completed(UnitTypes.ZERG_LAIR) > 0)
        {
            if (MacroData.DesiredTechCounts[UnitTypes.ZERG_SPIRE] < 1) MacroData.DesiredTechCounts[UnitTypes.ZERG_SPIRE] = 1;
        }

        if (UnitCountService.EquivalentTypeCompleted(UnitTypes.ZERG_SPIRE) > 0)
        {
            if (MacroData.DesiredUnitCounts[UnitTypes.ZERG_MUTALISK] < 75) MacroData.DesiredUnitCounts[UnitTypes.ZERG_MUTALISK] = 75;

            if (MacroData.VespeneGas >= 100)
                MacroData.DesiredUnitCounts[UnitTypes.ZERG_ZERGLING] = 0;
            else
                MacroData.DesiredUnitCounts[UnitTypes.ZERG_ZERGLING] = UnitCountService.Count(UnitTypes.ZERG_ZERGLING) + UnitCountService.Count(UnitTypes.ZERG_LARVA) * 2;

            if (MacroData.DesiredUnitCounts[UnitTypes.ZERG_OVERSEER] < 1) MacroData.DesiredUnitCounts[UnitTypes.ZERG_OVERSEER] = 1;

            MacroData.DesiredUpgrades[Upgrades.ZERGFLYERWEAPONSLEVEL1] = true;
        }

        if (MacroData.Minerals > 500)
        {
            if (MacroData.DesiredProductionCounts[UnitTypes.ZERG_HATCHERY] <= UnitCountService.EquivalentTypeCount(UnitTypes.ZERG_HATCHERY)) MacroData.DesiredProductionCounts[UnitTypes.ZERG_HATCHERY]++;
        }
    }
}