using BillyBot.Common;
using BillyBot.Terran.Builds.BuildServices;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds.Terran;
using Sharky.DefaultBot;
using Sharky.MicroTasks;

namespace BillyBot.Terran.Builds;

public class BattleCruiserRush : TerranSharkyBuild
{
    private readonly ExpandForever _expandForever;
    private bool _hasCompletedBattleCruiser;
    private bool _hasCompletedHellion;
    private bool _workerCountRestarted;
    private bool _workerCountStopped;

    public BattleCruiserRush(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
    {
        _expandForever = new(defaultSharkyBot);
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);
        BuildOptions.StrictGasCount = true;

        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_FACTORY] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_STARPORT] = 1;
        MacroData.DesiredTechCounts[UnitTypes.TERRAN_FUSIONCORE] = 1;
        MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 2;

        if (MicroTaskData.MicroTasks.ContainsKey("DefenseSquadTask"))
        {
            var defenseSquadTask = (DefenseSquadTask) MicroTaskData.MicroTasks["DefenseSquadTask"];
            defenseSquadTask.DesiredUnitsClaims = new() {new(UnitTypes.TERRAN_MARINE, 1)};
            defenseSquadTask.Enable();
        }
    }

    public override void OnFrame(ResponseObservation observation)
    {
        var frame = (int) observation.Observation.GameLoop;
        var time = FrameToTimeConverter.GetTime(frame);

        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] = MacroData.Minerals > 500 ? 50 : 4;

        StopSupplyAndWorkersUntilFactoryAndOrbital();
        CreateOneHellion();

        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_BATTLECRUISER] = 99;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_SUPPLYDEPOT) > 0) MacroData.DesiredGases = 1;
        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FACTORY) > 0
            && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_ORBITALCOMMAND) > 0) MacroData.DesiredProductionCounts[UnitTypes.TERRAN_COMMANDCENTER] = 2;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_ORBITALCOMMAND) > 0) BuildOptions.StrictGasCount = false;


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
                MacroData.DesiredUpgrades[Upgrades.TERRANSHIPWEAPONSLEVEL2] = true;
                MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 3;
                MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSTECHLAB] = 1;
                MacroData.DesiredAddOnCounts[UnitTypes.TERRAN_BARRACKSREACTOR] = 2;
                MacroData.DesiredUpgrades[Upgrades.SHIELDWALL] = true; // combat shield
            }
        }

        _expandForever.OnFrame();
    }

    private void StopSupplyAndWorkersUntilFactoryAndOrbital()
    {
        if (_workerCountRestarted) return;

        var shouldStop = ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.TERRAN_BARRACKS, .50f) > 0;
        if (shouldStop)
            _workerCountStopped = true;

        if (!_workerCountStopped) return;

        BuildOptions.StrictWorkerCount = true;
        BuildOptions.StrictSupplyCount = true;
        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_SCV] = 0;
        MacroData.DesiredSupplyDepots = 0;

        var shouldRestart = UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FACTORY) > 0
                               && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_ORBITALCOMMAND) > 0;

        if (!shouldRestart) return;

        _workerCountRestarted = true;
        BuildOptions.StrictWorkerCount = false;
        BuildOptions.StrictSupplyCount = false;
    }

    private void CreateOneHellion()
    {
        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_HELLION] = _hasCompletedHellion ? 0 : 1;

        if (UnitCountService.Completed(UnitTypes.TERRAN_HELLION) > 0)
            _hasCompletedHellion = true;
    }

    public override bool Transition(int frame) => false;
}