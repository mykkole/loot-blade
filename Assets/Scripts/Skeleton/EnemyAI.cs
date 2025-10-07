using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using LootAndBlade.Utils;
using UnityEngine.Diagnostics;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimeMax = 2f;

    [SerializeField] private bool isAttacking = false;

    [SerializeField] private bool isChasingEnemy = false;

    [SerializeField] private float attackDistance = 1.7f;
    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;

    [SerializeField] private float chasingDistance = 4f;
    [SerializeField] private float chasingSpedMultiplier = 2f;

    private NavMeshAgent navMeshAgent;
    private State currentState;
    private float roamingTime;
    private Vector3 roamingPosition;
    private Vector3 startPosition;

    private float roamingSpeed;
    private float chasingSpeed;

    public event EventHandler OnEnemyAttack;

    private float nextCheckDirectionTime = 0f;
    private float checkDirectionDuration = 0.1f;
    private Vector3 lastPosition;


    private enum State
    {
        Idle,
        Chasing,
        Attack,
        Death,
        Roaming
    }



    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false; //чтобы не крутился объект
        navMeshAgent.updateUpAxis = false; // чтобы ориентация навмеша не влияла на ориентацию объекта
        currentState = startingState;

        roamingSpeed = navMeshAgent.speed;
        chasingSpeed = navMeshAgent.speed * chasingSpedMultiplier;
    }

    private void Update()
    {
        StateHandler();
        MovementDirectionalHandler();
    }

    public void SetDeathState()
    {
        navMeshAgent.ResetPath();
        currentState = State.Death;
    }


    private void StateHandler()
    {
        switch (currentState)
        {
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimeMax;
                }
                GetCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                GetCurrentState();
                break;
            case State.Attack:
                AttackTarget();
                GetCurrentState();
                break;
            case State.Death:
                break;

            default:
            case State.Idle:
                break;
        }
    }

    private void AttackTarget()
    {
        if (Time.time > nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);

            nextAttackTime = Time.time + attackRate;
        }
    }

    private void ChasingTarget()
    {
        navMeshAgent.SetDestination(Player.Instance.transform.position);

    }

    public float GetRoamingAnimationSpeed()
    {
        return navMeshAgent.speed / roamingSpeed;
    }
    private void GetCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (isChasingEnemy) 
        { 
            if (distanceToPlayer <= chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (isAttacking)
        {
            if (distanceToPlayer <= attackDistance)
            {
                if (Player.Instance.IsAlive())
                {
                    newState = State.Attack;
                }
                else
                {
                    newState = State.Roaming;
                }
                
            }
        }
        if (newState != currentState)
        {
            if (newState==State.Chasing)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                roamingTime = 0f;
                navMeshAgent.speed = roamingSpeed;
            }
            else if (newState == State.Attack)
            {
                navMeshAgent.ResetPath();
            }

            currentState = newState;
        }
    }
    public bool IsRunning()
    {
        if (navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void Roaming()
    {
        startPosition = transform.position;
        roamingPosition = GetRoamingPos();
        //ChangeFacingDirection(startPosition, roamingPosition);
        navMeshAgent.SetDestination(roamingPosition);
    }

    private Vector3 GetRoamingPos()
    {
        return startPosition + LootAndBlade.Utils.Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private void MovementDirectionalHandler()
    {
        if (Time.time > nextCheckDirectionTime)
        {
            if (IsRunning())
            {
                ChangeFacingDirection(lastPosition, transform.position);
            }
            else if (currentState == State.Attack)
            {
                ChangeFacingDirection(transform.position, Player.Instance.transform.position);
            }

            lastPosition = transform.position;
            nextCheckDirectionTime = Time.time + checkDirectionDuration;
        }
    }

    private void ChangeFacingDirection(Vector3 sourcePos, Vector3 targetPos)
    {
        if (sourcePos.x > targetPos.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
