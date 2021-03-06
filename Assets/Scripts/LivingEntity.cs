﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    protected float health;
    protected bool dead;

    public Healthbar healthBar;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
        Time.timeScale = 1;
    }

    public virtual void TakeHit(float damage, Collision hit)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (healthBar)
        {
            healthBar.TakeDamage(damage);
        }

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void BloodParticle(Vector3 pos, Quaternion rot)
    {

    }

    [ContextMenu("Self Destruct")]
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }

        GameObject.Destroy(gameObject, 4);
    }

    public float GetHealth()
    {
        return health;
    }
}
