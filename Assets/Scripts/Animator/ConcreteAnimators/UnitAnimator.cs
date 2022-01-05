using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAnimator;

public class UnitAnimator : CustomAnimatorBase
{
    public MoveController MoveController;

    public int RunAnimationId;
    public int IdleAnimationId;
    public int AttackAnimationId;
    public int HitAnimationId;
    public int DeathAnimationId;
    
    protected override StateMachineMap CreateMap()
    {
        var map = new StateMachineMap();

        var idleState = new CustomAnimator.State
        {
            Id = IdleAnimationId,
            Transitions = new List<Transition>()
            {
                new Transition()
                {
                    TransitionStateId = RunAnimationId,
                    Priority = 0,
                    Condition = (() => MoveController.IsMoving)
                }
            }
        };
        var runState = new CustomAnimator.State
        {
            Id = RunAnimationId,
            Transitions = new List<Transition>()
            {
                new Transition()
                {
                    TransitionStateId = IdleAnimationId,
                    Priority = 0,
                    Condition = (() => !MoveController.IsMoving)
                }
            }
        };

        map.States = new List<CustomAnimator.State>()
        {
            idleState,
            runState,
        };
        map.DefaultState = idleState;
        return map;
    }
}
