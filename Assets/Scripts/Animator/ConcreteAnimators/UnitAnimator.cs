using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAnimator;
using AnimationEvent = CustomAnimator.AnimationEvent;

[Serializable]
public struct AnimationInfo
{
    public string AnimationId;
    public float AnimationSpeed;
    [Range(0, 1f)]
    public float ExitTime;
    public bool UseRootMotion;
}

public class UnitAnimator : CustomAnimatorBase
{
    public Unit Unit;
    public MoveController MoveController;
    public AttackController AttackController;

    public AnimationInfo RunAnimation;
    public AnimationInfo IdleAnimation;
    public AnimationInfo AttackAnimation;
    public AnimationInfo HitAnimation;
    public AnimationInfo DeathAnimation;

    [Range(0, 1f)]
    public float AttackEventTime;
    [Range(0, 1f)]
    public float HitEventTime;

    private Transition _RunTransition;
    private Transition _IdleTransition;

    private List<AnimationEvent> _AttackEvents;

    protected override void Start()
    {
        //Transitions
        _RunTransition =
            new Transition()
            {
                TransitionStateId = RunAnimation.AnimationId,
                Priority = 0,
                Condition = (() => MoveController.IsMoving && MoveController.Velocity.sqrMagnitude > 0.01f || MoveController.Rigidbody.velocity.sqrMagnitude > 0.01f)
            };
        _IdleTransition =
            new Transition()
            {
                TransitionStateId = IdleAnimation.AnimationId,
                Priority = 0,
                Condition = (() => !MoveController.IsMoving && MoveController.Rigidbody.velocity.sqrMagnitude < 0.01f)
            };
        
        //Events
        _AttackEvents = new List<AnimationEvent>
        {
            new AnimationEvent()
            {
                Action = AttackController.PerformHit,
                NormalizedTime = 0.5f,
                WasThrown = false,
            }
        };
        
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

        var idleTransitions =
            new List<Transition>()
            {
                _RunTransition,
            };
        var idleState =
            CreateState(IdleAnimation, idleTransitions);
        
        var runTransitions = new List<Transition>()
        {
            _IdleTransition,
        };
        var runState = CreateState(RunAnimation, runTransitions);
        
        var attackTransitions = new List<Transition>()
        {
            _IdleTransition,
            _RunTransition,
        };
        var attackState = CreateState(AttackAnimation, attackTransitions, _AttackEvents);
        
        var hitTransitions = new List<Transition>()
        {
            _IdleTransition,
            _RunTransition,
        };
        var hitState = CreateState(HitAnimation, hitTransitions);
        
        var deathState = CreateState(DeathAnimation, null);

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

    private CustomAnimator.State CreateState(AnimationInfo info, List<Transition> transitions, List<AnimationEvent> events = null)
    {
        var state = new CustomAnimator.State()
        {
            Id = info.AnimationId,
            Transitions = transitions,
            AnimationSpeed = info.AnimationSpeed,
            ExitTime = info.ExitTime,
            UseRootMotion = info.UseRootMotion,
            Events = events,
        };
        return state;
    }
    
    private void OnAttack()
    {
        SetAnimationForce(AttackAnimation.AnimationId);
    }

    private void OnTakeDamage()
    {
        SetAnimationForce(HitAnimation.AnimationId);
    }
    
    private void OnDeath()
    {
        SetAnimationForce(DeathAnimation.AnimationId);
    }
}
