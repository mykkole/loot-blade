using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float damageRecoveryTime = 0.5f;

    [Header("Dash")]
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashCollDownTime = 0.3f;

    private Vector2 inputVector;

   [SerializeField] private float movingSpeed = 7f;

    private float minMovingSped = 0.1f;
    private bool isRunning = false;

    public bool IsFacingRight { get;  set; } = true;

    private Rigidbody2D rb;
    private KnockBack knockBack;

    private int currentHeath;

    private bool canTakeDamage;
    private bool isAlive;

    private float initialMovingSpeed;

    private bool isDashing;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
        initialMovingSpeed = movingSpeed;
    }

    private void Start()
    {
        isAlive = true;
        currentHeath = maxHealth;
        canTakeDamage = true;

        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += Instance_OnPlayerDash;
    }

    private void Instance_OnPlayerDash(object sender, EventArgs e)
    {
        if (!isDashing)
        {
            Dash();
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        movingSpeed *= dashSpeed;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);

        trailRenderer.emitting = false;
        movingSpeed = initialMovingSpeed;

        yield return new WaitForSeconds(dashCollDownTime);
        isDashing = false;
    }

    private void Dash()
    {
        StartCoroutine(DashRoutine());
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        ActiveWeapone.Instance.GetActiveWeapon().Attack();
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (knockBack.IsGettingKnockBack)
        {
            return;
        }
        HandleMovement();
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (canTakeDamage && isAlive)
        {
            canTakeDamage =  false;
            currentHeath = Math.Max(0, currentHeath -= damage);
            Debug.Log(currentHeath);

            knockBack.GetKnockBackMovement(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRoutine());
        }
        DetectDeath();

    }

    private void DetectDeath()
    {
        if (currentHeath == 0 && isAlive)
        {
            isAlive = false;
            knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void HandleMovement()
    {
        
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSped || Mathf.Abs(inputVector.y) > minMovingSped)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
    }
}
