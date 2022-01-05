using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationInstancing;

namespace CustomAnimator
{
    public abstract class CustomAnimatorBase : MonoBehaviour
    {
        public AnimationInstancing.AnimationInstancing AnimationInstancing;

        protected StateMachine _StateMachine;
        
        protected virtual void Awake()
        {
            if (AnimationInstancing == null)
                AnimationInstancing = GetComponent<AnimationInstancing.AnimationInstancing>();
        }

        protected virtual void Start()
        {
            InitializeStateMachine();
        }

        protected virtual void Update()
        {
            _StateMachine?.Update();
        }

        protected virtual void OnDestroy()
        {
            _StateMachine.StateChanged -= OnStateChanged;
        }

        private void InitializeStateMachine()
        {
            _StateMachine = new StateMachine();
            var map = CreateMap();
            _StateMachine.StateChanged += OnStateChanged;
            _StateMachine.Initialize(map);
        }

        private void OnStateChanged(State state)
        {
            AnimationInstancing.PlayAnimation(state.Id);
        }
        
        protected abstract StateMachineMap CreateMap();
    }
}
