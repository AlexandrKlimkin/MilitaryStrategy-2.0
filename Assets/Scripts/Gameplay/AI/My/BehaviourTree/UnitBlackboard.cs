using System;
using Tools.BehaviourTree;
using UnityEngine;

[Serializable]
public class UnitBlackboard : Blackboard {
    //public Actor AttackTarget { get; set; }
    public Vector3? MoveDestination { get; set; }
}