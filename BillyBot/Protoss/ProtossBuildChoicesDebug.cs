using BillyBot.Protoss.Builds;
using BillyBot.Protoss.MicroTasks;
using SC2APIProtocol;
using Sharky.Builds;
using Sharky.DefaultBot;

namespace BillyBot.Protoss;

public class ProtossBuildChoicesDebug
{
    public ProtossBuildChoicesDebug(DefaultSharkyBot defaultSharkyBot)
    {
        var protossCounterTransitioner = new ProtossCounterTransitioner(defaultSharkyBot);

        var buildingBlockBuild = new DebugBuildingBlockBuild(defaultSharkyBot, protossCounterTransitioner);
       

        var builds = new Dictionary<string, ISharkyBuild>
        {
            [buildingBlockBuild.Name()] = buildingBlockBuild,
        };

        var versusEverything = new List<List<string>>
        {
            new() {buildingBlockBuild.Name()}
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

    }
}