using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerArrowAbility : Ability
{
    public MoveController MoveController { get; private set; }
    public Transform ProjectileSpawnPoint;
    public float Damage;
    public float ExplosionRadius;
    public float MaxDistance;

    public Vector3 Direction; // КОСТЫЛЬ

    protected override void Awake()
    {
        base.Awake();
        MoveController = Owner.MoveController;
    }

    public override void UseAbility()
    {
        MoveController.IsStopped = true;
        MoveController.ForceLookAt(Direction);
        Owner.Animator.SetTrigger("Attack");
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        var projectile = ArrowBlastProjectilePool.Instance.GetObject();
        projectile.gameObject.SetActive(true);
        projectile.transform.position = ProjectileSpawnPoint.position;
        projectile.transform.rotation = ProjectileSpawnPoint.rotation;
        var dmg = new Damage(Damage, Owner, DamageType.Middle);
        var parameters = new DirectedProjectileInit()
        {
            Damage = dmg,
            ExplosionRadius = this.ExplosionRadius,
            MaxDistance = this.MaxDistance,
            SpawnPoint = ProjectileSpawnPoint.position,
            Direction = this.Direction
        };
        projectile.Initialize(parameters);
    }
}