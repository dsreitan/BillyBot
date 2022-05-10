using SC2APIProtocol;
using Sharky;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;

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


        if (!MicroTaskData.MicroTasks["WorkerScoutTask"].Enabled) MicroTaskData.MicroTasks["WorkerScoutTask"].Enable();
    }

    public override void OnFrame(ResponseObservation observation)
    {
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
        {
            MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
        }

    }

    public override bool Transition(int frame) => UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0;
}