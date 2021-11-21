using Tools.BehaviourTree;

public class BotAIController : AIController {

    protected override BehaviourTree BuildBehaviourTree() {
        return UnitBTBuilder.Instance.Build();
    }

    protected override Blackboard BuildBlackboard() {
        return new UnitBlackboard();
    }

    public override void UpdateAI() {
        if (Unit.Dead) {
            this.enabled = false;
            return;
        }
        UpdateBT();
    }
}
