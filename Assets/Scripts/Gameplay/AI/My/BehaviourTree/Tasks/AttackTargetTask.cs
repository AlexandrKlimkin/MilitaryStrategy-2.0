using System.Collections;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

public class AttackTargetTask : UnitTask {

    public override TaskStatus Run() {
        if ((Unit.AttackController.Target == null || Unit.AttackController.Target.Dead)) {
            return TaskStatus.Success;
        }
        var sqrDistToTarget = Unit.AttackController.SqrDistanceToTarget;
        if (sqrDistToTarget > Unit.AttackController.Weapon.SqrRange) {
            return TaskStatus.Failure;
        }
        Unit.AttackController.Attack();
        return TaskStatus.Running;
    }
}
