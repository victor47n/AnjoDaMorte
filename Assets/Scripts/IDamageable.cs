using UnityEngine;

public interface IDamageable
{
    void TakeHit(float damage, Collision hit);

    void TakeDamage(float damage);

    void BloodParticle(Vector3 pos, Quaternion rot);
}
