using System.Collections;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

public abstract class UnitTask : Task {

    protected AIController AIController { get; private set; }
    protected Unit Unit { get; private set; }
    protected UnitBlackboard UnitBlackboard { get; private set; }

    public override void Init() {
        AIController = (AIController)BehaviourTree.Executor;
        Unit = AIController.Unit;
        UnitBlackboard = (UnitBlackboard)BehaviourTree.Blackboard;
    }

    public override void Begin() {
    }
}
