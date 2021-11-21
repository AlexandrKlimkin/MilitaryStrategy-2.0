using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneProjectilePool : MonoBehaviourObjectPool<StoneProjectilePool, StoneProjectile> {
    protected override string _PrefabPath {
        get {
            return "Prefabs/Projectiles/StoneProjectile";
        }
    }
}
