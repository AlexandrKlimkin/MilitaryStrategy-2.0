using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedProjectile : Projectile<DirectedProjectileInit>
{
    public Vector3 Direction { get; private set; }
    private Vector3 _NormalizedDirection;
    public Vector3 SpawnPosition { get; private set; }
    public float MaxDistance { get; private set; }
    private float _SqrMaxDistance;
    public float ExplosionRadius;
    private string _ExplosionEffectName = "FireSplashEffect";

    protected override void SimulateStep(float time)
    {
        if (!_Initialized || Direction == Vector3.zero)
        {
            gameObject.SetActive(false);
            return;
        }

        var sqrPassedDistance = Vector3.SqrMagnitude(transform.position - SpawnPosition);
        if (sqrPassedDistance < _SqrMaxDistance)
        {
            transform.position += _Velocity * time;
        }
        else
        {
            Hit();
        }
    }

    public override void Initialize(DirectedProjectileInit parametrs)
    {
        SpawnPosition = parametrs.SpawnPoint;
        Direction = parametrs.Direction;
        _NormalizedDirection = Direction.normalized;
        _Velocity = _NormalizedDirection * Speed;
        Damage = parametrs.Damage;
        MaxDistance = parametrs.MaxDistance;
        _SqrMaxDistance = MaxDistance * MaxDistance;
        _Initialized = true;
        ExplosionRadius = parametrs.ExplosionRadius;
    }

    protected override void Hit()
    {
        var actorColliders = Physics.OverlapSphere(transform.position, ExplosionRadius, Constants.Layers.Masks.Actor, QueryTriggerInteraction.Ignore);
        foreach(var col in actorColliders)
        {
            var actor = col.GetComponent<Actor>();
            if(actor)
                actor.TakeDamage(Damage);
        }
        ExplosionEffect();
        gameObject.SetActive(false);
    }

    private void ExplosionEffect()
    {
        var explosion = VisualEffect.GetEffect<ParticleEffect>(_ExplosionEffectName);
        explosion.transform.position = transform.position;
        explosion.transform.rotation = transform.rotation;
        explosion.Play();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if ((Constants.Layers.Masks.Damagable & (1 << other.gameObject.layer)) != 0)
        {
            Hit();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Direction = Vector3.zero;
        _NormalizedDirection = Vector3.zero;
        SpawnPosition = Vector3.zero;
        MaxDistance = 0;
        _SqrMaxDistance = 0;
    }
}

public class DirectedProjectileInit : ProjectileInit
{
    public Vector3 SpawnPoint;
    public Vector3 Direction;
    public float ExplosionRadius;
    public float MaxDistance;
}
