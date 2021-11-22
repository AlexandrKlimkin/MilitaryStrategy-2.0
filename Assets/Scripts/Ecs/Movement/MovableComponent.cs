using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct MovableComponent
{
    public Transform Transform;
    public NavMeshAgent NavMeshAgent;
    
    public float MoveSpeed;
    public bool CanMove;
    public Vector3? TargetPoint;
}
