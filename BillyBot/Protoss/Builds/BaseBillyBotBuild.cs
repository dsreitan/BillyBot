using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;
using Sharky.TypeData;

namespace BillyBot.Builds;

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
    ///     Override strict supply false
    /// </summary>
    protected void BalancePylons()
    {
        var nexusCount = CompletedAndNearlyCompleted(UnitTypes.PROTOSS_NEXUS, .90f);
        var gatewayCount = CompletedAndNearlyCompleted(UnitTypes.PROTOSS_GATEWAY, .90f);
        var warpgateCount = CompletedAndNearlyCompleted(UnitTypes.PROTOSS_WARPGATE, .90f);
        var roboCount = CompletedAndNearlyCompleted(UnitTypes.PROTOSS_ROBOTICSFACILITY, .90f);
        var stargateCount = CompletedAndNearlyCompleted(UnitTypes.PROTOSS_STARGATE, .90f);

        var productionCapacity = nexusCount +
                                 (gatewayCount + warpgateCount) * 2 +
                                 roboCount * 6 +
                                 stargateCount * 6;
        MacroData.DesiredPylons = (int) Math.Ceiling((MacroData.FoodUsed - nexusCount * 12) / 8.0 + productionCapacity / 8.0);
    }

    protected int CompletedAndNearlyCompleted(UnitTypes unitType, float percentageCompleted)
    {
        return ActiveUnitData.SelfUnits.Count(u =>
            !u.Value.Unit.IsHallucination && u.Value.Unit.UnitType == (uint) unitType
                                          && u.Value.Unit.BuildProgress > percentageCompleted);
    }

    protected void AdeptHarass()
    {
        if (UnitCountService.Completed(UnitTypes.PROTOSS_ADEPT) <= 2)
            MacroData.DesiredUnitCounts[UnitTypes.PROTOSS_ADEPT] = 2;
    }
}