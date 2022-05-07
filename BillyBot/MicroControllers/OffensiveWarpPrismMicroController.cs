using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;
using Sharky.MicroControllers.Protoss;
using Sharky.Pathing;
using Action = SC2APIProtocol.Action;

namespace BillyBot.MicroControllers;

public class OffensiveWarpPrismMicroController : WarpPrismMicroController
{
    public OffensiveWarpPrismMicroController(DefaultSharkyBot defaultSharkyBot, IPathFinder sharkyPathFinder, MicroPriority microPriority, bool groupUpEnabled) : base(defaultSharkyBot, sharkyPathFinder, microPriority, groupUpEnabled)
    {
    }

    public override List<Action> Attack(UnitCommander commander, Point2D target, Point2D defensivePoint, Point2D groupCenter, int frame)
    {
        var action = new List<SC2APIProtocol.Action>();

        if (SupportArmy(commander, target, defensivePoint, groupCenter, frame, out action))
        {
            return action;
        }
        return base.Attack(commander, target, defensivePoint, groupCenter, frame);
    }
}