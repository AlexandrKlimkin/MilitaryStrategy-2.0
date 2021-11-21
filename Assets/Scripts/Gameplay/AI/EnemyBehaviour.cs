using UnityEngine;
using NPBehave;

public class EnemyBehaviour : MonoBehaviour {
    private Blackboard _Blackboard;
    private Root _BehaviorTree;
    private Unit _Owner;
    public float VisionRadius = 15f;
    private float _SqrVisionRadius;

    private void Awake() {
        _Owner = GetComponent<Unit>();
    }

    private void Start() {
                _SqrVisionRadius = VisionRadius * VisionRadius;
        _BehaviorTree = CreateBehaviourTree();
        _Blackboard = _BehaviorTree.Blackboard;

#if UNITY_EDITOR
        Debugger _Debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        _Debugger.BehaviorTree = _BehaviorTree;
#endif
        _BehaviorTree.Start();
    }

    private Root CreateBehaviourTree() {
        return new Root(
            new Service(0.125f, UpdateDistanceToPlayer,
                new Selector(
                    new BlackboardCondition("SqrDistanceToPlayer", Operator.IS_SMALLER, _SqrVisionRadius, Stops.IMMEDIATE_RESTART,
                            // the player is in our range of VisionRadius                                                                                               
                            new Action((bool _shouldCancel) => { // go towards player until playerDistance is greater than VisionRadius ( in that case, _shouldCancel will get true )
                                if (!_shouldCancel) {
                                    _Owner.AttackController.Target = PlayerController.Instance.Unit;
                                    return Action.Result.PROGRESS;
                                }
                                else {
                                    return Action.Result.FAILED;
                                }
                            }) { Label = "Attacking" }
                    ),
                    // park until playerDistance does change
                    new Action(() => {
                        _Owner.AttackController.Target = null;
                        _Owner.MoveController.IsStopped = true;
                    } ) { Label = "Idle" }
                )
            )
        );
    }

    private void UpdateDistanceToPlayer() {
        Vector3 playerPos = PlayerController.Instance.Unit.transform.position;
        _Blackboard["playerPos"] = playerPos;
        _Blackboard["SqrDistanceToPlayer"] = Vector3.SqrMagnitude(playerPos - _Owner.transform.position);
    }
}
