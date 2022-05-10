using BillyBot.Protoss.MicroTasks;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.MicroTasks;
using Sharky.MicroTasks.Harass;

namespace BillyBot.Protoss.Builds;

public class DtRobo : BaseBillyBotBuild
{
    private WarpPrismOffenseTask _warpPrismOffenseTask;

    public DtRobo(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        BuildOptions.StrictSupplyCount = true;

        MacroData.DesiredUpgrades[Upgrades.WARPGATERESEARCH] = true;

        MicroTaskData.MicroTasks[nameof(AdeptWorkerHarassTask)].Enable();
        MicroTaskData.MicroTasks[nameof(DarkTemplarHarassTask)].Disable();
        _warpPrismOffenseTask = (WarpPrismOffenseTask) MicroTaskData.MicroTasks[nameof(WarpPrismOffenseTask)];
    }

    public override void OnFrame(ResponseObservation observation)
    {
        var frame = (int) observation.Observation.GameLoop;
        BalancePylons(frame);

        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] =
            UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_DARKSHRINE) == 1 ? 3 : 1;

        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;

        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_ROBOTICSFACILITY] = 1;
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_TWILIGHTCOUNCIL] = 1;
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_DARKSHRINE] = 1;

        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_DARKTEMPLAR] = 3;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_WARPPRISM] = 1;

        if (UnitCountService.Count(UnitTypes.PROTOSS_WARPPRISM) > 0
            && UnitCountService.Count(UnitTypes.PROTOSS_DARKTEMPLAR) > 0)
        {
            _warpPrismOffenseTask.Enable(); // already enabled hmm
            _warpPrismOffenseTask.SetDesiredUnitsClaims(new() {new(UnitTypes.PROTOSS_DARKTEMPLAR, 1), new(UnitTypes.PROTOSS_WARPPRISM, 1)});
            MicroTaskData.MicroTasks["AttackTask"].ResetClaimedUnits();
        }
    }

    public override bool Transition(int frame) => UnitCountService.Completed(UnitTypes.PROTOSS_DARKTEMPLAR) > 3;
}