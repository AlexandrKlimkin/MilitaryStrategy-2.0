using System.Collections;
using Gameplay.Camera;
using UnityEngine;

public interface ICameraTarget {
    Vector3 LookPosition { get; }
    Quaternion LookRotation { get; }
    bool UseRotation { get; }
}

public class CameraBehaviour : SingletonBehaviour<CameraBehaviour> {
    [SerializeField]
    private float _MoveSmoothness;
    [SerializeField]
    private float _RotateSmothness;
    [SerializeField]
    private float _ForceRotateSmothness;
    [SerializeField]
    private float _ScrollSmothness;
    [SerializeField]
    private float _MoveSpeed;
    // [SerializeField]
    // private float _RotateSpeed;

    [SerializeField]
    private float _DistanceFromTarget = 10f;

    [Interval(1, 100)] [SerializeField] private Vector2 _MaxMinDist;

    public ICameraTarget Target { get; set; }

    [SerializeField] private bool _UseStartTarget;
    [SerializeField] private CameraTarget _StartTarget;
    
    public Vector3 ForwardVector {
        get {
            return Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        }
    }

    public float DistanceFromTarget
    {
        get => _DistanceFromTarget;
        set => _DistanceFromTarget = Mathf.Clamp(_DistanceFromTarget, _MaxMinDist.x, _MaxMinDist.y);
    }
    
    private bool _NoDampingUpdate;

    void Start() {
        if (_UseStartTarget)
        {
            _StartTarget.LookRotation = transform.rotation;
            Target = _StartTarget;
        }

        InputSystem.Instance.MouseScroll += ScrollToTarget;
    }

    void Update() {
        if(Target == null)
            return;
        if (Target.UseRotation)
            transform.rotation = Target.LookRotation;
        ProcessMove();

    }

    private void ProcessMove() {
        var targetPos = Target.LookPosition - transform.forward * _DistanceFromTarget;
 
            transform.position = targetPos;
        // else
        //     transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * _MoveSmoothness);
    }

    private void ScrollToTarget(float delta)
    {
        _DistanceFromTarget = Mathf.Clamp(_DistanceFromTarget - delta * _ScrollSmothness, _MaxMinDist.x, _MaxMinDist.y);
    }

}