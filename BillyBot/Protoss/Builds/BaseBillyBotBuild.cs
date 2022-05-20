using System.Text.RegularExpressions;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;

namespace BillyBot.Protoss.Builds;

public class BaseBillyBotBuild : ProtossSharkyBuild
{
    protected bool showAllreadyAquired;

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

    }


    protected void debugChat(ResponseObservation observation) { 
    
  
        var chat = observation.Chat;
        foreach (var chatReceived in chat)
        {
            var match = Regex.Match(chatReceived.Message.ToLower(), @"desires");
            if (match.Success)
            {
                var desires = desiredDebug();
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

        var productionCapacityInPylons = productionCapacity / 8;
        var pylonsCurrentlyUsed = (MacroData.FoodUsed - nexusCount * 15) / 8.0;


        MacroData.DesiredPylons = (int) Math.Ceiling(pylonsCurrentlyUsed + productionCapacityInPylons);
    }

    protected void AdeptHarass()
    {
        if (UnitCountService.Completed(UnitTypes.PROTOSS_ADEPT) <= 2)
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ADEPT] = 2;
    }

    protected void MakeGateways(int desired)
    {
        var currentGates = UnitCountService.Count(UnitTypes.PROTOSS_GATEWAY) + UnitCountService.Count(UnitTypes.PROTOSS_WARPGATE);
        var currentGatesProgress = UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.PROTOSS_GATEWAY);
        if (currentGates < desired && currentGatesProgress < desired) MacroData.DesiredProductionCounts[UnitTypes.PROTOSS_GATEWAY] = currentGates + 1;
    }

    protected void desiredDebug(int frame, int delay)
    {
        if (frame % delay == 0) Console.WriteLine(desiredDebug());
    }


    protected string desiredDebug()
    {
        var desires = showAllreadyAquired ? "Shows Aquired" + "\n" : "Hides Aquired" + "\n";
        desires += prodDesires();
        desires += techDesires();
        desires += unitDesires();
        desires += morphDesires();
        desires += upgradeDesires();
        desires += gasDesires();
        desires += workerDesires();
        desires += "buildpylon " + MacroData.BuildPylon;
        desires += "desiredpylons " + MacroData.DesiredPylons;
        return desires;
    }


    

    private string prodDesires()
    {
        var prodDesires = "";
        foreach (var u in MacroData.DesiredProductionCounts)
            if (MacroData.BuildProduction[u.Key])
            {
                if (prodDesires.Length == 0) prodDesires += "production: \n";
                prodDesires += u + ":" + MacroData.BuildProduction[u.Key] + " want " + u.Value + "\n";
            }
            else if (showAllreadyAquired) prodDesires += u + ":" + MacroData.BuildProduction[u.Key] + " want " + u.Value + "\n";

        return prodDesires;
    }

    private string techDesires()
    {
        var techDesires = "";
        foreach (var u in MacroData.DesiredTechCounts)
            if (MacroData.BuildTech[u.Key])
            {
                if (techDesires.Length == 0) techDesires += "techs: \n";
                techDesires += u + ":" + MacroData.BuildTech[u.Key] + " want " + u.Value + "\n";
            }
            else if (showAllreadyAquired) techDesires += u + ":" + MacroData.BuildTech[u.Key] + " want " + u.Value + "\n";

        return techDesires;
    }

    private string unitDesires()
    {
        var unitDesires = "";
        var allUnits = MacroData.NexusUnits;
        allUnits.AddRange(MacroData.GatewayUnits);
        allUnits.AddRange(MacroData.RoboticsFacilityUnits);
        allUnits.AddRange(MacroData.StargateUnits);
        foreach (var u in MacroData.DesiredUnitCounts)
            if (MacroData.BuildUnits[u.Key])
            {
                if (unitDesires.Length == 0) unitDesires += "units \n";
                unitDesires += u + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";
            }
            else if (showAllreadyAquired) unitDesires += u + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";

        return unitDesires;
    }

    private string morphDesires()
    {
        var morphDesires = "";
        foreach (var u in MacroData.DesiredMorphCounts)
            if (MacroData.Morph[u.Key])
            {
                if (morphDesires.Length == 0) morphDesires += "morphs \n";
                morphDesires += u + ":" + MacroData.Morph[u.Key] + " want " + u.Value + "\n";
            }
            else if (showAllreadyAquired) morphDesires += u + ":" + MacroData.BuildUnits[u.Key] + " want " + u.Value + "\n";

        return morphDesires;
    }

    private string upgradeDesires()
    {
        var upgradeDesires = "";
        foreach (var u in MacroData.DesiredUpgrades)
            if (u.Value && !UnitCountService.UpgradeDoneOrInProgress(u.Key)) upgradeDesires += u + ":" + u.Value + "\n";
            else if (showAllreadyAquired) upgradeDesires += u + ":" + u.Value + "\n";
        return upgradeDesires;
    }

    private string gasDesires()
    {
        if (BuildOptions.StrictGasCount)
            return "Gascount desired " + MacroData.DesiredGases + "\n";
        return "";
    }

    private string workerDesires()
    {
        if (BuildOptions.StrictWorkerCount)
            return "Workercount desired " + MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_PROBE] + "\n";
        return "";
    }
}