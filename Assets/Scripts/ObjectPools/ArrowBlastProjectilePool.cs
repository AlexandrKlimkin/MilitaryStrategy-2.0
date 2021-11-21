public class ArrowBlastProjectilePool : MonoBehaviourObjectPool<ArrowBlastProjectilePool, ArrowBlastProjectile>
{
    protected override string _PrefabPath {
        get {
            return "Prefabs/Projectiles/ArrowBlast";
        }
    }
}
