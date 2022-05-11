namespace BillyBot.Common;

public static class Extensions
{
    public static int CompletedAndNearlyCompleted(this ActiveUnitData activeUnitData, UnitTypes unitType, float percentageCompleted)
    {
        return activeUnitData.SelfUnits.Count(u =>
            !u.Value.Unit.IsHallucination && u.Value.Unit.UnitType == (uint) unitType
                                          && u.Value.Unit.BuildProgress > percentageCompleted);
    }

    public static void SetMinProductionCount(this MacroData macroData, UnitTypes unitType, int minCount)
    {
        if (macroData.DesiredProductionCounts[unitType] < minCount)
            macroData.DesiredProductionCounts[unitType] = minCount;
    }

    public static void SetMinUnitCount(this MacroData macroData, UnitTypes unitType, int minCount)
    {
        if (macroData.DesiredUnitCounts[unitType] < minCount)
            macroData.DesiredUnitCounts[unitType] = minCount;
    }
}