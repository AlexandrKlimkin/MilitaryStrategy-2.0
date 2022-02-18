using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomAnimator
{
    public class AnimatorStateMachine
    {
        public StateMachineMap Map { get; protected set; }

        public State CurrentState { get; protected set; } = new State {Id = -1};
        
        public event Action<State> StateChanged;

        private Func<State, bool> _StateReturnCondition;
        
        public virtual void Initialize(StateMachineMap map)
        {
            Map = map;
            SetState(map.DefaultState.Id);
        }
        
        public virtual void Update()
        {
            if(CurrentState.Id == -1)
                return;
            if(_StateReturnCondition != null && _StateReturnCondition.Invoke(CurrentState))
                return;
            var transitions = CurrentState.Transitions;
            if (transitions == null)
                return;
            var validTransitions = transitions.Where(_=>_.Condition.Invoke());
            if(!validTransitions.Any())
                return;
            Transition highestPriorityTransition = null;
            int highestPriority = Int32.MinValue;
            foreach (var transition in validTransitions)
            {
                if(transition.Priority <= highestPriority)
                    continue;
                highestPriorityTransition = transition;
                highestPriority = transition.Priority;
            }
            SetState(highestPriorityTransition.TransitionStateId);
        }
        
        public virtual void SetState(int id)
        {
            var state = Map?.States?.FirstOrDefault(_ => _.Id == id);
            if(state == null)
                return;
            CurrentState = state;
            StateChanged?.Invoke(CurrentState);
        }

        public void SetStateReturnCondition(Func<State, bool> condition)
        {
            _StateReturnCondition = condition;
        }
    }
}
