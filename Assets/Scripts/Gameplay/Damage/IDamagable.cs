public interface IDamagable {
    float MaxHealth { get; }
    float Health { get; }

    void TakeDamage(Damage damage);
}
