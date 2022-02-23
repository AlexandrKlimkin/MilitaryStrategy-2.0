using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAnimator
{
    public class State
    {
        public int Id;
        public float AnimationSpeed;
        public bool UseRootMotion;
        public List<Transition> Transitions;
        public float ExitTime;
        public List<AnimationEvent> Events;
    }
    
    public class AnimationEvent
    {
        public Action Action;
        public float NormalizedTime;
        public bool WasThrown;
    }
}
