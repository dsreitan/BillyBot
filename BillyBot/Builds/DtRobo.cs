using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;

namespace BillyBot.Builds;

public class DtRobo : ProtossSharkyBuild
{
    public DtRobo(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);
        
        ChronoData.ChronodUnits = new()
        {
            UnitTypes.PROTOSS_PROBE,
            UnitTypes.PROTOSS_ADEPT,
            UnitTypes.PROTOSS_WARPPRISM,
        };
        
        if (!MicroTaskData.MicroTasks["AdeptWorkerHarassTask"].Enabled) MicroTaskData.MicroTasks["AdeptWorkerHarassTask"].Enable();
        if (!MicroTaskData.MicroTasks["DarkTemplarHarassTask"].Enabled) MicroTaskData.MicroTasks["DarkTemplarHarassTask"].Enable();

        // ChronoData.ChronodUpgrades = new()
        // {
        //     Upgrades.WARPGATERESEARCH
        // };
    }

    private bool hasCreatedGateway = false;
    public override void OnFrame(ResponseObservation observation)
    {
        //TODO: ta 1 gas før nexus, vent med chrono, worker count stop?
        
        BuildOptions.StrictGasCount = true;


        hasCreatedGateway = UnitCountService.Completed(UnitTypes.PROTOSS_GATEWAY) > 0;

        BuildOptions.StrictGasCount = true;



        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_PYLON) == 1)
        {
            MacroData.DesiredGases = 1;
            if(!(UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)) { 
               MacroData.DesiredPylons = 1;
            }
        }

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_CYBERNETICSCORE) > 0)
        {
            BuildOptions.StrictGasCount = false;
            BuildOptions.StrictSupplyCount = false;
        }



        MacroData.DesiredUpgrades[Upgrades.WARPGATERESEARCH] = true;
        
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = 1;


        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;
        

        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_ROBOTICSFACILITY] = 1;
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_TWILIGHTCOUNCIL] = 1;
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_DARKSHRINE] = 1;
        
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ADEPT] = 2;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_STALKER] = 1;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_DARKTEMPLAR] = 3;
        MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_WARPPRISM] = 1;
    }
}