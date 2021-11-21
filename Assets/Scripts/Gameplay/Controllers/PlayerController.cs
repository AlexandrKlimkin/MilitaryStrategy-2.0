using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonBehaviour<PlayerController> {

    public Unit Unit { get; private set; }
    public MoveController MoveController { get; private set; }
    public AttackController AttackController { get; private set; }

    public PowerArrowAbility PowerArrowAbility { get; private set; }  // КОСТЫЛЬ

    private InputSystem _Input;

    protected override void Awake () {
        base.Awake();
        Unit = GetComponent<Unit>();
        MoveController = GetComponentInChildren<MoveController>();
        PowerArrowAbility = GetComponentInChildren<PowerArrowAbility>();
    }
	
    protected void Start() {
        AttackController = Unit.AttackController;
        _Input = InputSystem.Instance;
        _Input.TouchGroundHit += MoveToGroundHitPoint;
        _Input.TouchActorHit += AttackTarget;
        _Input.UseQAbility += UseQAbility;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.D)) {
            Unit.TakeDamage(new Damage(100));
        }
        PursueOrAttackTarget();
    }

    private void MoveToGroundHitPoint(Vector3 point) {
        AttackController.Target = null;
        MoveController.MoveToPoint(point);
    }

    private void AttackTarget(Actor actor) {
        AttackController.Target = actor;
    }

    private void UseQAbility()
    {
        var mouseHit = _Input.CameraRaycastHit;
        var targetPoint = mouseHit.point;
        if(mouseHit.transform == null)
        {
            var plane = new Plane(Unit.transform.up, Unit.transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var enter = 0f;
            if (plane.Raycast(ray, out enter))
            {
                targetPoint = ray.GetPoint(enter);
            }
        }
        var dir = Vector3.Scale(targetPoint - Unit.transform.position, new Vector3(1,0,1));
        PowerArrowAbility.Direction = dir;
        PowerArrowAbility.UseAbility();
    }

    private void PursueOrAttackTarget() {
        if(AttackController.Target != null) {
            var sqrDistToTarget = Unit.AttackController.SqrDistanceToTarget;
            if (sqrDistToTarget < Unit.AttackController.Weapon.SqrRange) {
                Unit.AttackController.Attack();
                Unit.MoveController.IsStopped = true;
            }
            else {
                var targetPos = Unit.AttackController.Target.transform.position;
                Unit.MoveController.MoveToPoint(targetPos);
            }
        }
    }

    protected override void OnDestroy() {
        _Input.TouchGroundHit -= MoveToGroundHitPoint;
        _Input.TouchActorHit -= AttackTarget;
        _Input.UseQAbility -= UseQAbility;
        base.OnDestroy();
    }
}