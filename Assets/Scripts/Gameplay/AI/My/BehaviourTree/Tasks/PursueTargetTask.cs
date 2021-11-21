using System.Collections;
using System.Collections.Generic;
using Tools.BehaviourTree;
using UnityEngine;

public class PursueTargetTask : UnitTask {

    public override TaskStatus Run() {
        if (Unit.AttackController.Target == null || Unit.AttackController.Target.Dead)
            return TaskStatus.Failure;
        var sqrDistToTarget = Unit.AttackController.SqrDistanceToTarget;
        if (sqrDistToTarget > Unit.AttackController.SqrRange) {
            Unit.AttackController.Target = null;
            Unit.MoveController.IsStopped = true;
            return TaskStatus.Failure;
        }
        if (sqrDistToTarget < Unit.AttackController.Weapon.SqrRange) {
            Unit.MoveController.IsStopped = true;
            return TaskStatus.Success;
        }
        var targetPos = Unit.AttackController.Target.transform.position;
        UnitBlackboard.MoveDestination = targetPos;
        Unit.MoveController.MoveToPoint(targetPos);
        return TaskStatus.Running;
    }

}
