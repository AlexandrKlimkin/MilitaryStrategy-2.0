using Tools.BehaviourTree;

public class UnitBTBuilder : BehaviourTreeBuilder<UnitBTBuilder> {

    public override BehaviourTree Build() {
        var bt = new BehaviourTree();
            var combat = bt.AddChild<ExecuteAllTask>();
                var tryFindTarget = combat.AddChild<SequenceTask>();
                    tryFindTarget.AddChild<WaitForTimeTask>().WaitTime = 3f;
                    tryFindTarget.AddChild<FindTargetTask>();
                var pursue = combat.AddChild<SequenceTask>();
                    pursue.AddChild<PursueTargetTask>();
                    pursue.AddChild<AttackTargetTask>();
                return bt;
    }
}
