using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAnimator
{
    public class Transition
    {
        public Func<bool> Condition;
        public int Priority;
        public int TransitionStateId;
    }
}
