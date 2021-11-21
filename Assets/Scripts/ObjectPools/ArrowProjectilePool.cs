using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectilePool : MonoBehaviourObjectPool<ArrowProjectilePool, ArrowProjectile> {
    protected override string _PrefabPath {
        get {
            return "Prefabs/Projectiles/Arrow";
        }
    }
}
