using Sharky.Builds.BuildChoosing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillyBot.Protoss.Builds;

public class DebugBuildingBlockBuild : BaseBillyBotBuild
{
    public DebugBuildingBlockBuild(DefaultSharkyBot defaultSharkyBot, ICounterTransitioner counterTransitioner) : base(defaultSharkyBot, counterTransitioner)
    { }

    public override void StartBuild(int frame)
    {
        BuildOptions.StrictSupplyCount = true;

    }

    public override void OnFrame(ResponseObservation observation)
    {


        MakeGateways(10);

        MacroData.DesiredPylons = 1;

    }

}
