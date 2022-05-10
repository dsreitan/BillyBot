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
        //base.StartBuild(frame);


        BuildOptions.StrictSupplyCount = true;
        BuildOptions.StrictGasCount = false;

        BuildOptions.StrictWorkerCount = false;

        MacroData.DesiredUpgrades[Upgrades.WARPGATERESEARCH] = true;

        if (!MicroTaskData.MicroTasks["AdeptWorkerHarassTask"].Enabled) MicroTaskData.MicroTasks["AdeptWorkerHarassTask"].Enable();
        if (!MicroTaskData.MicroTasks["DarkTemplarHarassTask"].Enabled) MicroTaskData.MicroTasks["DarkTemplarHarassTask"].Enable();
        if (!MicroTaskData.MicroTasks["WarpPrismOffenseTask"].Enabled) MicroTaskData.MicroTasks["WarpPrismOffenseTask"].Enable();
    }

    public override void OnFrame(ResponseObservation observation)
    {
        int frame = (int)observation.Observation.GameLoop;
        BalancePylons(frame);

        //permanent
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_CYBERNETICSCORE] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_NEXUS] = 2;


        if(UnitCountService.UpgradeDoneOrInProgress(Upgrades.WARPGATERESEARCH)) 
            BuildOptions.StrictGasCount = (UnitCountService.Count(UnitTypes.PROTOSS_GATEWAY)
             + UnitCountService.Count(UnitTypes.PROTOSS_WARPGATE) < 3);

 
        

        if (UnitCountService.Count(UnitTypes.PROTOSS_DARKSHRINE) == 1)
        {
            MakeGateways(3);
            if(frame % 1000 == 0)
            {
            }
            

             
        }


        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_TWILIGHTCOUNCIL] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_ROBOTICSFACILITY] = 1;
        MacroData.DesiredTechCounts[UnitTypes.PROTOSS_DARKSHRINE] = 1;

        if((UnitCountService.Count(UnitTypes.PROTOSS_GATEWAY)
             + UnitCountService.Count(UnitTypes.PROTOSS_WARPGATE)) >= 3)
        {
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_WARPPRISM] = 1;
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_DARKTEMPLAR] = 4;
        }
        
    }

    public override bool Transition(int frame) => UnitCountService.Completed(UnitTypes.PROTOSS_DARKTEMPLAR) > 3;
}