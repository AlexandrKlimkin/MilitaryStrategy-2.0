using Tools.BehaviourTree;
using UnityEngine;

public abstract partial class AIController : BehaviourTreeExecutor {

    public Unit Unit { get; protected set; }
    public UnitBlackboard UnitBlackboard { get; protected set; }

    protected override void Awake() {
        base.Awake();
        UnitBlackboard = new UnitBlackboard();
        Unit = GetComponent<Unit>();
        Unit.OnDeath += OnBecameDead;
        UnitBlackboard = BehaviourTree.Blackboard as UnitBlackboard;
    }

    private void Start() {
        AIScheduler.Instance.Register(this);
    }

    private void OnDestroy() {
        if (Unit != null)
            Unit.OnDeath -= OnBecameDead;
        if (AIScheduler.Instance != null)
            AIScheduler.Instance.Unregister(this);
    }

    private void OnBecameDead() {
        if (AIScheduler.Instance != null)
            AIScheduler.Instance.Unregister(this);
    }

    public void SetBehaviourTreeOverride(BehaviourTree behaviourTree) {
        BehaviourTree = behaviourTree;
        UnitBlackboard = BehaviourTree.Blackboard as UnitBlackboard;
        BehaviourTree.Executor = this;
        BehaviourTree.Init();
    }

    public abstract void UpdateAI();

//#if UNITY_EDITOR
//    private void OnDrawGizmos() {
//        var shipBlackboard = this.BehaviourTree.Blackboard as ShipBlackboard;
//        if (shipBlackboard.MoveDestination == null)
//            return;
//        UnityEditor.Handles.DrawWireCube(shipBlackboard.MoveDestination.Value, Vector3.one);
//    }
//#endif
}
