using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAnimator;

public class UnitAnimator : CustomAnimatorBase
{
    public Unit Unit;
    public MoveController MoveController;
    public AttackController AttackController;

    public int RunAnimationId;
    public int IdleAnimationId;
    public int AttackAnimationId;
    public int HitAnimationId;
    public int DeathAnimationId;

    protected override void Start()
    {
        base.Start();
        AttackController.OnAttack += OnAttack;
        Unit.OnDamageTake += OnTakeDamage;
        Unit.OnDeath += OnDeath;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AttackController.OnAttack -= OnAttack;
        Unit.OnDamageTake -= OnTakeDamage;
        Unit.OnDeath -= OnDeath;
    }

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
            },
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
        var attackState = new CustomAnimator.State
        {
            Id = AttackAnimationId,
            Transitions = new List<Transition>()
            {
                new Transition()
                {
                    TransitionStateId = IdleAnimationId,
                    Priority = 0,
                    Condition = () => !MoveController.IsMoving,
                },
                new Transition()
                {
                    TransitionStateId = RunAnimationId,
                    Priority = 0,
                    Condition = () => MoveController.IsMoving,
                }
            },
            ExitTime = 1f,
        };
        var hitState = new CustomAnimator.State
        {
            Id = HitAnimationId,
            Transitions = new List<Transition>()
            {
                new Transition()
                {
                    TransitionStateId = IdleAnimationId,
                    Priority = 0,
                    Condition = () => !MoveController.IsMoving,
                },
                new Transition()
                {
                    TransitionStateId = RunAnimationId,
                    Priority = 0,
                    Condition = () => MoveController.IsMoving,
                }
            },
            ExitTime = 1f,
        };
        var deathState = new CustomAnimator.State
        {
            Id = DeathAnimationId,
        };

        map.States = new List<CustomAnimator.State>()
        {
            idleState,
            runState,
            attackState,
            hitState,
            deathState,
        };
        map.DefaultState = idleState;
        return map;
    }

    private void OnAttack()
    {
        SetAnimationForce(AttackAnimationId);
    }

    private void OnTakeDamage()
    {
        SetAnimationForce(HitAnimationId);
    }
    
    private void OnDeath()
    {
        SetAnimationForce(DeathAnimationId);
    }
}
