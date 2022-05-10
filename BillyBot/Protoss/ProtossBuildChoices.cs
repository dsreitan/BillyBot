using BillyBot.Protoss.Builds;
using BillyBot.Protoss.MicroTasks;
using SC2APIProtocol;
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
            new() {macroOpener.Name(), dtRobo.Name()}
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
        var warpPrismOffenseTask = new WarpPrismOffenseTask(defaultSharkyBot, defaultSharkyBot.MicroController, -1);
        defaultSharkyBot.MicroTaskData.MicroTasks[nameof(WarpPrismOffenseTask)] = warpPrismOffenseTask;
    }
}