using Tools.BehaviourTree;

public class UnitBTBuilder : BehaviourTreeBuilder<UnitBTBuilder> {

    public override BehaviourTree Build() {
        var bt = new BehaviourTree();
            var combat = bt.AddChild<SequenceTask_Correct>();
                combat.AddChild<FindTargetTask>();
                combat.AddChild<PursueTargetTask>();
                combat.AddChild<AttackTargetTask>();
        //var root = bt.AddChild<ExecuteAllTask>();
        //    var combat = root.AddChild<SequenceTask>();
        //        combat.AddChild<FindTargetTask>();
        //        combat.AddChild<AttackTargetTask>();
        //    var movement = root.AddChild<SequenceTask>();
        //        var destination = movement.AddChild<SelectorTask>();
        //            var combatDestination = destination.AddChild<CombatDestinationTask>();
        //            var controlPointDestination = destination.AddChild<SelectorTask>();
        //                controlPointDestination.AddChild<CaptureCPTask>();
        //                controlPointDestination.AddChild<SelectCaptureCPTask>();
        //            var randomDestination = destination.AddChild<SelectorTask>();
        //                randomDestination.AddChild<CheckDestinationTask>();
        //                randomDestination.AddChild<SetRandomDestinationTask>();
        //        movement.AddChild<MoveToTask>();
        return bt;
    }
}
