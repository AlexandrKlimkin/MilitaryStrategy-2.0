public class FireBallsPool : MonoBehaviourObjectPool<FireBallsPool, FireBallProjectile> {
    protected override string _PrefabPath {
        get {
            return "Prefabs/Projectiles/Fireball";
        }
    }
}