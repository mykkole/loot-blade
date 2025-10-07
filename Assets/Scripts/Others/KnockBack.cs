using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackMovingTimeMax = 0.3f;

    private float knockBackMovingTimer;

    private Rigidbody2D rb;

    public bool IsGettingKnockBack { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        knockBackMovingTimer -= Time.deltaTime;
        if (knockBackMovingTimer < 0)
        {
            StopKnockBackMovement();
        }
    }

    public void GetKnockBackMovement(Transform damageSource)
    {
        IsGettingKnockBack = true;
        knockBackMovingTimer = knockBackMovingTimeMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackForce;
        rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        rb.velocity = Vector2.zero;
        IsGettingKnockBack = false;
    }
}

