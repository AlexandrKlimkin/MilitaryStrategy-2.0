using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveController : MonoBehaviour {

    public float Speed = 5f;
    public float AngularSpeed = 1080f;

    public float AvoidUnitsCD;
    
    public Vector3 Velocity { get; private set; }
    public Vector3 RBVelocity => Rigidbody.velocity;
    public bool IsStopped { get; set; }
    public Vector3 DestinationPoint { get; private set; }
    private bool _DestinationPointReached;
    private Vector3 _TargetPathPoint;
    private int _TargetPathPointIndex;
    private Quaternion _TargetRotation;

    public float MiddleDmgStanTime = 1.5f;

    public Rigidbody Rigidbody;
    
    public Unit Owner { get; private set; }

    public NavMeshPath Path { get; private set; }

    private Coroutine _StanRoutine;
    private float _LastAvoidUnitsTime;


    public bool CanMove {
        get {
            return !Owner.Dead && !Staned;
        }
    }

    public bool Staned { get; private set; }

    public bool IsMoving =>
        //return _IsMoving;
        CanMove && !IsStopped && Velocity.sqrMagnitude > 0f;

    // public bool _IsMoving;

    private void Awake() {
        Owner = GetComponent<Unit>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        Path = new NavMeshPath();
        _TargetRotation = Owner.transform.rotation;
        IsStopped = true;
        Owner.OnDeath += OnOwnerDeath;
        _LastAvoidUnitsTime = Random.Range(0, AvoidUnitsCD);
        //Owner.Animator.SetFloat("StanTime", MiddleDmgStanTime);
    }

    private void Update() {
        if (Owner.Dead)
            return;
        MoveAlongPath();
        //AvoidUnits();
        RotateUnit();
        //UpdateAnimator();
        DrawPath();
    }

    public void MoveToPoint(Vector3 point) {
        IsStopped = false;
        var havePath = NavMesh.CalculatePath(Owner.transform.position, point, NavMesh.AllAreas, Path);
        if (havePath) {
            _DestinationPointReached = false;
            DestinationPoint = Path.corners[Path.corners.Length - 1];
            _TargetPathPointIndex = 1;
            _TargetPathPoint = Path.corners[_TargetPathPointIndex];
        }
    }

    private void MoveAlongPath() {
        if (_DestinationPointReached || !CanMove || IsStopped)
        {
            Velocity = Vector3.zero;
            return;
        }
        var direction = _TargetPathPoint - Owner.transform.position;
        var sqrDistToTargetPoint = Vector3.SqrMagnitude(direction);
        if (sqrDistToTargetPoint > 0.1f) {
            Velocity = direction.normalized * Speed * Time.deltaTime;
        }
        else {
            if(_TargetPathPointIndex < Path.corners.Length - 1) {
                _TargetPathPointIndex++;
                _TargetPathPoint = Path.corners[_TargetPathPointIndex];
            }
            else {
                _DestinationPointReached = true;
                Velocity = Vector3.zero;
            }
        }
        Rigidbody.MovePosition(Rigidbody.position + Velocity);
    }

    private void AvoidUnits()
    {
        var raycastPos = transform.position + Vector3.up * 0.5f;
        var length = 2f;
        var avoidStrength = 1f;
        
        if (Time.time > _LastAvoidUnitsTime + AvoidUnitsCD)
        {
            if (Physics.Raycast(raycastPos, transform.forward, out var hit, length,
                Constants.Layers.Masks.Actor, QueryTriggerInteraction.Ignore))
            {
                var dir = hit.collider.transform.position - raycastPos;
                Velocity = (Velocity + transform.right * avoidStrength).normalized * Speed * Time.deltaTime;
            }
            _LastAvoidUnitsTime = Time.time;
        }
        Debug.DrawLine(raycastPos, raycastPos + Velocity, Color.green);
    }

    private void RotateUnit() {
        if (IsMoving)
            _TargetRotation = Quaternion.LookRotation(Vector3.Scale(Velocity, new Vector3(1, 0, 1)), Vector3.up);
        Owner.transform.rotation = Quaternion.Lerp(Owner.transform.rotation, _TargetRotation, AngularSpeed * Time.deltaTime);
    }

    public void ForceLookAt(Actor actor) {
        if (actor == null)
            return;
        var dir = Vector3.Scale(actor.transform.position - Owner.transform.position, new Vector3(1,0,1));
        ForceLookAt(dir);
    }

    public void SetToGround()
    {
        
    }

    public void ForceLookAt(Vector3 direction)
    {
        _TargetRotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void OnHit() {
        if (_StanRoutine != null)
            StopCoroutine(_StanRoutine);
        _StanRoutine = StartCoroutine(StanRoutine(MiddleDmgStanTime));
    }
    
    private IEnumerator StanRoutine(float time) {
        Staned = true;
        Velocity = Vector3.zero;
        yield return new WaitForSeconds(time);
        Staned = false;
    }

    public void UpdateAnimator() {
        // Owner.Animator.SetBool("Moving", IsMoving);
    }

    private void OnOwnerDeath() {
        IsStopped = true;
    }

    private void DrawPath()
    {
        if (Path != null)
            for (int i = 0; i < Path.corners.Length - 1; i++)
                Debug.DrawLine(Path.corners[i], Path.corners[i + 1], Color.red);
    }

    private void OnDestroy() {
        Owner.OnDeath -= OnOwnerDeath;
    }
}