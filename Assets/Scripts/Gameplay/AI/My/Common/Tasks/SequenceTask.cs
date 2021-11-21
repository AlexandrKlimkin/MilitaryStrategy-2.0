using System;
using UnityEngine;

namespace Tools.BehaviourTree {

    /// <summary>
    /// Logical AND
    /// </summary>
    public class SequenceTask : Task {

        private int _CurrentTaskIndex;

        public override void Init() {
        }

        public override void Begin() {
        }

        /// <returns>
        /// Success only if all tasks completed successfully,
        /// else first unsuccessful task status.
        /// </returns>
        public override TaskStatus Run() {
            foreach(var childTask in Children) {
                var childStatus = childTask.UpdateTask();
                switch (childStatus) {
                    case TaskStatus.Success:
                    case TaskStatus.Running:
                        _CurrentTaskIndex++; break;
                    case TaskStatus.Failure:
                        return childStatus;
                }
            }
            return TaskStatus.Success;
        }
    }
}
