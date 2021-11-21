using System;
using UnityEngine;

namespace Tools.BehaviourTree {

    /// <summary>
    /// ForEach
    /// </summary>
    public class ExecuteAllTask : Task {

        public override void Init() { }

        public override void Begin() { }

        /// <returns>
        /// Always success
        /// </returns>
        public override TaskStatus Run() {
            foreach (var childTask in Children) {
                childTask.UpdateTask();
            }
            return TaskStatus.Success;
        }
    }
}
