using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;

    private Animator animator;

    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_DIE = "IsDie";

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyAI.OnEnemyAttack += enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += EnemyEntity_OnTakeHit;
        enemyEntity.OnDeath += EnemyEntity_OnDeath;
    }

    private void EnemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        spriteRenderer.sortingOrder = -1;
    }

    private void EnemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TAKE_HIT);
    }

    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit -= EnemyEntity_OnTakeHit;
        enemyEntity.OnDeath -= EnemyEntity_OnDeath;
    }


    private void Update()
    {
        animator.SetBool(IS_RUNNING, enemyAI.IsRunning());
        animator.SetFloat(CHASING_SPEED_MULTIPLIER, enemyAI.GetRoamingAnimationSpeed());
    }
    public void TriggerAttackAnimationOff()
    {
        enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationOn()
    {
        enemyEntity.PolygonColliderTurnOn();
    }
    private void enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK);
    }

}
