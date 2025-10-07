using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damageAmount = 2;
    public EventHandler OnSwordSwing;

    private PolygonCollider2D polygonCollider;

    private void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        AttackColliderOff();
    }
    public void Attack()
    {
        AttackColliderOffOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity)) {
            enemyEntity.TakeDamage(damageAmount);
        }

    }

    public void AttackColliderOff()
    {
        polygonCollider.enabled = false;
    }

    private void AttackColliderOn()
    {
        polygonCollider.enabled = true;
    }

    private void AttackColliderOffOn()
    {
        AttackColliderOff();
        AttackColliderOn();
    }
}
