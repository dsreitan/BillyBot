using BillyBot.Protoss.MicroTasks;
using Sharky.Builds.BuildChoosing;
using Sharky.MicroTasks;
using Sharky.MicroTasks.Harass;

namespace BillyBot.Protoss.Builds;

/**
 * Build order
 * https://lotv.spawningtool.com/build/118611/
 */
public class DtRobo : BaseBillyBotBuild
{
    public DtRobo(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    {
    }

    public override void StartBuild(int frame)
    {
        ChronoData.ChronodUnits = new()
        {
            UnitTypes.PROTOSS_OBSERVER,
            UnitTypes.PROTOSS_WARPPRISM,
            UnitTypes.PROTOSS_PROBE
        };

        BuildOptions.StrictSupplyCount = true;
        BuildOptions.StrictGasCount = false;
        MacroData.DesiredPylonsAtEveryBase = 1;

        BuildOptions.StrictWorkerCount = false;

        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;

        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
        MacroData.DesiredUpgrades[Upgrades.WARPGATERESEARCH] = true;

        MicroTaskData.MicroTasks[nameof(AdeptWorkerHarassTask)].Enable();
        MicroTaskData.MicroTasks[nameof(DarkTemplarHarassTask)].Enable();
        MicroTaskData.MicroTasks[nameof(WarpPrismInEnemyBaseTask)].Enable();
        MicroTaskData.MicroTasks[nameof(DtWarpInTask)].Enable();
    }

    public override void OnFrame(ResponseObservation observation)
    {
        base.OnFrame(observation);

        var frame = (int) observation.Observation.GameLoop;

        BalancePylons(frame);

        if (UnitCountService.UpgradeDoneOrInProgress(Upgrades.WARPGATERESEARCH))
        {
            BuildOptions.StrictGasCount = UnitCountService.Count(UnitTypes.PROTOSS_GATEWAY)
                + UnitCountService.Count(UnitTypes.PROTOSS_WARPGATE) < 3;
        }

        if (UnitCountService.Count(UnitTypes.PROTOSS_DARKSHRINE) == 1) MakeGateways(3);

        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_TWILIGHTCOUNCIL] = 1;
        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_TWILIGHTCOUNCIL) > 0)
        {
            MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_ROBOTICSFACILITY] = 1;
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_WARPPRISM] = 1;
            MacroData.DesiredTechCounts[UnitTypes.PROTOSS_DARKSHRINE] = 1;
        }

        if (UnitCountService.Completed(UnitTypes.PROTOSS_DARKTEMPLAR) >= 4)
        {
            MicroTaskData.MicroTasks[nameof(WarpPrismInEnemyBaseTask)].Disable();
            MicroTaskData.MicroTasks[nameof(WarpPrismSupportTask)].Enable();
        }

        // DtWarpInTask should make sure DT's are warped in the enemy base
    }

    public override bool Transition(int frame) => false;
}