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

        var testy = Math.Ceiling(((MacroData.FoodUsed - (nexusCount * 12)) / 8.0) + (productionCapacity / 8.0));

    }

    protected void AdeptHarass()
    {
        if (UnitCountService.Completed(UnitTypes.PROTOSS_ADEPT) <= 2)
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ADEPT] = 2;
    }
}