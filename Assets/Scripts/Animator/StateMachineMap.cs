using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAnimator
{
    public class StateMachineMap
    {
        public IReadOnlyList<State> States { get; set; }
        //public IReadOnlyDictionary<int, State> StatesDict { get; set; }
        public State DefaultState { get; set; }
    }
}
