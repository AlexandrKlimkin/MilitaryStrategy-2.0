using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools.BehaviourTree;
using UnityEngine;

public class FindTargetTask : UnitTask {

    public override TaskStatus Run() {
        FindClosestTarget();
        return Unit.AttackController.Target == null ? TaskStatus.Failure : TaskStatus.Success;
    }

    private void FindClosestTarget() {
        //if (UnitBlackboard.AttackTarget == null || UnitBlackboard.AttackTarget.Dead) {
            Unit closest = null;
            float sqrDistToClosest = float.PositiveInfinity;
            foreach(var unit in Unit.ActiveUnits) {
                if(unit.TeamIndex != Unit.TeamIndex) {
                    float sqrDist;
                    if(Unit.AttackController.CheckActorInVisionRange(unit, out sqrDist)) {
                        if(sqrDist < sqrDistToClosest) {
                            closest = unit;
                            sqrDistToClosest = sqrDist;
                        }
                    }
                }
            }
            //UnitBlackboard.AttackTarget = closest;
            Unit.AttackController.Target = closest;
       // }
    }
}
