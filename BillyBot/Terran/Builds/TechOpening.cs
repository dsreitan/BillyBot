using BillyBot.Common;
using SC2APIProtocol;
using Sharky;
using Sharky.Builds.Terran;
using Sharky.DefaultBot;
using Sharky.MicroTasks;

namespace BillyBot.Terran.Builds;

/// <summary>
///     One gas opening - aims to create factory and orbital right after rax, where it also ends
/// </summary>
public class TechOpening : TerranSharkyBuild
{
    public TechOpening(DefaultSharkyBot defaultSharkyBot) : base(defaultSharkyBot)
    {
    }

    public override void StartBuild(int frame)
    {
        base.StartBuild(frame);
        BuildOptions.StrictGasCount = true;

        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_BARRACKS] = 1;
        MacroData.DesiredProductionCounts[UnitTypes.TERRAN_FACTORY] = 1;
        MacroData.DesiredMorphCounts[UnitTypes.TERRAN_ORBITALCOMMAND] = 1;

        if (MicroTaskData.MicroTasks.ContainsKey("DefenseSquadTask"))
        {
            var defenseSquadTask = (DefenseSquadTask) MicroTaskData.MicroTasks["DefenseSquadTask"];
            defenseSquadTask.DesiredUnitsClaims = new() {new(UnitTypes.TERRAN_MARINE, 1)};
            defenseSquadTask.Enable();
        }
    }

    public override void OnFrame(ResponseObservation observation)
    {
        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_MARINE] = 4;

        if (UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_SUPPLYDEPOT) > 0) MacroData.DesiredGases = 1;

        StopSupplyAndWorkersWhenRaxIsNearlyCompleted();
    }

    private void StopSupplyAndWorkersWhenRaxIsNearlyCompleted()
    {
        var shouldStop = ActiveUnitData.CompletedAndNearlyCompleted(UnitTypes.TERRAN_BARRACKS, .60f) > 0;
        if (!shouldStop) return;

        BuildOptions.StrictWorkerCount = true;
        BuildOptions.StrictSupplyCount = true;
        MacroData.DesiredUnitCounts[UnitTypes.TERRAN_SCV] = 0;
        MacroData.DesiredSupplyDepots = 0;
    }

    public override bool Transition(int frame) => UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_FACTORY) > 0
                                                  && UnitCountService.BuildingsDoneAndInProgressCount(UnitTypes.TERRAN_ORBITALCOMMAND) > 0;
}