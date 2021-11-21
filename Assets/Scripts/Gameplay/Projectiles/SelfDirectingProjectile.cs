using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDirectingProjectile : Projectile<SelfDirectingProjectileInit> {

    public Actor Target { get; protected set; }

    protected override void SimulateStep(float time) {
        if (!_Initialized || Target == null) {
            gameObject.SetActive(false);
            return;
        }
        var direction = Target.PointToFire.position - transform.position;
        var sqrDistanceToTarget = Vector3.SqrMagnitude(direction);
        if(sqrDistanceToTarget > 0.1f) {
            _Velocity = direction.normalized * Speed;
            transform.position += _Velocity * time;
        } else {
            Hit();
            gameObject.SetActive(false);
        }
    }

    public override void Initialize(SelfDirectingProjectileInit parametrs) {
        Damage = parametrs.Damage;
        Target = parametrs.Target;
        _Initialized = true;
    }

    protected override void Hit() {
        Target.TakeDamage(Damage);
    }

    protected override void OnDisable() {
        base.OnDisable();
        Target = null;
    }
}

public class SelfDirectingProjectileInit : ProjectileInit {
    public Actor Target;
}