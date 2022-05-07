using Sharky;
using Sharky.Builds.BuildChoosing;
using Sharky.DefaultBot;

namespace BillyBot;

public class ProtossCounterTransitioner : ICounterTransitioner
{
    private readonly EnemyData EnemyData;
    private readonly SharkyOptions SharkyOptions;

    public ProtossCounterTransitioner(DefaultSharkyBot defaultSharkyBot)
    {
        EnemyData = defaultSharkyBot.EnemyData;
        SharkyOptions = defaultSharkyBot.SharkyOptions;
    }

    public List<string> DefaultCounterTransition(int frame)
    {
        if (EnemyData.EnemyStrategies["ZerglingRush"].Active && frame < SharkyOptions.FramesPerSecond * 3 * 60) return new() {"ZealotRush"};

        return null;
    }
}