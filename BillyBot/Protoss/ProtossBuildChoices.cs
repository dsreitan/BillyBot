using BillyBot.MicroControllers;
using BillyBot.MicroTasks;
using BillyBot.Protoss.Builds;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.DefaultBot;

namespace BillyBot.Protoss;

public class ProtossBuildChoices
{
    public ProtossBuildChoices(DefaultSharkyBot defaultSharkyBot)
    {
        var protossCounterTransitioner = new ProtossCounterTransitioner(defaultSharkyBot);

        var macroOpener = new MacroOpener(defaultSharkyBot, protossCounterTransitioner);
        var dtRobo = new DtRobo(defaultSharkyBot, protossCounterTransitioner);
        var roboSentry = new RoboSentry(defaultSharkyBot, protossCounterTransitioner);

        var builds = new Dictionary<string, ISharkyBuild>
        {
            [macroOpener.Name()] = macroOpener,
            [dtRobo.Name()] = dtRobo,
            [roboSentry.Name()] = roboSentry
        };

        var versusEverything = new List<List<string>>
        {
            new() {macroOpener.Name(), roboSentry.Name()}
        };

        var buildSequences = new Dictionary<string, List<List<string>>>
        {
            [Race.Terran.ToString()] = versusEverything,
            [Race.Zerg.ToString()] = versusEverything,
            [Race.Protoss.ToString()] = versusEverything,
            [Race.Random.ToString()] = versusEverything,
            ["Transition"] = versusEverything
        };

        BuildChoices = new() {Builds = builds, BuildSequences = buildSequences};

        AddProtossTasks(defaultSharkyBot);
    }

    public BuildChoices BuildChoices { get; }

    private void AddProtossTasks(DefaultSharkyBot defaultSharkyBot)
    {
        var warpPrismMicroController = new OffensiveWarpPrismMicroController(defaultSharkyBot, defaultSharkyBot.SharkyPathFinder, MicroPriority.AttackForward, true);
        var warpPrismOffenseTask = new WarpPrismOffenseTask(defaultSharkyBot, defaultSharkyBot.MicroController, warpPrismMicroController, new() {new(UnitTypes.PROTOSS_DARKTEMPLAR, 1)}, 999);
        defaultSharkyBot.MicroTaskData.MicroTasks[warpPrismOffenseTask.GetType().Name] = warpPrismOffenseTask;
    }
}