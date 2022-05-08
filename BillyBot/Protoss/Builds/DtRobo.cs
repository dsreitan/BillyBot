using SC2APIProtocol;
using Sharky;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;

namespace BillyBot.Protoss.Builds;

public class DtRobo : BaseBillyBotBuild
{
    public DtRobo(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        MacroData.DesiredUpgrades[Upgrades.WARPGATERESEARCH] = true;

        if (!MicroTaskData.MicroTasks["AdeptWorkerHarassTask"].Enabled) MicroTaskData.MicroTasks["AdeptWorkerHarassTask"].Enable();
        if (!MicroTaskData.MicroTasks["DarkTemplarHarassTask"].Enabled) MicroTaskData.MicroTasks["DarkTemplarHarassTask"].Enable();
        if (!MicroTaskData.MicroTasks["WarpPrismOffenseTask"].Enabled) MicroTaskData.MicroTasks["WarpPrismOffenseTask"].Enable();
    }

    public override void OnFrame(ResponseObservation observation)
    {
        BalancePylons();

        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] =
            UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_DARKSHRINE) == 1 ? 3 : 1;

        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;

        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_ROBOTICSFACILITY] = 1;
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_TWILIGHTCOUNCIL] = 1;
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_DARKSHRINE] = 1;

        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_DARKTEMPLAR] = 3;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_WARPPRISM] = 1;
    }

    public override bool Transition(int frame) => UnitCountService.Completed(UnitTypes.PROTOSS_DARKTEMPLAR) > 3;
}