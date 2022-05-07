using BillyBot.Builds;
using BillyBot.MicroTasks;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.DefaultBot;
using Sharky.MicroControllers;

namespace BillyBot;

public class ProtossBuildChoices
{
    public ProtossBuildChoices(DefaultSharkyBot defaultSharkyBot)
    {
        // we can use this to switch builds mid-game if we detect certain strategies (it can also be handled specificly for each build)
        var protossCounterTransitioner = new ProtossCounterTransitioner(defaultSharkyBot);

        // a probe microcontroller for our proxy builds
        var probeMicroController = new IndividualMicroController(defaultSharkyBot, defaultSharkyBot.SharkyAdvancedPathFinder, MicroPriority.JustLive, false);

        // We create all of our builds
        var macroOpener = new MacroOpener(defaultSharkyBot, protossCounterTransitioner);
        var dtRobo = new DtRobo(defaultSharkyBot, protossCounterTransitioner);
        var roboSentry = new RoboSentry(defaultSharkyBot, protossCounterTransitioner);

        // We add all the builds to a build dictionary.  Every protoss build your bot uses must be included here.
        var builds = new Dictionary<string, ISharkyBuild>
        {
            [macroOpener.Name()] = macroOpener,
            [dtRobo.Name()] = dtRobo,
            [roboSentry.Name()] = roboSentry,
        };

        // we create build sequences to be used by each matchup
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
    }

    public BuildChoices BuildChoices { get; }
}