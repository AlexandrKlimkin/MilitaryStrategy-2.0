using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AnimationInstancing;

namespace CustomAnimator
{
    public abstract class CustomAnimatorBase : MonoBehaviour
    {
        //public AnimationInstancing.AnimationInstancing AnimationInstancing;
        public GPUSkinningPlayerMono GPUSkinningPlayerMono;

        protected AnimatorStateMachine AnimatorStateMachine;
        
        protected virtual void Awake()
        {
            // if (AnimationInstancing == null)
            //     AnimationInstancing = GetComponent<AnimationInstancing.AnimationInstancing>();
            if (GPUSkinningPlayerMono == null)
                GPUSkinningPlayerMono = GetComponent<GPUSkinningPlayerMono>();
        }

        protected virtual void Start()
        {
            InitializeStateMachine();
        }


        protected virtual void Update()
        {
            if (AnimatorStateMachine == null)
                return;
            AnimatorStateMachine?.Update();
            var curState = AnimatorStateMachine.CurrentState;
            // if (curState.Id == -1)
            //     return;
            if(string.IsNullOrEmpty(curState.Id))
                return;
            if(curState.Events == null)
                return;
            if (!curState.Events.Any())
                return;
            var normStateTime = GPUSkinningPlayerMono.Player.NormalizedTime;
            foreach (var ev in curState.Events)
            {
                if (ev.WasThrown)
                    continue;
                if (normStateTime < ev.NormalizedTime)
                    continue;
                ev.Action?.Invoke();
                ev.WasThrown = true;
                // Debug.LogError("Event was thrown!");
            }
        }

        protected virtual void OnDestroy()
        {
            if(AnimatorStateMachine != null)
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
            state.Events?.ForEach(_=>_.WasThrown = false);
            GPUSkinningPlayerMono.Player.Play(state.Id);
            
            // AnimationInstancing.PlayAnimation(state.Id);
            // AnimationInstancing.playSpeed = state.AnimationSpeed;
            // AnimationInstancing.applyRootMotion = state.UseRootMotion;
        }
        
        protected abstract StateMachineMap CreateMap();

        private bool CheckStateExitTime(State state)
        {
            if (state.ExitTime == 0)
                return false;
            // var normStateTime =
            //     AnimationInstancing.curFrame / (AnimationInstancing.aniInfo[state.Id].totalFrame - 1);
            return GPUSkinningPlayerMono.Player.NormalizedTime < state.ExitTime;
        }

        protected void SetAnimationForce(string id)
        {
            AnimatorStateMachine.SetState(id);
        }
    }
}
