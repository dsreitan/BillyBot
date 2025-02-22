﻿using Sharky.MicroControllers.Protoss;
using Sharky.Pathing;
using Action = SC2APIProtocol.Action;

namespace BillyBot.Protoss.MicroControllers;

public class WarpPrismSupportMicroController : WarpPrismMicroController
{
    public WarpPrismSupportMicroController(DefaultSharkyBot defaultSharkyBot, IPathFinder sharkyPathFinder, MicroPriority microPriority, bool groupUpEnabled) : base(defaultSharkyBot, sharkyPathFinder, microPriority, groupUpEnabled)
    {
    }

    public override List<Action> Support(UnitCommander commander, IEnumerable<UnitCommander> supportTargets, Point2D target, Point2D defensivePoint, Point2D groupCenter, int frame)
    {
        if (!defensivePoint.HasX)
            defensivePoint = TargetingData.MainDefensePoint;
        if (!target.HasX)
            target = TargetingData.EnemyMainBasePoint;
        if (!groupCenter.HasX)
            groupCenter = TargetingData.NaturalBasePoint;

        SupportArmy(commander, target, defensivePoint, groupCenter, frame, out var actions);
        if(actions == null) return new List<Action>();

        return actions;
    }

    public override List<Action> Attack(UnitCommander commander, Point2D target, Point2D defensivePoint, Point2D groupCenter, int frame)
    {
        if (SupportArmy(commander, target, defensivePoint, groupCenter, frame, out var action))
            return action;

        return base.Attack(commander, target, defensivePoint, groupCenter, frame);
    }
}