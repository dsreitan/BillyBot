﻿using BillyBot;
using BillyBot.Protoss;
using BillyBot.Sharky.Builds.BuildingPlacement;
using BillyBot.Terran;
using BillyBot.Zerg;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildingPlacement;
using Sharky.DefaultBot;
using Sharky.Macro;
using Sharky.Builds.MacroServices;
using Sharky.Managers;

Console.WriteLine("Starting BillyBot");

var gameConnection = new GameConnection();
var defaultSharkyBot = new DefaultSharkyBot(gameConnection);

var protossBuildChoices = new ProtossBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Protoss] = protossBuildChoices.BuildChoices;

var terranBuildChoices = new TerranBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Terran] = terranBuildChoices.BuildChoices;

var zergBuildChoices = new ZergBuildChoices(defaultSharkyBot);
defaultSharkyBot.BuildChoices[Race.Zerg] = zergBuildChoices.BuildChoices;

setMacroManager(defaultSharkyBot);
defaultSharkyBot.Managers = NewManagers(defaultSharkyBot);


var billyBot = defaultSharkyBot.CreateBot(defaultSharkyBot.Managers, defaultSharkyBot.DebugService);

var mapName = "HardwireAIE.SC2Map";
var myRace = Race.Protoss;
if (args.Length == 0)
    gameConnection.RunSinglePlayer(billyBot, mapName, myRace, Race.Zerg, Difficulty.Hard, AIBuild.RandomBuild).Wait();
else
    gameConnection.RunLadder(billyBot, myRace, args).Wait();

static void setMacroManager(DefaultSharkyBot? defaultSharkyBot)
{
    var protossBuildingPlacement = new ProtossBuildingPlacementCopy(defaultSharkyBot);
    defaultSharkyBot.ProtossBuildingPlacement = protossBuildingPlacement;

    var buildingPlacement = new BuildingPlacement(protossBuildingPlacement, defaultSharkyBot.TerranBuildingPlacement, defaultSharkyBot.ZergBuildingPlacement, defaultSharkyBot.ResourceCenterLocator, defaultSharkyBot.BaseData, defaultSharkyBot.SharkyUnitData, defaultSharkyBot.MacroData, defaultSharkyBot.UnitCountService);
    defaultSharkyBot.BuildingPlacement = buildingPlacement;

    var buildBuilder = new BuildingBuilder(defaultSharkyBot.ActiveUnitData, defaultSharkyBot.TargetingData, defaultSharkyBot.BuildingPlacement, defaultSharkyBot.SharkyUnitData, defaultSharkyBot.BaseData, defaultSharkyBot.BuildingService, defaultSharkyBot.MapDataService, defaultSharkyBot.WorkerBuilderService);
    defaultSharkyBot.BuildingBuilder = buildBuilder;

    //This part uses the new buildingbuilder
    defaultSharkyBot.BuildPylonService = new BuildPylonService(defaultSharkyBot.MacroData, defaultSharkyBot.BuildingBuilder, defaultSharkyBot.SharkyUnitData, defaultSharkyBot.ActiveUnitData, defaultSharkyBot.BaseData, defaultSharkyBot.TargetingData, defaultSharkyBot.BuildingService);
    defaultSharkyBot.BuildDefenseService = new BuildDefenseService(defaultSharkyBot.MacroData, defaultSharkyBot.BuildingBuilder, defaultSharkyBot.SharkyUnitData, defaultSharkyBot.ActiveUnitData, defaultSharkyBot.BaseData, defaultSharkyBot.TargetingData, defaultSharkyBot.BuildOptions, defaultSharkyBot.BuildingService);

    defaultSharkyBot.BuildProxyService = new BuildProxyService(defaultSharkyBot.MacroData, defaultSharkyBot.BuildingBuilder, defaultSharkyBot.SharkyUnitData, defaultSharkyBot.ActiveUnitData, defaultSharkyBot.Morpher, defaultSharkyBot.MicroTaskData);
    defaultSharkyBot.VespeneGasBuilder = new VespeneGasBuilder(defaultSharkyBot, defaultSharkyBot.BuildingBuilder);

    defaultSharkyBot.SupplyBuilder = new SupplyBuilder(defaultSharkyBot, buildBuilder);
    defaultSharkyBot.ProductionBuilder = new ProductionBuilder(defaultSharkyBot, buildBuilder);
    defaultSharkyBot.TechBuilder = new TechBuilder(defaultSharkyBot, buildBuilder);
    defaultSharkyBot.AddOnBuilder = new AddOnBuilder(defaultSharkyBot, defaultSharkyBot.BuildingBuilder);

    defaultSharkyBot.MacroManager = new MacroManager(defaultSharkyBot);

}

static List<IManager> NewManagers(DefaultSharkyBot? defaultSharkyBot)
{
    var Managers = new List<IManager>();
    Managers.Add(defaultSharkyBot.DebugManager);
    Managers.Add(defaultSharkyBot.UnitDataManager);
    Managers.Add(defaultSharkyBot.MapManager);
    Managers.Add(defaultSharkyBot.UnitManager);
    Managers.Add(defaultSharkyBot.EnemyRaceManager);
    Managers.Add(defaultSharkyBot.BaseManager);
    Managers.Add(defaultSharkyBot.TargetingManager);
    Managers.Add(defaultSharkyBot.NexusManager);
    Managers.Add(defaultSharkyBot.ShieldBatteryManager);
    Managers.Add(defaultSharkyBot.PhotonCannonManager);
    Managers.Add(defaultSharkyBot.RallyPointManager);
    Managers.Add(defaultSharkyBot.OrbitalManager);
    Managers.Add(defaultSharkyBot.SupplyDepotManager);
    Managers.Add(defaultSharkyBot.ChatManager);
    Managers.Add(defaultSharkyBot.MicroManager);
    Managers.Add(defaultSharkyBot.AttackDataManager);
    Managers.Add(defaultSharkyBot.MacroManager);
    Managers.Add(defaultSharkyBot.EnemyStrategyManager);
    Managers.Add(defaultSharkyBot.BuildManager);

    return Managers;

}