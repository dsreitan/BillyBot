using BillyBot;
using BillyBot.Protoss;
using BillyBot.Terran;
using BillyBot.Zerg;
using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;

Console.WriteLine("Starting BillyBot");

var gameConnection = new GameConnection();
var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

var protossBuildChoices = new ProtossBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Protoss] = protossBuildChoices.BuildChoices;

var terranBuildChoices = new TerranBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Terran] = terranBuildChoices.BuildChoices;

var zergBuildChoices = new ZergBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Zerg] = zergBuildChoices.BuildChoices;

var billyBot = defaultSharkyBot.CreateBot(defaultSharkyBot.Managers, defaultSharkyBot.DebugService);

var mapName = "2000AtmospheresAIE.SC2Map";
var myRace = Race.Terran;
if (args.Length == 0)
    gameConnection.RunSinglePlayer(billyBot, mapName, myRace, Race.Zerg, Difficulty.Hard, AIBuild.RandomBuild).Wait();
else
    gameConnection.RunLadder(billyBot, myRace, args).Wait();