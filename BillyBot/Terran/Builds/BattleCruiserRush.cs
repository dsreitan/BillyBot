using BillyBot.Common;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds.Terran;
using Sharky.DefaultBot;

namespace BillyBot.Terran.Builds;

public class BattleCruiserRush : TerranSharkyBuild
{
    private bool _hasCompletedBattleCruiser;

    public BattleCruiserRush(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);
        BuildOptions.StrictGasCount = true;
        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] = 2;
        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_FACTORY] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_STARPORT] = 1;
        MacroData.DesiredTechCounts[UnitTypes.TERRAN_FUSIONCORE] = 1;
        MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 2;
    }

    public override void OnFrame(ResponseObservation observation)
    {
        var frame = (int) observation.Observation.GameLoop;
        var time = FrameToTimeConverter.GetTime(frame);

        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] = MacroData.Minerals > 500 ? 50 : 4;
        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_HELLION] = time.TotalSeconds < 300 ? 1 : 0;
        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_BATTLECRUISER] = 99;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_SUPPLYDEPOT) > 0) MacroData.DesiredGases = 1;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_ORBITALCOMMAND) > 0) BuildOptions.StrictGasCount = false;

        // stop scv/depot, wait for orbital and factory 
        if (ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.TERRAN_BARRACKS, 50f) > 0
            && (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FACTORY) == 0
                || UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_ORBITALCOMMAND) == 0))
        {
            BuildOptions.StrictWorkerCount = true;
            BuildOptions.StrictSupplyCount = true;
            MacroData.DesiredUnitCounts[UnitTypes.TERRAN_SCV] = 0;
            MacroData.DesiredSupplyDepots = 0;
        }
        else
        {
            BuildOptions.StrictWorkerCount = false;
            BuildOptions.StrictSupplyCount = false;
        }

        if (UnitCountService.EquivalentTypeCount(UnitTypes.TERRAN_COMMANDCENTER) > 1)
            MacroData.DesiredDefensiveBuildingsAtDefensivePoint[UnitTypes.TERRAN_BUNKER] = 1;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FUSIONCORE) > 0)
        {
            MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_STARPORTTECHLAB] = 99;
            MacroData.DesiredTechCounts[UnitTypes.TERRAN_ARMORY] = 1;
        }

        if (UnitCountService.UnitsDoneAndInProgressCount(UnitTypes.TERRAN_BATTLECRUISER) > 0)
        {
            MacroData.DesiredUpgrades[Upgrades.TERRANSHIPWEAPONSLEVEL1] = true;
            MacroData.DesiredUpgrades[Upgrades.BATTLECRUISERENABLESPECIALIZATIONS] = true;
        }

        if (UnitCountService.Completed(UnitTypes.TERRAN_BATTLECRUISER) > 0)
            _hasCompletedBattleCruiser = true;

        if (_hasCompletedBattleCruiser)
        {
            MacroData.DesiredProductionCounts[UnitTypes.TERRAN_STARPORT] = 2;
            if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_STARPORT) > 1)
            {
                MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 3;
                MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] = 1;
                MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] = 2;
                MacroData.DesiredUpgrades[Upgrades.TERRANSHIPWEAPONSLEVEL2] = true;
            }
        }
    }

    public override bool Transition(int frame) => false;
}