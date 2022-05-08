using SC2APIProtocol;
using Sharky;
using Sharky.Builds.Terran;
using Sharky.DefaultBot;

namespace BillyBot.Terran.Builds;

public class AdaptiveOpening : TerranSharkyBuild
{
    private readonly EnemyData EnemyData;

    public AdaptiveOpening(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
    {
        EnemyData = defaultSharkyBot.EnemyData;
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);

        BuildOptions.StrictGasCount = true;

        MicroTaskData.MicroTasks["WorkerScoutTask"].Enable();
        MicroTaskData.MicroTasks["ProxyScoutTask"].Enable();
    }

    public override List<string> CounterTransition(int frame)
    {
        if (UnitCountService.EnemyCount(UnitTypes.ZERG_HATCHERY) > 1 || EnemyData.EnemyStrategies["Proxy"].Active) return new() {"HellionRush"};

        if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_ORBITALCOMMAND) > 0) return new() {"BansheesAndMarines"};

        return new();
    }

    public override void EndBuild(int frame)
    {
        ChatService.SendChatMessage("I know what to do");
    }

    public override void OnFrame(ResponseObservation observation)
    {
        var frame = (int) observation.Observation.GameLoop;

        SendScvForFirstDepot(frame);

        if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_SUPPLYDEPOT) > 0)
        {
            if (MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] < 1) MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 1;
        }


        if (UnitCountService.EquivalentTypeCompleted(UnitTypes.TERRAN_BARRACKS) > 0)
        {
            if (MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] < 1) MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 1;
            if (MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] < 10) MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] = 10;
        }
    }
}