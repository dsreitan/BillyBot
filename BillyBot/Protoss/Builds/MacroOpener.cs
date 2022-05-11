using Sharky.Builds.BuildChoosing;
using Sharky.MicroTasks;

namespace BillyBot.Protoss.Builds;

public class MacroOpener : BaseBillyBotBuild
{
    public MacroOpener(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        BuildOptions.StrictSupplyCount = true;
        BuildOptions.StrictGasCount = true;
        BuildOptions.StrictWorkerCount = true;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_PROBE] = 20;

        MicroTaskData.MicroTasks[nameof(WorkerScoutTask)].Enable();
    }

    public override void OnFrame(ResponseObservation observation)
    {
        base.OnFrame(observation);

        MacroData.DesiredPylons = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;

        if (MacroData.FoodUsed > 15)
        {
            ChronoData.ChronodUnits = new()
            {
                UnitTypes.PROTOSS_PROBE
            };
        }

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_PYLON) == 1)
            MacroData.DesiredGases = 1;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_NEXUS) > 1)
            MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
    }

    public override bool Transition(int frame) => UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0;
}