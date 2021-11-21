using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon {

    public Transform ProjectileSpawnPoint;

    public override void OnStartAttack() { }

    public override void Hit() {
        if (Controller.Target == null)
            return;
        var projectile = ArrowProjectilePool.Instance.GetObject();
        projectile.gameObject.SetActive(true);
        projectile.transform.position = ProjectileSpawnPoint.position;
        projectile.transform.rotation = ProjectileSpawnPoint.rotation;
        var dmg = new Damage(Damage, Controller.Owner, DamageType.Middle);
        var parameters = new SelfDirectingProjectileInit() { Damage = dmg, Target = Controller.Target };
        projectile.Initialize(parameters);
    }
}