using BillyBot;
using BillyBot.MicroControllers;
using BillyBot.MicroTasks;
using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;

var gameConnection = new GameConnection();
var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

// add dt rush
var warpPrismMicroController = new OffensiveWarpPrismMicroController(defaultSharkyBot, defaultSharkyBot.SharkyPathFinder, MicroPriority.AttackForward, true);
var warpPrismOffenseTask = new WarpPrismOffenseTask(defaultSharkyBot, defaultSharkyBot.MicroController, warpPrismMicroController, new() {new(UnitTypes.PROTOSS_DARKTEMPLAR, 1)}, 999);
defaultSharkyBot.MicroTaskData.MicroTasks[warpPrismOffenseTask.GetType().Name] = warpPrismOffenseTask;

var protossBuildChoices = new ProtossBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Protoss] = protossBuildChoices.BuildChoices;

var sharkyExampleBot = defaultSharkyBot.CreateBot(defaultSharkyBot.Managers, defaultSharkyBot.DebugService);

var myRace = Race.Protoss;
if (args.Length == 0)
    gameConnection.RunSinglePlayer(sharkyExampleBot, @"CuriousMindsAIE.SC2Map", myRace, Race.Zerg, Difficulty.Hard, AIBuild.RandomBuild).Wait();
else
    gameConnection.RunLadder(sharkyExampleBot, myRace, args).Wait();