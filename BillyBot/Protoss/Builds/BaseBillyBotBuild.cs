using BillyBot.Common;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using System.Text.RegularExpressions;

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

    public override void OnFrame(ResponseObservation observation)
    {
        base.OnFrame(observation);
        var chat = observation.Chat;
        foreach (var chatReceived in chat)
        {
            var match = Regex.Match(chatReceived.Message.ToLower(), @"desires");
            if (match.Success)
            {
                string desires = desiredDebug(false);
                Console.WriteLine(desires);
            }
        }
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
        if (frame % delay == 0) Console.WriteLine(desiredDebug(showAllreadyAquired));
    }


    protected String desiredDebug(bool showAllreadyAquired)
    {

        String desires = (showAllreadyAquired) ? "Shows Aquired" + "\n" : "Hides Aquired" + "\n";


        var prodDesires = "";
        foreach (var u in MacroData.DesiredProductionCounts)
            if (MacroData.BuildProduction[u.Key]) {
                if(prodDesires.Length==0) prodDesires+= "buildings: \n";
                prodDesires += u.ToString() + ":" + MacroData.BuildProduction[u.Key] + " want " + u.Value + "\n";
            }
            else
                if (showAllreadyAquired) prodDesires += u.ToString() + ":" + MacroData.BuildProduction[u.Key] + " want " + u.Value + "\n";
        desires += prodDesires;


        var techDesires = "";
        foreach (var u in MacroData.DesiredTechCounts) { 
            if (MacroData.BuildTech[u.Key]) {
                if (techDesires.Length == 0) techDesires += "techs: \n";
                techDesires += u.ToString() + ":" + MacroData.BuildTech[u.Key] + " want " + u.Value + "\n"; 
            }
            else
                if (showAllreadyAquired) techDesires += u.ToString() + ":" + MacroData.BuildTech[u.Key] + " want " + u.Value + "\n";
        }
        desires += techDesires;


        var unitDesires = "";
        List<UnitTypes> allUnits = MacroData.NexusUnits;
        allUnits.AddRange(MacroData.GatewayUnits);
        allUnits.AddRange(MacroData.RoboticsFacilityUnits);
        allUnits.AddRange(MacroData.StargateUnits);
        foreach (var u in MacroData.DesiredUnitCounts)
            if (MacroData.BuildUnits[u.Key])
            {
                if (unitDesires.Length == 0) unitDesires += "units \n";
                unitDesires += u.ToString() + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";
            }
            else
                if (showAllreadyAquired) unitDesires += u.ToString() + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";
        desires += unitDesires;

        var morphDesires = "";
        foreach (var u in MacroData.DesiredMorphCounts)
            if (MacroData.Morph[u.Key])
            {
                if(morphDesires.Length==0) morphDesires+= "morphs \n";
                desires += u.ToString() + ":" + MacroData.Morph[u.Key] + " want " + u.Value + "\n";
            }
            else
                if (showAllreadyAquired) desires += u.ToString() + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";
        desires += morphDesires;

        var upgradeDesires = "";
        foreach (var u in MacroData.DesiredUpgrades)
            if(u.Value && !UnitCountService.UpgradeDoneOrInProgress(u.Key)) upgradeDesires += u.ToString() + ":" + u.Value + "\n";
            else
                if (showAllreadyAquired) upgradeDesires += u.ToString() + ":" + u.Value + "\n";
        desires += upgradeDesires;

        if (BuildOptions.StrictGasCount)
            desires += "Gascount desired " + MacroData.DesiredGases + "\n";
        if (BuildOptions.StrictWorkerCount)
            desires += "Workercount desired " + MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_PROBE] + "\n";

        return desires;
    }
        
}