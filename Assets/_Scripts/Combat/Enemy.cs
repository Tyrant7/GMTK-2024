using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{
    public Action<Enemy> OnDeath;

    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        Behave();
    }
    protected abstract void Behave();

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
