using System.Collections;
using UnityEngine;

public interface ICameraTarget {
    Vector3 LookPosition { get; }
}

public class CameraBehaviour : SingletonBehaviour<CameraBehaviour> {
    [SerializeField]
    private float _TargetAngleX;
    [SerializeField]
    private float _TargetAngleY;
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

    [SerializeField]
    private float _DistanceFromTarget = 10f;

    public ICameraTarget Target { get; set; }

    public Vector3 ForwardVector {
        get {
            return Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        }
    }

    private bool _NoDampingUpdate;

    void Start() {
        Target = PlayerController.Instance?.Unit;
        InputSystem.Instance.RotateCameraLeft += RotateLeftAroundTarget;
        InputSystem.Instance.RotateCameraRight += RotateRightAroundTarget;
        InputSystem.Instance.MouseScroll += ScrollToTarget;
        InputSystem.Instance.MoveCameraUp += MoveUp;
        InputSystem.Instance.MoveCameraDown += MoveDown;
        _TargetAngleY = transform.rotation.y;
    }

    void Update() {
        if(Target == null)
            return;
        ProcessMove();
        ProcessRotate();
        _NoDampingUpdate = false;
    }

    private void ProcessMove() {
        var targetPos = Target.LookPosition - transform.forward * _DistanceFromTarget;
        if(_NoDampingUpdate)
            transform.position = targetPos;
        else
            transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * _MoveSmoothness);
    }

    private void ProcessRotate() {
        transform.rotation = Quaternion.Euler(_TargetAngleX, _TargetAngleY, 0);
    }

    private void RotateAroundTarget(bool right) {
        if(Target != null) {
            var sideCof = right ? 1 : -1;
            _TargetAngleY += Time.deltaTime * _ForceRotateSmothness * sideCof;
        }
        _NoDampingUpdate = true;
    }

    private void RotateRightAroundTarget() {
        RotateAroundTarget(true);
    }

    private void RotateLeftAroundTarget() {
        RotateAroundTarget(false);
    }

    private void MoveDown() {
        _TargetAngleX -= Time.deltaTime * _MoveSpeed;
    }

    private void MoveUp() {
        _TargetAngleX += Time.deltaTime * _MoveSpeed;
    }

    private void ScrollToTarget(float delta) {
        _DistanceFromTarget -= delta * _ScrollSmothness;
    }

}