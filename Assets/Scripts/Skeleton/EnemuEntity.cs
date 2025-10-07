using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;

    private int currentHealth;

    private PolygonCollider2D polygonCollider2D;
    private BoxCollider2D boxCollider2D;
    private EnemyAI enemyAI;

    private void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Start()
    {
        currentHealth = enemySO.enemyHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, enemySO.enemyDamageAmount);
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        polygonCollider2D.enabled = true;
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            boxCollider2D.enabled = false;
            polygonCollider2D.enabled = false;

            enemyAI.SetDeathState();

            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }

}
