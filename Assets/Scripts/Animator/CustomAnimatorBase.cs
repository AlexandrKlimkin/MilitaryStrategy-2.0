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

        protected AnimatorStateMachine AnimatorStateMachine;
        
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
            AnimatorStateMachine?.Update();
        }

        protected virtual void OnDestroy()
        {
            AnimatorStateMachine.StateChanged -= OnAnimatorStateChanged;
        }

        private void InitializeStateMachine()
        {
            AnimatorStateMachine = new AnimatorStateMachine();
            var map = CreateMap();
            AnimatorStateMachine.StateChanged += OnAnimatorStateChanged;
            AnimatorStateMachine.Initialize(map);
            AnimatorStateMachine.SetStateReturnCondition(CheckStateExitTime);
        }

        private void OnAnimatorStateChanged(State state)
        {
            AnimationInstancing.PlayAnimation(state.Id);
        }
        
        protected abstract StateMachineMap CreateMap();

        private bool CheckStateExitTime(State state)
        {
            if (state.ExitTime == 0)
                return false;
            var normStateTime =
                AnimationInstancing.curFrame / (AnimationInstancing.aniInfo[state.Id].totalFrame - 1);
            return normStateTime < state.ExitTime;
        }

        protected void SetAnimationForce(int id)
        {
            AnimatorStateMachine.SetState(id);
        }
    }
}
