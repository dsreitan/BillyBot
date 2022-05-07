﻿using BillyBot;
using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;

var gameConnection = new GameConnection();
var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

var protossBuildChoices = new ProtossBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Protoss] = protossBuildChoices.BuildChoices;

var sharkyExampleBot = defaultSharkyBot.CreateBot(defaultSharkyBot.Managers, defaultSharkyBot.DebugService);

var myRace = Race.Protoss;
if (args.Length == 0)
{
    gameConnection.RunSinglePlayer(sharkyExampleBot, @"2000AtmospheresAIE.SC2Map", myRace, Race.Zerg, Difficulty.VeryHard, AIBuild.RandomBuild).Wait();
}
else
{
    gameConnection.RunLadder(sharkyExampleBot, myRace, args).Wait();
}