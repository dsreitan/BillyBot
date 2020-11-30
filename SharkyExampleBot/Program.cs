﻿using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.BuildChoosing;
using Sharky.Builds.BuildingPlacement;
using Sharky.Builds.MacroServices;
using Sharky.Builds.Protoss;
using Sharky.Builds.Terran;
using Sharky.Builds.Zerg;
using Sharky.Chat;
using Sharky.EnemyPlayer;
using Sharky.EnemyStrategies;
using Sharky.EnemyStrategies.Protoss;
using Sharky.EnemyStrategies.Terran;
using Sharky.EnemyStrategies.Zerg;
using Sharky.Managers;
using Sharky.Managers.Protoss;
using Sharky.MicroControllers;
using Sharky.MicroControllers.Protoss;
using Sharky.MicroControllers.Zerg;
using Sharky.MicroTasks;
using Sharky.Pathing;
using Sharky.TypeData;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SharkyExampleBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("starting");
            var gameConnection = new GameConnection();
            var sharkyBot = CreateBot(gameConnection);

            var myRace = Race.Protoss;
            if (args.Length == 0)
            {
                gameConnection.RunSinglePlayer(sharkyBot, @"AutomatonLE.SC2Map", myRace, Race.Random, Difficulty.VeryEasy).Wait();
            }
            else
            {
                gameConnection.RunLadder(sharkyBot, myRace, args).Wait();
            }
        }

        // TODO: defaultBot where you just pass in builds

        private static SharkyBot CreateBot(GameConnection gameConnection)
        {
            var debug = false;
#if DEBUG
            debug = true;
#endif

            var framesPerSecond = 22.4f;

            var sharkyOptions = new SharkyOptions { Debug = debug, FramesPerSecond = framesPerSecond };

            var managers = new List<IManager>();

            var debugManager = new DebugManager(gameConnection, sharkyOptions);
            managers.Add(debugManager);

            var upgradeDataService = new UpgradeDataService();
            var buildingDataService = new BuildingDataService();
            var trainingDataService = new TrainingDataService();
            var addOnDataService = new AddOnDataService();
            var morphDataService = new MorphDataService();

            var unitDataManager = new UnitDataManager(upgradeDataService, buildingDataService, trainingDataService, addOnDataService, morphDataService);
            managers.Add(unitDataManager);
            var mapData = new MapData();
            var mapManager = new MapManager(mapData);
            managers.Add(mapManager);

            var mapDataService = new MapDataService(mapData);

            var targetPriorityService = new TargetPriorityService(unitDataManager);
            var collisionCalculator = new CollisionCalculator();
            var unitManager = new UnitManager(unitDataManager, sharkyOptions, targetPriorityService, collisionCalculator, mapDataService, debugManager);
            managers.Add(unitManager);

            var enemyRaceManager = new EnemyRaceManager(unitManager, unitDataManager);
            managers.Add(enemyRaceManager);

            var baseManager = new BaseManager(unitDataManager, unitManager);
            managers.Add(baseManager);

            var targetingManager = new TargetingManager(unitManager, unitDataManager, mapDataService, baseManager);
            managers.Add(targetingManager);

            var buildOptions = new BuildOptions { StrictGasCount = false, StrictSupplyCount = false, StrictWorkerCount = false };
            var macroSetup = new MacroSetup();
            var buildingService = new BuildingService(mapData, unitManager);
            var protossBuildingPlacement = new ProtossBuildingPlacement(unitManager, unitDataManager, debugManager, mapData, buildingService);
            var terranBuildingPlacement = new TerranBuildingPlacement(unitManager, unitDataManager, debugManager, buildingService);
            var zergBuildingPlacement = new ZergBuildingPlacement(unitManager, unitDataManager, debugManager, buildingService);
            var buildingPlacement = new BuildingPlacement(protossBuildingPlacement, terranBuildingPlacement, zergBuildingPlacement, baseManager, unitManager, buildingService, unitDataManager);
            var buildingBuilder = new BuildingBuilder(unitManager, targetingManager, buildingPlacement, unitDataManager);


            var attackData = new AttackData { ArmyFoodAttack = 30, Attacking = false, CustomAttackFunction = false };
            var warpInPlacement = new WarpInPlacement(unitManager, debugManager, mapData);
            var macroData = new MacroData();
            var morpher = new Morpher(unitManager, unitDataManager, sharkyOptions);
            var buildPylonService = new BuildPylonService(macroData, buildingBuilder, unitDataManager, unitManager, baseManager, targetingManager);
            var buildDefenseService = new BuildDefenseService(macroData, buildingBuilder, unitDataManager, unitManager, baseManager, targetingManager);
            var macroManager = new MacroManager(macroSetup, unitManager, unitDataManager, buildingBuilder, sharkyOptions, baseManager, targetingManager, attackData, warpInPlacement, macroData, morpher, buildPylonService, buildDefenseService);
            managers.Add(macroManager);

            var nexusManager = new NexusManager(unitManager, unitDataManager);
            managers.Add(nexusManager);

            var httpClient = new HttpClient();
            var chatHistory = new ChatHistory();
            var chatDataService = new ChatDataService();
            var enemyNameService = new EnemyNameService();
            var enemyPlayerService = new EnemyPlayerService(enemyNameService);
            var chatManager = new ChatManager(httpClient, chatHistory, sharkyOptions, chatDataService, enemyPlayerService, enemyNameService);
            managers.Add(chatManager);

            var sharkyPathFinder = new SharkyPathFinder(new Roy_T.AStar.Paths.PathFinder(), mapData, mapDataService);
            var sharkySimplePathFinder = new SharkySimplePathFinder(mapDataService);
            var noPathFinder = new SharkyNoPathFinder();

            var individualMicroController = new IndividualMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.LiveAndAttack, false);
        
            var colossusMicroController = new ColossusMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false, collisionCalculator);
            var darkTemplarMicroController = new DarkTemplarMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var disruptorMicroController = new DisruptorMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var disruptorPhasedMicroController = new DisruptorPhasedMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var mothershipMicroController = new MothershipMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var oraclepMicroController = new OracleMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var phoenixMicroController = new PhoenixMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var sentryMicroController = new SentryMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.StayOutOfRange, true);
            var stalkerMicroController = new StalkerMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var tempestMicroController = new TempestMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var voidrayMicroController = new VoidRayMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var warpPrismpMicroController = new WarpPrismMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false, baseManager);
            var zealotMicroController = new ZealotMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            var observerMicroController = new IndividualMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.StayOutOfRange, true);

            var zerglingMicroController = new ZerglingMicroController(mapDataService, unitDataManager, unitManager, debugManager, noPathFinder, sharkyOptions, MicroPriority.AttackForward, false);
            
            var individualMicroControllers = new Dictionary<UnitTypes, IIndividualMicroController>
            {               
                { UnitTypes.PROTOSS_COLOSSUS, colossusMicroController },
                { UnitTypes.PROTOSS_DARKTEMPLAR, darkTemplarMicroController },
                { UnitTypes.PROTOSS_DISRUPTOR, disruptorMicroController },
                { UnitTypes.PROTOSS_DISRUPTORPHASED, disruptorPhasedMicroController },
                { UnitTypes.PROTOSS_MOTHERSHIP, mothershipMicroController },
                { UnitTypes.PROTOSS_ORACLE, oraclepMicroController },
                { UnitTypes.PROTOSS_PHOENIX, phoenixMicroController },
                { UnitTypes.PROTOSS_SENTRY, sentryMicroController },
                { UnitTypes.PROTOSS_STALKER, stalkerMicroController },
                { UnitTypes.PROTOSS_TEMPEST, tempestMicroController },
                { UnitTypes.PROTOSS_VOIDRAY, voidrayMicroController },
                { UnitTypes.PROTOSS_WARPPRISM, warpPrismpMicroController },
                { UnitTypes.PROTOSS_WARPPRISMPHASING, warpPrismpMicroController },
                { UnitTypes.PROTOSS_ZEALOT, zealotMicroController },
                { UnitTypes.PROTOSS_OBSERVER, observerMicroController },

                { UnitTypes.ZERG_ZERGLING, zerglingMicroController }
            };

            var defenseService = new DefenseService(unitManager);
            var microController = new MicroController(individualMicroControllers, individualMicroController);

            var defenseSquadTask = new DefenseSquadTask(unitManager, targetingManager, defenseService, microController, new List<DesiredUnitsClaim>(), 0, false);
            var workerScoutTask = new WorkerScoutTask(unitDataManager, targetingManager, mapDataService, true, 0.5f);
            var miningTask = new MiningTask(unitDataManager, baseManager, unitManager, 1, collisionCalculator, debugManager);          
            var attackTask = new AttackTask(microController, targetingManager, unitManager, defenseService, macroData, attackData, 2);

            var microTasks = new Dictionary<string, IMicroTask>
            {
                [defenseSquadTask.GetType().Name] = defenseSquadTask,
                [workerScoutTask.GetType().Name] = workerScoutTask,
                [miningTask.GetType().Name] = miningTask,
                [attackTask.GetType().Name] = attackTask
            };

            var microManager = new MicroManager(unitManager, microTasks);
            managers.Add(microManager);

            var enemyStrategyHistory = new EnemyStrategyHistory();
            var enemyStrategies = new Dictionary<string, IEnemyStrategy>
            {
                ["Proxy"] = new Proxy(enemyStrategyHistory, chatManager, unitManager, sharkyOptions, targetingManager),
                ["WorkerRush"] = new WorkerRush(enemyStrategyHistory, chatManager, unitManager, sharkyOptions, targetingManager),
                ["AdeptRush"] = new AdeptRush(enemyStrategyHistory, chatManager, unitManager, sharkyOptions),
                ["MarineRush"] = new MarineRush(enemyStrategyHistory, chatManager, unitManager, sharkyOptions),
                ["MassVikings"] = new MassVikings(enemyStrategyHistory, chatManager, unitManager, sharkyOptions),
                ["ZerglingRush"] = new ZerglingRush(enemyStrategyHistory, chatManager, unitManager, sharkyOptions)
            };

            var enemyStrategyManager = new EnemyStrategyManager(enemyStrategies);
            managers.Add(enemyStrategyManager);

            var protossCounterTransitioner = new ProtossCounterTransitioner(enemyStrategyManager, sharkyOptions);

            var antiMassMarine = new AntiMassMarine(buildOptions, macroData, unitManager, attackData, chatManager, nexusManager);
            var fourGate = new FourGate(buildOptions, macroData, unitManager, attackData, chatManager, nexusManager, unitDataManager);
            var nexusFirst = new NexusFirst(buildOptions, macroData, unitManager, attackData, chatManager, nexusManager, protossCounterTransitioner);
            var robo = new Robo(buildOptions, macroData, unitManager, attackData, chatManager, nexusManager);
            var protossRobo = new ProtossRobo(buildOptions, macroData, unitManager, attackData, chatManager, nexusManager, sharkyOptions, microManager, enemyRaceManager);
            var everyProtossUnit = new EveryProtossUnit(buildOptions, macroData, unitManager, attackData, chatManager, nexusManager);

            var protossBuilds = new Dictionary<string, ISharkyBuild>
            {
                [everyProtossUnit.Name()] = everyProtossUnit,
                [nexusFirst.Name()] = nexusFirst,
                [robo.Name()] = robo,
                [protossRobo.Name()] = protossRobo,
                [fourGate.Name()] = fourGate,
                [antiMassMarine.Name()] = antiMassMarine
            };
            var protossSequences = new List<List<string>>
            {
                new List<string> { everyProtossUnit.Name() },
                //new List<string> { nexusFirst.Name(), robo.Name(), protossRobo.Name() },
                //new List<string> { antiMassMarine.Name() },
                //new List<string> { fourGate.Name() }
            };
            var protossBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = protossSequences,
                [Race.Zerg.ToString()] = protossSequences,
                [Race.Protoss.ToString()] = protossSequences,
                [Race.Random.ToString()] = protossSequences,
                ["Transition"] = protossSequences
            };

            var massMarine = new MassMarines(buildOptions, macroData, unitManager, attackData, chatManager);
            var battleCruisers = new BattleCruisers(buildOptions, macroData, unitManager, attackData, chatManager);
            var everyTerranUnit = new EveryTerranUnit(buildOptions, macroData, unitManager, attackData, chatManager);
            var terranBuilds = new Dictionary<string, ISharkyBuild>
            {
                [massMarine.Name()] = massMarine,
                [battleCruisers.Name()] = battleCruisers,
                [everyTerranUnit.Name()] = everyTerranUnit
            };
            var terranSequences = new List<List<string>>
            {
                new List<string> { massMarine.Name(), battleCruisers.Name() },
                new List<string> { everyTerranUnit.Name() },
                new List<string> { battleCruisers.Name() },
            };
            var terranBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = terranSequences,
                [Race.Zerg.ToString()] = terranSequences,
                [Race.Protoss.ToString()] = terranSequences,
                [Race.Random.ToString()] = terranSequences,
                ["Transition"] = terranSequences
            };

            var basicZerglingRush = new BasicZerglingRush(buildOptions, macroData, unitManager, attackData, chatManager, microManager);
            var everyZergUnit = new EveryZergUnit(buildOptions, macroData, unitManager, attackData, chatManager);
            var zergBuilds = new Dictionary<string, ISharkyBuild>
            {
                [everyZergUnit.Name()] = everyZergUnit,
                [basicZerglingRush.Name()] = basicZerglingRush
            };
            var zergSequences = new List<List<string>>
            {
                new List<string> { everyZergUnit.Name() },
                new List<string> { basicZerglingRush.Name(), everyZergUnit.Name() }
            };
            var zergBuildSequences = new Dictionary<string, List<List<string>>>
            {
                [Race.Terran.ToString()] = zergSequences,
                [Race.Zerg.ToString()] = zergSequences,
                [Race.Protoss.ToString()] = zergSequences,
                [Race.Random.ToString()] = zergSequences,
                ["Transition"] = zergSequences
            };

            var macroBalancer = new MacroBalancer(buildOptions, unitManager, macroData, unitDataManager);
            var buildChoices = new Dictionary<Race, BuildChoices>
            {
                { Race.Protoss, new BuildChoices { Builds = protossBuilds, BuildSequences = protossBuildSequences } },
                { Race.Terran, new BuildChoices { Builds = terranBuilds, BuildSequences = terranBuildSequences } },
                { Race.Zerg, new BuildChoices { Builds = zergBuilds, BuildSequences = zergBuildSequences } }
            };
            var buildDecisionService = new BuildDecisionService(chatManager);
            var buildManager = new BuildManager(buildChoices, debugManager, macroBalancer, buildDecisionService, enemyPlayerService, chatHistory, enemyStrategyHistory);
            managers.Add(buildManager);

            return new SharkyBot(managers, debugManager);
        }
    }
}
