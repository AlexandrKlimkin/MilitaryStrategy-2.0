using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultWeapon : Weapon {

    public Transform ProjectileSpawnPoint;

    public override void OnStartAttack() { }

    public override void Hit() {
        var projectile = StoneProjectilePool.Instance.GetObject();
        projectile.gameObject.SetActive(true);
        projectile.transform.position = ProjectileSpawnPoint.position;
        projectile.transform.rotation = ProjectileSpawnPoint.rotation;
        projectile.Rigidbody.AddForce( (transform.forward + transform.up * 0.1f) * 50f, ForceMode.VelocityChange);
    }
}
