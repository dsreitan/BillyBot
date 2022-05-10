using BillyBot.Common;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;

namespace BillyBot.Protoss.Builds;

public class BaseBillyBotBuild : ProtossSharkyBuild
{
    public BaseBillyBotBuild(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        MacroData.DesiredUpgrades[Upgrades.WARPGATERESEARCH] = true;
    }

    /// <summary>
    ///     Used with strict supply true, to override it's formula.
    /// </summary>
    protected void BalancePylons(int frame)
    {
        var nexusCount = ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.PROTOSS_NEXUS, .90f);
        var gatewayCount = ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.PROTOSS_GATEWAY, .90f);
        var warpgateCount = ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.PROTOSS_WARPGATE, .90f);
        var roboCount = ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.PROTOSS_ROBOTICSFACILITY, .90f);
        var stargateCount = ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.PROTOSS_STARGATE, .90f);

        double productionCapacity = nexusCount +
                                 (gatewayCount + warpgateCount) * 2 +
                                 roboCount * 6 +
                                 stargateCount * 6;

        var productionCapacityInPylons =  (productionCapacity / 8);
        double pylonsCurrentlyUsed = (MacroData.FoodUsed - nexusCount * 15) / 8.0;
  
        
        MacroData.DesiredPylons = (int) Math.Ceiling(pylonsCurrentlyUsed + productionCapacityInPylons);


    }

    protected void AdeptHarass()
    {
        if (UnitCountService.Completed(UnitTypes.PROTOSS_ADEPT) <= 2)
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ADEPT] = 2;
    }

    protected void MakeGateways(int desired)
    {
        int currentGates = UnitCountService.Count(UnitTypes.PROTOSS_GATEWAY) + UnitCountService.Count(UnitTypes.PROTOSS_WARPGATE);
        int currentGatesProgress = UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_GATEWAY);
        if (currentGates < desired && currentGatesProgress < desired)
        {
                MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = currentGates+1;
        }


    }

    protected void desiredDebug(int frame, int delay, bool showAllreadyAquired)
    {
        if (frame % delay == 0)
        {
            String desires = (showAllreadyAquired) ? "Shows Aquired" + "\n" : "Hides Aquired" +  "\n";
            
            
            desires += "buildings: \n";
            foreach (var u in MacroData.DesiredProductionCounts)
                if (MacroData.BuildProduction[u.Key]) desires += u.ToString() + ":" + MacroData.BuildProduction[u.Key] + " want " + u.Value + "\n";
                else
                   if (showAllreadyAquired) desires += u.ToString() + ":" + MacroData.BuildProduction[u.Key] + " want " + u.Value + "\n";

            desires += "techs \n";
            foreach (var u in MacroData.DesiredTechCounts)
                if(MacroData.BuildTech[u.Key]) desires += u.ToString() + ":" + MacroData.BuildTech[u.Key] + " want " + u.Value + "\n";
                else
                    if(showAllreadyAquired) desires += u.ToString() + ":" + MacroData.BuildTech[u.Key] + " want " + u.Value + "\n";

            desires += "units \n";
            List<UnitTypes> allUnits = MacroData.NexusUnits;
            allUnits.AddRange(MacroData.GatewayUnits);
            allUnits.AddRange(MacroData.RoboticsFacilityUnits);
            allUnits.AddRange(MacroData.StargateUnits);
            foreach (var u in MacroData.DesiredUnitCounts)
                if (MacroData.BuildUnits[u.Key]) desires += u.ToString() + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";
                else
                    if(showAllreadyAquired) desires += u.ToString() + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";

            desires += "morphs \n";
            foreach (var u in MacroData.DesiredMorphCounts)
                if (MacroData.Morph[u.Key]) desires += u.ToString() + ":" + MacroData.Morph[u.Key] + " want " + u.Value + "\n";
                else
                    if(showAllreadyAquired) desires += u.ToString() + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";
            
            foreach (var u in MacroData.DesiredUpgrades)
                if(u.Value) desires += u.ToString() + ":" + u.Value + "\n";
                else
                    if (showAllreadyAquired) desires += u.ToString() + ":" + u.Value + "\n";

            if (BuildOptions.StrictGasCount)
                desires += "Gascount desired " + MacroData.DesiredGases + "\n";
            if (BuildOptions.StrictWorkerCount)
                desires += "Workercount desired " + MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_PROBE] + "\n";

            Console.WriteLine(desires);

        }


    }
        
}