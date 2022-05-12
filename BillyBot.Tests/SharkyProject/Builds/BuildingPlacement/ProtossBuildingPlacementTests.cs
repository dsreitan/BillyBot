using System.Linq;
using AutoFixture;
using BillyBot.Common;
using NUnit.Framework;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds.BuildingPlacement;


namespace BillyBot.Tests.SharkyCore.Builds.BuildingPlacement;


public class ProtossBuildingPlacementTest
{
    private ActiveUnitData? _activeUnitData;
    private SharkyOptions? _sharkyOptions;
    private SharkyUnitData? _sharkyUnitData;
    private UnitDataService? _unitDataService;
    private ProtossBuildingPlacementCopy? protossBuildingPlacement;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _sharkyUnitData = fixture.Create<SharkyUnitData>();
        _sharkyOptions = fixture.Create<SharkyOptions>();
        _activeUnitData = fixture.Create<ActiveUnitData>();
        _unitDataService = new(_sharkyUnitData, _sharkyOptions, new());

        // public ProtossBuildingPlacementCopy(ActiveUnitData activeUnitData,
        // SharkyUnitData sharkyUnitData, BaseData baseData,
        // MapDataService mapDataService,
        // BuildingService buildingService, IBuildingPlacement wallOffPlacement,
        // ProtossPylonGridPlacement protossPylonGridPlacement,
        // ProtossProductionGridPlacement protossProductionGridPlacement,
        // IBuildingPlacement protectNexusPylonPlacement, TargetingData targetingData,
        // IBuildingPlacement protectNexusCannonPlacement, BuildOptions buildOptions,
        // IBuildingPlacement protossDefensiveGridPlacement, IBuildingPlacement protossProxyGridPlacement)



    }

    [Theory]
    [TestCase(.10f, ExpectedResult = true)]
    public bool IsUnitNearlyCompleted(float percentCompleteThreshold)
    {
        // setup
        var unitType = UnitTypes.PROTOSS_ZEALOT;
        var unit = new Unit { UnitType = (uint)unitType, BuildProgress = .25f, Pos = new() { X = 1, Y = 2 } };
        _sharkyUnitData.UnitData.Add(unitType, new());
        _activeUnitData.SelfUnits.TryAdd(1, new(unit, 0, _sharkyUnitData, _sharkyOptions, _unitDataService, 1));



        // assert
        return _activeUnitData.CompletedAndNearlyCompleted(UnitTypes.PROTOSS_ZEALOT, percentCompleteThreshold) > 0;
    }


}

