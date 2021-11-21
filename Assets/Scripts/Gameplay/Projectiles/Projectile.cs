using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {
    public Damage Damage { get; protected set; }
    public float Speed = 20f;
    protected Vector3 _Velocity;
    protected bool _Initialized;

    protected virtual void FixedUpdate() {
        if (!_Initialized) {
            //gameObject.SetActive(false);
            return;
        }
        SimulateStep(Time.fixedDeltaTime);
        if (_Velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_Velocity);
    }

    protected abstract void SimulateStep(float time);

    protected abstract void Hit();

    protected virtual void OnDisable() {
        _Initialized = false;
        _Velocity = Vector3.zero;
        Damage = null;
    }
}

public abstract class Projectile<T> : Projectile where T: ProjectileInit {
    public abstract void Initialize(T parameters);
}

public class ProjectileInit {
    public Damage Damage;
}
