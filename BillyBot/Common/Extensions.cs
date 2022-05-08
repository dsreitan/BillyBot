using Sharky;

namespace BillyBot.Common;

public static class Extensions
{
    public static int CompletedAndNearlyCompleted(this ActiveUnitData activeUnitData, UnitTypes unitType, float percentageCompleted)
    {
        return activeUnitData.SelfUnits.Count(u =>
            !u.Value.Unit.IsHallucination && u.Value.Unit.UnitType == (uint) unitType
                                          && u.Value.Unit.BuildProgress > percentageCompleted);
    }
}