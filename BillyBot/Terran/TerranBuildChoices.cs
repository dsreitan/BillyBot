using BillyBot.Terran.Builds;
using BillyBot.Terran.MicroTasks;
using SC2APIProtocol;
using Sharky.Builds;
using Sharky.DefaultBot;

namespace BillyBot.Terran;

public class TerranBuildChoices
{
    public TerranBuildChoices(DefaultSharkyBot defaultSharkyBot)
    {
        var battleCruiserRush = new BattleCruiserRush(defaultSharkyBot);

        var builds = new Dictionary<string, ISharkyBuild>
        {
            [battleCruiserRush.Name()] = battleCruiserRush
        };

        var versusEverything = new List<List<string>>
        {
            new() {battleCruiserRush.Name()}
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

        AddTerranTasks(defaultSharkyBot);
    }

    public BuildChoices BuildChoices { get; }

    private void AddTerranTasks(DefaultSharkyBot defaultSharkyBot)
    {
        var vikingDropTask = new VikingDropTask(defaultSharkyBot, .5f, false);
        defaultSharkyBot.MicroTaskData.MicroTasks[vikingDropTask.GetType().Name] = vikingDropTask;
    }
}