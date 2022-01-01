using UnityEngine;

namespace Tools.BehaviourTree
{

    public class WaitForTimeTask : Task
    {
        public float WaitTime = 1f;
        private float _LastTime;

        public override void Init() { }

        public override void Begin() { }

        public override TaskStatus Run()
        {
            if (Time.time < _LastTime + WaitTime)
                return TaskStatus.Success;
            return TaskStatus.Running;
        }
    }
}