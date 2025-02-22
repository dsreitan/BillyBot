﻿namespace Sharky
{
    /// <summary>
    /// Taken from https://github.com/Blizzard/s2client-api/blob/master/include/sc2api/sc2_typeenums.h
    /// </summary>
    public enum UnitTypes
    {
        INVALID = 0,

        // Terran
        TERRAN_ARMORY = 29,    // CANCEL, HALT, CANCEL_LAST, RESEARCH_TERRANSHIPWEAPONS, RESEARCH_TERRANVEHICLEANDSHIPPLATING, RESEARCH_TERRANVEHICLEWEAPONS
        TERRAN_AUTOTURRET = 31,    // SMART, STOP, ATTACK
        TERRAN_BANSHEE = 55,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK, BEHAVIOR_CLOAKON, BEHAVIOR_CLOAKOFF
        TERRAN_BARRACKS = 21,    // SMART, TRAIN_MARINE, TRAIN_REAPER, TRAIN_GHOST, TRAIN_MARAUDER, CANCEL, HALT, CANCEL_LAST, RALLY_UNITS, LIFT, BUILD_TECHLAB, BUILD_REACTOR
        TERRAN_BARRACKSFLYING = 46,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, LAND, BUILD_TECHLAB, BUILD_REACTOR
        TERRAN_BARRACKSREACTOR = 38,    // CANCEL
        TERRAN_BARRACKSTECHLAB = 37,    // RESEARCH_STIMPACK, RESEARCH_COMBATSHIELD, RESEARCH_CONCUSSIVESHELLS, CANCEL, CANCEL_LAST
        TERRAN_BATTLECRUISER = 57,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_YAMATOGUN, EFFECT_TACTICALJUMP, STOP, ATTACK
        TERRAN_BUNKER = 24,    // SMART, EFFECT_SALVAGE, CANCEL, HALT, UNLOADALL, STOP, LOAD, RALLY_UNITS, ATTACK, EFFECT_STIM
        TERRAN_COMMANDCENTER = 18,    // SMART, TRAIN_SCV, MORPH_PLANETARYFORTRESS, MORPH_ORBITALCOMMAND, CANCEL, HALT, LOADALL, UNLOADALL, CANCEL_LAST, LIFT, RALLY_WORKERS
        TERRAN_COMMANDCENTERFLYING = 36,    // SMART, MOVE, PATROL, HOLDPOSITION, LOADALL, UNLOADALL, STOP, LAND
        TERRAN_CYCLONE = 692,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_LOCKON, CANCEL, STOP, ATTACK
        TERRAN_ENGINEERINGBAY = 22,    // RESEARCH_HISECAUTOTRACKING, RESEARCH_TERRANSTRUCTUREARMORUPGRADE, RESEARCH_NEOSTEELFRAME, CANCEL, HALT, CANCEL_LAST, RESEARCH_TERRANINFANTRYARMOR, RESEARCH_TERRANINFANTRYWEAPONS
        TERRAN_FACTORY = 27,    // SMART, TRAIN_SIEGETANK, TRAIN_THOR, TRAIN_HELLION, TRAIN_HELLBAT, TRAIN_CYCLONE, TRAIN_WIDOWMINE, CANCEL, HALT, CANCEL_LAST, RALLY_UNITS, LIFT, BUILD_TECHLAB, BUILD_REACTOR
        TERRAN_FACTORYFLYING = 43,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, LAND, BUILD_TECHLAB, BUILD_REACTOR
        TERRAN_FACTORYREACTOR = 40,    // CANCEL
        TERRAN_FACTORYTECHLAB = 39,    // RESEARCH_INFERNALPREIGNITER, RESEARCH_DRILLINGCLAWS, RESEARCH_RAPIDFIRELAUNCHERS, RESEARCH_SMARTSERVOS, CANCEL, CANCEL_LAST
        TERRAN_FUSIONCORE = 30,    // RESEARCH_BATTLECRUISERWEAPONREFIT, CANCEL, HALT, CANCEL_LAST
        TERRAN_GHOST = 50,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_NUKECALLDOWN, EFFECT_EMP, EFFECT_GHOSTSNIPE, CANCEL, STOP, ATTACK, BEHAVIOR_CLOAKON, BEHAVIOR_CLOAKOFF, BEHAVIOR_HOLDFIREON, BEHAVIOR_HOLDFIREOFF
        TERRAN_GHOSTACADEMY = 26,    // BUILD_NUKE, RESEARCH_PERSONALCLOAKING, CANCEL, HALT, CANCEL_LAST
        TERRAN_HELLION = 53,    // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_HELLBAT, STOP, ATTACK
        TERRAN_HELLIONTANK = 484,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_HELLION, STOP, ATTACK
        TERRAN_LIBERATOR = 689,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_LIBERATORAGMODE, STOP, ATTACK
        TERRAN_LIBERATORAG = 734,   // SMART, MORPH_LIBERATORAAMODE, STOP, ATTACK
        TERRAN_MARAUDER = 51,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK, EFFECT_STIM
        TERRAN_MARINE = 48,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK, EFFECT_STIM
        TERRAN_MEDIVAC = 54,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_HEAL, EFFECT_MEDIVACIGNITEAFTERBURNERS, STOP, LOAD, UNLOADALLAT, ATTACK
        TERRAN_MISSILETURRET = 23,    // SMART, CANCEL, HALT, STOP, ATTACK
        TERRAN_MULE = 268,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, HARVEST_GATHER, HARVEST_RETURN, ATTACK, EFFECT_REPAIR
        TERRAN_ORBITALCOMMAND = 132,   // SMART, EFFECT_CALLDOWNMULE, EFFECT_SUPPLYDROP, EFFECT_SCAN, TRAIN_SCV, CANCEL_LAST, LIFT, RALLY_WORKERS
        TERRAN_ORBITALCOMMANDFLYING = 134,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, LAND
        TERRAN_PLANETARYFORTRESS = 130,   // SMART, TRAIN_SCV, LOADALL, STOP, CANCEL_LAST, ATTACK, RALLY_WORKERS
        TERRAN_RAVEN = 56,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_POINTDEFENSEDRONE, EFFECT_HUNTERSEEKERMISSILE, EFFECT_AUTOTURRET, STOP, ATTACK
        TERRAN_REAPER = 49,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_KD8CHARGE, STOP, ATTACK
        TERRAN_REFINERY = 20,    // CANCEL, HALT
        TERRAN_SCV = 45,    // SMART, MOVE, PATROL, HOLDPOSITION, BUILD_COMMANDCENTER, BUILD_SUPPLYDEPOT, BUILD_REFINERY, BUILD_BARRACKS, BUILD_ENGINEERINGBAY, BUILD_MISSILETURRET, BUILD_BUNKER, BUILD_SENSORTOWER, BUILD_GHOSTACADEMY, BUILD_FACTORY, BUILD_STARPORT, BUILD_ARMORY, BUILD_FUSIONCORE, HALT, STOP, HARVEST_GATHER, HARVEST_RETURN, ATTACK, EFFECT_SPRAY, EFFECT_REPAIR
        TERRAN_SENSORTOWER = 25,    // CANCEL, HALT
        TERRAN_SIEGETANK = 33,    // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_SIEGEMODE, STOP, ATTACK
        TERRAN_SIEGETANKSIEGED = 32,    // SMART, MORPH_UNSIEGE, STOP, ATTACK
        TERRAN_STARPORT = 28,    // SMART, TRAIN_MEDIVAC, TRAIN_BANSHEE, TRAIN_RAVEN, TRAIN_BATTLECRUISER, TRAIN_VIKINGFIGHTER, TRAIN_LIBERATOR, CANCEL, HALT, CANCEL_LAST, RALLY_UNITS, LIFT, BUILD_TECHLAB, BUILD_REACTOR
        TERRAN_STARPORTFLYING = 44,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, LAND, BUILD_TECHLAB, BUILD_REACTOR
        TERRAN_STARPORTREACTOR = 42,    // CANCEL
        TERRAN_STARPORTTECHLAB = 41,    // RESEARCH_BANSHEECLOAKINGFIELD, RESEARCH_RAVENCORVIDREACTOR, RESEARCH_ENHANCEDMUNITIONS, RESEARCH_BANSHEEHYPERFLIGHTROTORS, RESEARCH_RAVENRECALIBRATEDEXPLOSIVES, RESEARCH_HIGHCAPACITYFUELTANKS, RESEARCH_ADVANCEDBALLISTICS, CANCEL, CANCEL_LAST
        TERRAN_SUPPLYDEPOT = 19,    // MORPH_SUPPLYDEPOT_LOWER, CANCEL, HALT
        TERRAN_SUPPLYDEPOTLOWERED = 47,    // MORPH_SUPPLYDEPOT_RAISE
        TERRAN_THOR = 52,    // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_THORHIGHIMPACTMODE, STOP, ATTACK
        TERRAN_THORAP = 691,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_THOREXPLOSIVEMODE, CANCEL, STOP, ATTACK
        TERRAN_VIKINGASSAULT = 34,    // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_VIKINGFIGHTERMODE, STOP, ATTACK
        TERRAN_VIKINGFIGHTER = 35,    // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_VIKINGASSAULTMODE, STOP, ATTACK
        TERRAN_WIDOWMINE = 498,   // SMART, MOVE, PATROL, HOLDPOSITION, BURROWDOWN, STOP, ATTACK
        TERRAN_WIDOWMINEBURROWED = 500,   // SMART, EFFECT_WIDOWMINEATTACK, BURROWUP
        TERRAN_REFINERYRICH = 1943,

        // Terran non-interactive
        TERRAN_KD8CHARGE = 830,
        TERRAN_NUKE = 58,
        TERRAN_POINTDEFENSEDRONE = 11,
        TERRAN_REACTOR = 6,
        TERRAN_TECHLAB = 5,

        // Zerg
        ZERG_BANELING = 9,     // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_EXPLODE, BEHAVIOR_BUILDINGATTACKON, BEHAVIOR_BUILDINGATTACKOFF, BURROWDOWN, STOP, ATTACK
        ZERG_BANELINGBURROWED = 115,   // EFFECT_EXPLODE, BURROWUP
        ZERG_BANELINGCOCOON = 8,     // SMART, CANCEL_LAST, RALLY_UNITS
        ZERG_BANELINGNEST = 96,    // RESEARCH_CENTRIFUGALHOOKS, CANCEL, CANCEL_LAST
        ZERG_BROODLING = 289,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_BROODLORD = 114,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_BROODLORDCOCOON = 113,   // SMART, MOVE, PATROL, HOLDPOSITION, CANCEL
        ZERG_CHANGELING = 12,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_CHANGELINGMARINE = 15,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_CHANGELINGMARINESHIELD = 14,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_CHANGELINGZEALOT = 13,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_CHANGELINGZERGLING = 17,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_CHANGELINGZERGLINGWINGS = 16,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_CORRUPTOR = 112,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_BROODLORD, EFFECT_CAUSTICSPRAY, STOP, ATTACK
        ZERG_CREEPTUMOR = 87,    // CANCEL
        ZERG_CREEPTUMORBURROWED = 137,   // SMART, CANCEL, BUILD_CREEPTUMOR
        ZERG_CREEPTUMORQUEEN = 138,   // CANCEL
        ZERG_DRONE = 104,   // SMART, MOVE, PATROL, HOLDPOSITION, BUILD_HATCHERY, BUILD_EXTRACTOR, BUILD_SPAWNINGPOOL, BUILD_EVOLUTIONCHAMBER, BUILD_HYDRALISKDEN, BUILD_SPIRE, BUILD_ULTRALISKCAVERN, BUILD_INFESTATIONPIT, BUILD_NYDUSNETWORK, BUILD_BANELINGNEST, BUILD_ROACHWARREN, BUILD_SPINECRAWLER, BUILD_SPORECRAWLER, BURROWDOWN, STOP, HARVEST_GATHER, HARVEST_RETURN, ATTACK, EFFECT_SPRAY
        ZERG_DRONEBURROWED = 116,   // BURROWUP
        ZERG_EGG = 103,   // SMART, CANCEL_LAST, RALLY_UNITS
        ZERG_EVOLUTIONCHAMBER = 90,    // CANCEL, CANCEL_LAST, RESEARCH_ZERGGROUNDARMOR, RESEARCH_ZERGMELEEWEAPONS, RESEARCH_ZERGMISSILEWEAPONS
        ZERG_EXTRACTOR = 88,    // CANCEL
        ZERG_GREATERSPIRE = 102,   // CANCEL_LAST, RESEARCH_ZERGFLYERARMOR, RESEARCH_ZERGFLYERATTACK
        ZERG_HATCHERY = 86,    // SMART, MORPH_LAIR, RESEARCH_PNEUMATIZEDCARAPACE, RESEARCH_BURROW, TRAIN_QUEEN, CANCEL, CANCEL_LAST, RALLY_UNITS, RALLY_WORKERS
        ZERG_HIVE = 101,   // SMART, RESEARCH_PNEUMATIZEDCARAPACE, RESEARCH_BURROW, TRAIN_QUEEN, CANCEL_LAST, RALLY_UNITS, RALLY_WORKERS
        ZERG_HYDRALISK = 107,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_LURKER, BURROWDOWN, STOP, ATTACK
        ZERG_HYDRALISKBURROWED = 117,   // BURROWUP
        ZERG_HYDRALISKDEN = 91,    // RESEARCH_GROOVEDSPINES, RESEARCH_MUSCULARAUGMENTS, MORPH_LURKERDEN, CANCEL, CANCEL_LAST
        ZERG_INFESTATIONPIT = 94,    // RESEARCH_PATHOGENGLANDS, RESEARCH_NEURALPARASITE, CANCEL, CANCEL_LAST
        ZERG_INFESTEDTERRANSEGG = 150,   // SMART, MOVE, PATROL, HOLDPOSITION
        ZERG_INFESTOR = 111,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_FUNGALGROWTH, EFFECT_INFESTEDTERRANS, EFFECT_NEURALPARASITE, CANCEL, BURROWDOWN, STOP, ATTACK
        ZERG_INFESTORBURROWED = 127,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_FUNGALGROWTH, EFFECT_INFESTEDTERRANS, EFFECT_NEURALPARASITE, CANCEL, BURROWUP, STOP, ATTACK
        ZERG_INFESTORTERRAN = 7,     // SMART, MOVE, PATROL, HOLDPOSITION, BURROWDOWN, STOP, ATTACK
        ZERG_LAIR = 100,   // SMART, MORPH_HIVE, RESEARCH_PNEUMATIZEDCARAPACE, RESEARCH_BURROW, TRAIN_QUEEN, CANCEL, CANCEL_LAST, RALLY_UNITS, RALLY_WORKERS
        ZERG_LARVA = 151,   // TRAIN_DRONE, TRAIN_ZERGLING, TRAIN_OVERLORD, TRAIN_HYDRALISK, TRAIN_MUTALISK, TRAIN_ULTRALISK, TRAIN_ROACH, TRAIN_INFESTOR, TRAIN_CORRUPTOR, TRAIN_VIPER, TRAIN_SWARMHOST
        ZERG_LOCUSTMP = 489,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_LOCUSTMPFLYING = 693,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_LOCUSTSWOOP, STOP, ATTACK
        ZERG_LURKERDENMP = 504,   // RESEARCH_GROOVEDSPINES, RESEARCH_MUSCULARAUGMENTS, CANCEL_LAST
        ZERG_LURKERMP = 502,   // SMART, MOVE, PATROL, HOLDPOSITION, BURROWDOWN, STOP, ATTACK
        ZERG_LURKERMPBURROWED = 503,   // SMART, BURROWUP, STOP, ATTACK, BEHAVIOR_HOLDFIREON, BEHAVIOR_HOLDFIREOFF
        ZERG_LURKERMPEGG = 501,   // SMART, CANCEL, RALLY_UNITS
        ZERG_MUTALISK = 108,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        ZERG_NYDUSCANAL = 142,   // SMART, UNLOADALL, STOP, LOAD, RALLY_UNITS
        ZERG_NYDUSNETWORK = 95,    // SMART, BUILD_NYDUSWORM, CANCEL, UNLOADALL, STOP, LOAD, RALLY_UNITS
        ZERG_OVERLORD = 106,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_OVERSEER, BEHAVIOR_GENERATECREEPON, BEHAVIOR_GENERATECREEPOFF, MORPH_OVERLORDTRANSPORT, CANCEL, STOP, ATTACK
        ZERG_OVERLORDCOCOON = 128,   // SMART, MOVE, PATROL, HOLDPOSITION, CANCEL
        ZERG_OVERLORDTRANSPORT = 893,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_OVERSEER, BEHAVIOR_GENERATECREEPON, BEHAVIOR_GENERATECREEPOFF, STOP, LOAD, UNLOADALLAT, ATTACK
        ZERG_OVERSEER = 129,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_SPAWNCHANGELING, EFFECT_CONTAMINATE, STOP, ATTACK
        ZERG_QUEEN = 126,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_INJECTLARVA, EFFECT_TRANSFUSION, BURROWDOWN, STOP, ATTACK, BUILD_CREEPTUMOR
        ZERG_QUEENBURROWED = 125,   // BURROWUP
        ZERG_RAVAGER = 688,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_CORROSIVEBILE, BURROWDOWN, STOP, ATTACK
        ZERG_RAVAGERCOCOON = 687,   // SMART, CANCEL, RALLY_UNITS
        ZERG_ROACH = 110,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_RAVAGER, BURROWDOWN, STOP, ATTACK
        ZERG_ROACHBURROWED = 118,   // SMART, MOVE, PATROL, HOLDPOSITION, BURROWUP, STOP, ATTACK
        ZERG_ROACHWARREN = 97,    // RESEARCH_GLIALREGENERATION, RESEARCH_TUNNELINGCLAWS, CANCEL, CANCEL_LAST
        ZERG_SPAWNINGPOOL = 89,    // RESEARCH_ZERGLINGADRENALGLANDS, RESEARCH_ZERGLINGMETABOLICBOOST, CANCEL, CANCEL_LAST
        ZERG_SPINECRAWLER = 98,    // SMART, CANCEL, STOP, ATTACK, MORPH_UPROOT
        ZERG_SPINECRAWLERUPROOTED = 139,   // SMART, MOVE, PATROL, HOLDPOSITION, CANCEL, STOP, ATTACK, MORPH_ROOT
        ZERG_SPIRE = 92,    // MORPH_GREATERSPIRE, CANCEL, CANCEL_LAST, RESEARCH_ZERGFLYERARMOR, RESEARCH_ZERGFLYERATTACK
        ZERG_SPORECRAWLER = 99,    // SMART, CANCEL, STOP, ATTACK, MORPH_UPROOT
        ZERG_SPORECRAWLERUPROOTED = 140,   // SMART, MOVE, PATROL, HOLDPOSITION, CANCEL, STOP, ATTACK, MORPH_ROOT
        ZERG_SWARMHOSTBURROWEDMP = 493,   // SMART, EFFECT_SPAWNLOCUSTS, BURROWUP
        ZERG_SWARMHOSTMP = 494,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_SPAWNLOCUSTS, BURROWDOWN, STOP, ATTACK
        ZERG_TRANSPORTOVERLORDCOCOON = 892,   // SMART, MOVE, PATROL, HOLDPOSITION, CANCEL
        ZERG_ULTRALISK = 109,   // SMART, MOVE, PATROL, HOLDPOSITION, BURROWDOWN, STOP, ATTACK
        ZERG_ULTRALISKCAVERN = 93,    // RESEARCH_CHITINOUSPLATING, CANCEL, CANCEL_LAST
        ZERG_VIPER = 499,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_BLINDINGCLOUD, EFFECT_ABDUCT, EFFECT_VIPERCONSUME, EFFECT_PARASITICBOMB, STOP, ATTACK
        ZERG_ZERGLING = 105,   // SMART, MOVE, PATROL, HOLDPOSITION, TRAIN_BANELING, BURROWDOWN, STOP, ATTACK
        ZERG_ZERGLINGBURROWED = 119,   // BURROWUP
        ZERG_EXTRACTORRICH = 1995,

        // Zerg non-interactive
        ZERG_PARASITICBOMBDUMMY = 824,

        // Protoss
        PROTOSS_ADEPT = 311,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_ADEPTPHASESHIFT, CANCEL, STOP, RALLY_UNITS, ATTACK
        PROTOSS_ADEPTPHASESHIFT = 801,   // SMART, MOVE, PATROL, HOLDPOSITION, CANCEL, STOP, ATTACK
        PROTOSS_ARCHON = 141,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, RALLY_UNITS, ATTACK
        PROTOSS_ASSIMILATOR = 61,    // CANCEL
        PROTOSS_CARRIER = 79,    // SMART, MOVE, PATROL, HOLDPOSITION, BUILD_INTERCEPTORS, STOP, CANCEL_LAST, ATTACK
        PROTOSS_COLOSSUS = 4,     // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        PROTOSS_CYBERNETICSCORE = 72,    // RESEARCH_WARPGATE, CANCEL, CANCEL_LAST, RESEARCH_PROTOSSAIRARMOR, RESEARCH_PROTOSSAIRWEAPONS
        PROTOSS_DARKSHRINE = 69,    // RESEARCH_SHADOWSTRIKE, CANCEL, CANCEL_LAST
        PROTOSS_DARKTEMPLAR = 76,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, RALLY_UNITS, ATTACK, EFFECT_BLINK
        PROTOSS_DISRUPTOR = 694,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_PURIFICATIONNOVA, STOP, ATTACK
        PROTOSS_DISRUPTORPHASED = 733,   // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        PROTOSS_FLEETBEACON = 64,    // RESEARCH_INTERCEPTORGRAVITONCATAPULT, RESEARCH_PHOENIXANIONPULSECRYSTALS, CANCEL, CANCEL_LAST
        PROTOSS_FORGE = 63,    // CANCEL, CANCEL_LAST, RESEARCH_PROTOSSGROUNDARMOR, RESEARCH_PROTOSSGROUNDWEAPONS, RESEARCH_PROTOSSSHIELDS
        PROTOSS_GATEWAY = 62,    // SMART, TRAIN_ZEALOT, TRAIN_STALKER, TRAIN_HIGHTEMPLAR, TRAIN_DARKTEMPLAR, TRAIN_SENTRY, TRAIN_ADEPT, MORPH_WARPGATE, CANCEL, CANCEL_LAST, RALLY_UNITS
        PROTOSS_HIGHTEMPLAR = 75,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_FEEDBACK, EFFECT_PSISTORM, STOP, RALLY_UNITS, ATTACK
        PROTOSS_IMMORTAL = 83,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_IMMORTALBARRIER, STOP, ATTACK
        PROTOSS_INTERCEPTOR = 85,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        PROTOSS_MOTHERSHIP = 10,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_PHOTONOVERCHARGE, EFFECT_TIMEWARP, STOP, ATTACK, EFFECT_MASSRECALL
        PROTOSS_MOTHERSHIPCORE = 488,   // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_MOTHERSHIP, EFFECT_PHOTONOVERCHARGE, EFFECT_TIMEWARP, CANCEL, STOP, ATTACK, EFFECT_MASSRECALL
        PROTOSS_NEXUS = 59,    // SMART, EFFECT_CHRONOBOOST, TRAIN_PROBE, TRAIN_MOTHERSHIP, CANCEL, CANCEL_LAST, RALLY_WORKERS
        PROTOSS_OBSERVER = 82,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, ATTACK
        PROTOSS_ORACLE = 495,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_ORACLEREVELATION, BEHAVIOR_PULSARBEAMON, BEHAVIOR_PULSARBEAMOFF, BUILD_STASISTRAP, CANCEL, STOP, ATTACK
        PROTOSS_ORACLESTASISTRAP = 732,   // CANCEL
        PROTOSS_PHOENIX = 78,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_GRAVITONBEAM, CANCEL, STOP, ATTACK
        PROTOSS_PHOTONCANNON = 66,    // SMART, CANCEL, STOP, ATTACK
        PROTOSS_PROBE = 84,    // SMART, MOVE, PATROL, HOLDPOSITION, BUILD_NEXUS, BUILD_PYLON, BUILD_ASSIMILATOR, BUILD_GATEWAY, BUILD_FORGE, BUILD_FLEETBEACON, BUILD_TWILIGHTCOUNCIL, BUILD_PHOTONCANNON, BUILD_SHIELDBATTERY, BUILD_STARGATE, BUILD_TEMPLARARCHIVE, BUILD_DARKSHRINE, BUILD_ROBOTICSBAY, BUILD_ROBOTICSFACILITY, BUILD_CYBERNETICSCORE, STOP, HARVEST_GATHER, HARVEST_RETURN, ATTACK, EFFECT_SPRAY
        PROTOSS_PYLON = 60,    // CANCEL
        PROTOSS_PYLONOVERCHARGED = 894,   // SMART, STOP, ATTACK
        PROTOSS_ROBOTICSBAY = 70,    // RESEARCH_GRAVITICBOOSTER, RESEARCH_GRAVITICDRIVE, RESEARCH_EXTENDEDTHERMALLANCE, CANCEL, CANCEL_LAST
        PROTOSS_ROBOTICSFACILITY = 71,    // SMART, TRAIN_WARPPRISM, TRAIN_OBSERVER, TRAIN_COLOSSUS, TRAIN_IMMORTAL, TRAIN_DISRUPTOR, CANCEL, CANCEL_LAST, RALLY_UNITS
        PROTOSS_SENTRY = 77,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_GUARDIANSHIELD, HALLUCINATION_ARCHON, HALLUCINATION_COLOSSUS, HALLUCINATION_HIGHTEMPLAR, HALLUCINATION_IMMORTAL, HALLUCINATION_PHOENIX, HALLUCINATION_PROBE, HALLUCINATION_STALKER, HALLUCINATION_VOIDRAY, HALLUCINATION_WARPPRISM, HALLUCINATION_ZEALOT, EFFECT_FORCEFIELD, HALLUCINATION_ORACLE, HALLUCINATION_DISRUPTOR, HALLUCINATION_ADEPT, STOP, RALLY_UNITS, ATTACK
        PROTOSS_SHIELDBATTERY = 1910,   // SMART, EFFECT_RESTORE
        PROTOSS_STALKER = 74,    // SMART, MOVE, PATROL, HOLDPOSITION, STOP, RALLY_UNITS, ATTACK, EFFECT_BLINK
        PROTOSS_STARGATE = 67,    // SMART, TRAIN_PHOENIX, TRAIN_CARRIER, TRAIN_VOIDRAY, TRAIN_ORACLE, TRAIN_TEMPEST, CANCEL, CANCEL_LAST, RALLY_UNITS
        PROTOSS_TEMPEST = 496,   // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_TEMPESTDISRUPTIONBLAST, CANCEL, STOP, ATTACK
        PROTOSS_TEMPLARARCHIVE = 68,    // RESEARCH_PSISTORM, CANCEL, CANCEL_LAST
        PROTOSS_TWILIGHTCOUNCIL = 65,    // RESEARCH_CHARGE, RESEARCH_BLINK, RESEARCH_ADEPTRESONATINGGLAIVES, CANCEL, CANCEL_LAST
        PROTOSS_VOIDRAY = 80,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_VOIDRAYPRISMATICALIGNMENT, STOP, ATTACK
        PROTOSS_WARPGATE = 133,   // SMART, TRAINWARP_ZEALOT, TRAINWARP_STALKER, TRAINWARP_HIGHTEMPLAR, TRAINWARP_DARKTEMPLAR, TRAINWARP_SENTRY, TRAINWARP_ADEPT, MORPH_GATEWAY
        PROTOSS_WARPPRISM = 81,    // SMART, MOVE, PATROL, HOLDPOSITION, MORPH_WARPPRISMPHASINGMODE, STOP, LOAD, UNLOADALLAT, ATTACK
        PROTOSS_WARPPRISMPHASING = 136,   // SMART, MORPH_WARPPRISMTRANSPORTMODE, STOP, LOAD, UNLOADALLAT
        PROTOSS_ZEALOT = 73,    // SMART, MOVE, PATROL, HOLDPOSITION, EFFECT_CHARGE, STOP, RALLY_UNITS, ATTACK
        PROTOSS_ASSIMILATORRICH = 1994,

        // Protoss non-interactive

        // Neutral
        NEUTRAL_BATTLESTATIONMINERALFIELD = 886,
        NEUTRAL_BATTLESTATIONMINERALFIELD750 = 887,
        NEUTRAL_COLLAPSIBLEROCKTOWERDEBRIS = 490,
        NEUTRAL_COLLAPSIBLEROCKTOWERDIAGONAL = 588,
        NEUTRAL_COLLAPSIBLEROCKTOWERPUSHUNIT = 561,
        NEUTRAL_COLLAPSIBLETERRANTOWERDEBRIS = 485,
        NEUTRAL_COLLAPSIBLETERRANTOWERDIAGONAL = 589,
        NEUTRAL_COLLAPSIBLETERRANTOWERPUSHUNIT = 562,
        NEUTRAL_COLLAPSIBLETERRANTOWERPUSHUNITRAMPLEFT = 559,
        NEUTRAL_COLLAPSIBLETERRANTOWERPUSHUNITRAMPRIGHT = 560,
        NEUTRAL_COLLAPSIBLETERRANTOWERRAMPLEFT = 590,
        NEUTRAL_COLLAPSIBLETERRANTOWERRAMPRIGHT = 591,
        NEUTRAL_DEBRISRAMPLEFT = 486,
        NEUTRAL_DEBRISRAMPRIGHT = 487,
        NEUTRAL_DESTRUCTIBLEDEBRIS6X6 = 365,
        NEUTRAL_DESTRUCTIBLEDEBRISRAMPDIAGONALHUGEBLUR = 377,
        NEUTRAL_DESTRUCTIBLEDEBRISRAMPDIAGONALHUGEULBR = 376,
        NEUTRAL_DESTRUCTIBLEROCK6X6 = 371,
        NEUTRAL_DESTRUCTIBLEROCKEX1DIAGONALHUGEBLUR = 641,
        NEUTRAL_FORCEFIELD = 135,
        NEUTRAL_KARAKFEMALE = 324,
        NEUTRAL_LABMINERALFIELD = 665,
        NEUTRAL_LABMINERALFIELD750 = 666,
        NEUTRAL_MINERALFIELD = 341,
        NEUTRAL_MINERALFIELD750 = 483,
        NEUTRAL_PROTOSSVESPENEGEYSER = 608,
        NEUTRAL_PURIFIERMINERALFIELD = 884,
        NEUTRAL_PURIFIERMINERALFIELD750 = 885,
        NEUTRAL_PURIFIERRICHMINERALFIELD = 796,
        NEUTRAL_PURIFIERRICHMINERALFIELD750 = 797,
        NEUTRAL_PURIFIERVESPENEGEYSER = 880,
        NEUTRAL_RICHMINERALFIELD = 146,
        NEUTRAL_RICHMINERALFIELD750 = 147,
        NEUTRAL_RICHVESPENEGEYSER = 344,
        NEUTRAL_SCANTIPEDE = 335,
        NEUTRAL_SHAKURASVESPENEGEYSER = 881,
        NEUTRAL_SPACEPLATFORMGEYSER = 343,
        NEUTRAL_UNBUILDABLEBRICKSDESTRUCTIBLE = 473,
        NEUTRAL_UNBUILDABLEPLATESDESTRUCTIBLE = 474,
        NEUTRAL_UTILITYBOT = 330,
        NEUTRAL_VESPENEGEYSER = 342,
        NEUTRAL_XELNAGATOWER = 149,
        NEUTRAL_CLEANINGBOT = 612
    }
}
