using BillyBot.Zerg.Builds;
using BillyBot.Zerg.MicroTasks;
using SC2APIProtocol;
using Sharky.Builds;
using Sharky.Builds.Zerg;
using Sharky.DefaultBot;

namespace BillyBot.Zerg;

public class ZergBuildChoices
{
    public ZergBuildChoices(DefaultSharkyBot defaultSharkyBot)
    {
        var zerglingRush = new BasicZerglingRush(defaultSharkyBot);
        var roachRush = new RoachRush(defaultSharkyBot);
        var mutaliskRush = new MutaliskRush(defaultSharkyBot);

        var builds = new Dictionary<string, ISharkyBuild>
        {
            [zerglingRush.Name()] = zerglingRush,
            [roachRush.Name()] = roachRush,
            [mutaliskRush.Name()] = mutaliskRush
        };

        var versusEverything = new List<List<string>>
        {
            new() {roachRush.Name(), mutaliskRush.Name()},
        };

        var transitions = new List<List<string>>
        {
            new() {mutaliskRush.Name()}
        };

        var buildSequences = new Dictionary<string, List<List<string>>>
        {
            [Race.Terran.ToString()] = versusEverything,
            [Race.Zerg.ToString()] = versusEverything,
            [Race.Protoss.ToString()] = versusEverything,
            [Race.Random.ToString()] = versusEverything,
            ["Transition"] = transitions
        };

        BuildChoices = new() {Builds = builds, BuildSequences = buildSequences};

        AddZergTasks(defaultSharkyBot);
    }

    public BuildChoices BuildChoices { get; }

    private void AddZergTasks(DefaultSharkyBot defaultSharkyBot)
    {
        var overlordScoutTask = new OverlordScoutTask(defaultSharkyBot, 2, true);
        defaultSharkyBot.MicroTaskData.MicroTasks[overlordScoutTask.GetType().Name] = overlordScoutTask;
    }
}