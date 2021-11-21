using UnityEngine;

namespace Tools.BehaviourTree {

    public abstract class BehaviourTreeExecutor : MonoBehaviour {

        public BehaviourTree BehaviourTree { get; protected set; }

        protected virtual void Awake() {
            BehaviourTree = BuildBehaviourTree();
            BehaviourTree.Blackboard = BuildBlackboard();
            BehaviourTree.Executor = this;
            BehaviourTree.Init();
        }

        protected abstract BehaviourTree BuildBehaviourTree();

        protected abstract Blackboard BuildBlackboard();

        protected void UpdateBT() {
            BehaviourTree.UpdateTask();
        }
    }
}
