using BillyBot.Terran.Builds;
using BillyBot.Terran.MicroTasks;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.DefaultBot;
using Sharky.MicroControllers;

namespace BillyBot.Terran;

public class TerranBuildChoices
{
    public TerranBuildChoices(DefaultSharkyBot defaultSharkyBot)
    {
        var hellionRush = new HellionRush(defaultSharkyBot);
        var massVikings = new MassVikings(defaultSharkyBot);
        var bansheesAndMarines = new BansheesAndMarines(defaultSharkyBot);
        var adaptiveOpening = new AdaptiveOpening(defaultSharkyBot);
        var vikingDrops = new VikingDrops(defaultSharkyBot);
        var battleCruiserRush = new BattleCruiserRush(defaultSharkyBot);

        var scvMicroController = new IndividualMicroController(defaultSharkyBot, defaultSharkyBot.SharkyAdvancedPathFinder, MicroPriority.JustLive, false);
        var reaperCheese = new ReaperCheese(defaultSharkyBot, scvMicroController);

        var builds = new Dictionary<string, ISharkyBuild>
        {
            [hellionRush.Name()] = hellionRush,
            [massVikings.Name()] = massVikings,
            [bansheesAndMarines.Name()] = bansheesAndMarines,
            [adaptiveOpening.Name()] = adaptiveOpening,
            [vikingDrops.Name()] = vikingDrops,
            [reaperCheese.Name()] = reaperCheese,
            [battleCruiserRush.Name()] = battleCruiserRush
        };

        var versusTerran = new List<List<string>>
        {
            new() {hellionRush.Name()},
            new() {reaperCheese.Name()},
            new() {vikingDrops.Name()}
        };
        var versusEverything = new List<List<string>>
        {
            // new() {adaptiveOpening.Name()},
            // new() {hellionRush.Name()},
            // new() {massVikings.Name()},
            new() {battleCruiserRush.Name()}
        };
        var transitions = new List<List<string>>
        {
            new() {bansheesAndMarines.Name()}
        };

        var buildSequences = new Dictionary<string, List<List<string>>>
        {
            [Race.Terran.ToString()] = versusTerran,
            [Race.Zerg.ToString()] = versusEverything,
            [Race.Protoss.ToString()] = versusEverything,
            [Race.Random.ToString()] = versusEverything,
            ["Transition"] = transitions
        };

        BuildChoices = new() {Builds = builds, BuildSequences = buildSequences};

        AddTerranTasks(defaultSharkyBot);
    }

    public BuildChoices BuildChoices { get; }

    private void AddTerranTasks(DefaultSharkyBot defaultSharkyBot)
    {
        var vikingDropTask = new VikingDropTask(defaultSharkyBot, .5f, false);
        defaultSharkyBot.MicroTaskData.MicroTasks[vikingDropTask.GetType().Name] = vikingDropTask;
    }
}