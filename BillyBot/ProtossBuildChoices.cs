﻿using BillyBot.Builds;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds;
using Sharky.Builds.Protoss;
using Sharky.DefaultBot;
using Sharky.MicroControllers;

namespace BillyBot;

public class ProtossBuildChoices
{
    public ProtossBuildChoices(DefaultSharkyBot defaultSharkyBot)
    {
        // we can use this to switch builds mid-game if we detect certain strategies (it can also be handled specificly for each build)
        var protossCounterTransitioner = new ProtossCounterTransitioner(defaultSharkyBot);

        // a probe microcontroller for our proxy builds
        var probeMicroController = new IndividualMicroController(defaultSharkyBot, defaultSharkyBot.SharkyAdvancedPathFinder, MicroPriority.JustLive, false);

        // We create all of our builds
        var proxyVoidRay = new ProxyVoidRay(defaultSharkyBot, protossCounterTransitioner, probeMicroController);
        var zealotRush = new ZealotRush(defaultSharkyBot, protossCounterTransitioner);
        var oneBaseCarriers = new OneBaseCarriers(defaultSharkyBot, protossCounterTransitioner);
        var robo = new Robo(defaultSharkyBot, protossCounterTransitioner);
        var nexusFirst = new NexusFirst(defaultSharkyBot, protossCounterTransitioner);
        var protossRobo = new ProtossRobo(defaultSharkyBot, protossCounterTransitioner);
        var dtRobo = new DtRobo(defaultSharkyBot, protossCounterTransitioner);

        // We add all the builds to a build dictionary.  Every protoss build your bot uses must be included here.
        var builds = new Dictionary<string, ISharkyBuild>
        {
            [proxyVoidRay.Name()] = proxyVoidRay,
            [zealotRush.Name()] = zealotRush,
            [oneBaseCarriers.Name()] = oneBaseCarriers,
            [robo.Name()] = robo,
            [nexusFirst.Name()] = nexusFirst,
            [protossRobo.Name()] = protossRobo,
            [dtRobo.Name()] = dtRobo
        };

        // we create build sequences to be used by each matchup
        var versusEverything = new List<List<string>>
        {
            new() {oneBaseCarriers.Name()},
            new() {nexusFirst.Name(), robo.Name(), protossRobo.Name()},
            new() {zealotRush.Name()},
            new() {proxyVoidRay.Name()}
        };
        var versusZerg = new List<List<string>>
        {
            new() {zealotRush.Name()},
            new() {nexusFirst.Name(), robo.Name(), protossRobo.Name()}
        };
        var transitions = new List<List<string>>
        {
            new() {robo.Name(), protossRobo.Name()}
        };

        versusZerg = new()
        {
            new() {dtRobo.Name()}
        };
        versusEverything = new()
        {
            new() {dtRobo.Name()}
        };

        var buildSequences = new Dictionary<string, List<List<string>>>
        {
            [Race.Terran.ToString()] = versusEverything,
            [Race.Zerg.ToString()] = versusZerg,
            [Race.Protoss.ToString()] = versusEverything,
            [Race.Random.ToString()] = versusEverything,
            ["Transition"] = transitions
        };

        BuildChoices = new() {Builds = builds, BuildSequences = buildSequences};
    }

    public BuildChoices BuildChoices { get; }
}