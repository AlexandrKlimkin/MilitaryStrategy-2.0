using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    //public float Damage;
    public float Range;
    public float SqrRange { get { return Range * Range; } }
    //public float ReloadTime;
    //private float _CoolDownTime;
    //private float _SqrRange;
    private Actor _Target;

    public Unit Owner { get; private set; }
    public MoveController MoveController { get; private set; }
    public Weapon Weapon { get; private set; }
    public Actor Target {
        get {
            return _Target;
        }
        set {
            if (_Target != null)
                _Target.OnDeath -= OnTargetDeath;
            _Target = (value == null || value.Dead) ? null : value;
            if (_Target != null)
                _Target.OnDeath += OnTargetDeath;
            SqrDistanceToTarget = Target != null ? Vector3.SqrMagnitude(transform.position - Target.transform.position) : float.PositiveInfinity;
        }
    }
    public float SqrDistanceToTarget { get; private set; }
    public bool TargetInRange { get { return Target != null && SqrDistanceToTarget <= Weapon.SqrRange; } }
    public bool CanAttack { get { return !Owner.Dead && Target != null; } }

    private void Awake() {
        Owner = GetComponentInParent<Unit>();
        Weapon = GetComponentInChildren<Weapon>();
    }

    private void Start() {
        MoveController = Owner.MoveController;
        Owner.OnDeath += OnOwnerDeath;
    }

    private void Update() {
        if (Owner.Dead)
            return;
        SqrDistanceToTarget = Target != null ? Vector3.SqrMagnitude(transform.position - Target.transform.position) : float.PositiveInfinity;
    }

    public void Attack() {
        MoveController.ForceLookAt(Target);
        if (!CanAttack || !Weapon.Reloaded)
            return;
        if (Weapon.Reloaded) {
            Weapon.Attack();
            Owner.Animator.SetTrigger("Attack");
        }
    }

    public void PerformHit() {
        Weapon.Hit();
    }

    public bool CheckActorInVisionRange(Actor other, out float sqrDist) {
        sqrDist = SqrDistanceToActor(other);
        if (sqrDist < Range * Range)
            return true;
        else
            return false;
    }

    public bool CheckActorInWeaponRange(Actor other, out float sqrDist) {
        sqrDist = SqrDistanceToActor(other);
        if (Weapon == null)
            return false;
        if (sqrDist < Weapon.SqrRange)
            return true;
        else
            return false;
    }

    public float SqrDistanceToActor(Actor other) {
        if (other == null)
            return float.PositiveInfinity;
        return Vector3.SqrMagnitude(Owner.transform.position - other.transform.position);
    }

    private void OnOwnerDeath() {
        Target = null;
    }

    private void OnTargetDeath() {
        Target = null;
    }

    private void OnDestroy() {
        Owner.OnDeath -= OnOwnerDeath;
    }
}
