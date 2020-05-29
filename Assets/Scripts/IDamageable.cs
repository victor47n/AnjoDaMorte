using UnityEngine;

public interface IDamageable
{
    void TakeHit(float damage, Collision hit);
}
