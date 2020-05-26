using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : LivingEntity
{
    public enum State { Idle, Chasing, Attacking };
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    float distance;
    private Vector3 finalTurnEnemyLookDir;

    float attackDistanceThreshold = 3f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    private Animator animationEnemy;
    private Vector3 move;
    private Vector3 moveInput;
    private float forwardAmount;
    private float turnAmount;

    /* Gun variable */
    GunController gunController;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        gunController = GetComponent<GunController>();
        animationEnemy = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        // target.
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > 12.3)
        {
            currentState = State.Idle;
        }
        else
        {
            currentState = State.Chasing;
        }

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        SetupAnimator();

        StartCoroutine(UpdatePath());
    }

    void Update()
    {

        // if (target. == false)
        // {
        // }
        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);

            bool enoughDistance = Time.time > nextAttackTime && distance < 8.05;

            if (enoughDistance == true)
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }

        // if (distance > 8.06)
        // {
        //     StartCoroutine(UpdatePath());
        // }
    }

    // void FixedUpdate()
    // {

    // }

    IEnumerator Attack()
    {
        float attackSpeed = .5f;
        float percent = 0;

        if (distance >= 6.05 && distance < 8.05)
        {
            if (target != null)
            {
                currentState = State.Attacking;
                pathfinder.enabled = true;

                // animationEnemy.SetBool("Moviment", true);
                animationEnemy.SetBool("Firing", true);
                UpdateAnimator(distance);

                while (percent <= 1)
                {
                    if (target != null)
                    {
                        Vector3 turnEnemyLookDir = (target.position - transform.position).normalized;
                        turnEnemyLookDir.y = 0f;

                        finalTurnEnemyLookDir = Vector3.Lerp(finalTurnEnemyLookDir, turnEnemyLookDir, Time.deltaTime * 8);
                        transform.rotation = Quaternion.LookRotation(finalTurnEnemyLookDir);


                        percent += Time.deltaTime * attackSpeed;
                        // Atirar
                        gunController.OnTriggerHold();
                    }

                    yield return null;
                }
                gunController.OnTriggerRelease();
            }
        }
        else if (distance < 6.05)
        {
            if (target != null)
            {
                currentState = State.Attacking;
                pathfinder.enabled = false;

                // animationEnemy.SetBool("Moviment", false);
                animationEnemy.SetBool("Firing", true);
                UpdateAnimator(distance);

                while (percent <= 1)
                {
                    if (target != null)
                    {
                        Vector3 turnEnemyLookDir = (target.position - transform.position).normalized;
                        turnEnemyLookDir.y = 0f;

                        finalTurnEnemyLookDir = Vector3.Lerp(finalTurnEnemyLookDir, turnEnemyLookDir, Time.deltaTime * 8);
                        transform.rotation = Quaternion.LookRotation(finalTurnEnemyLookDir);

                        percent += Time.deltaTime * attackSpeed;
                        // Atirar
                        gunController.OnTriggerHold();
                    }

                    yield return null;
                }
                gunController.OnTriggerRelease();
            }
        }

        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;
        // Debug.Log("ta no path: " + refreshRate);

        while (target != null)
        {
            if (distance > 12.1)
            {
                currentState = State.Idle;
            }
            else if (distance >= 8.06 && distance <= 12.1)
            {
                currentState = State.Chasing;
            }

            if (currentState == State.Idle)
            {
                pathfinder.enabled = false;
                // animationEnemy.SetBool("Moviment", false);
                // Debug.Log("Ta no idle com a distancia: " + distance);
                UpdateAnimator(distance);
                animationEnemy.SetBool("Firing", false);
            }

            if (currentState == State.Chasing)
            {
                // Debug.Log("Ta no chasing com a distancia: " + distance);
                // animationEnemy.SetBool("Moviment", true);
                // animationEnemy.SetBool("Firing", false);
                pathfinder.enabled = true;

                Vector3 direcao = target.transform.position - transform.position;

                Quaternion novaRotacao = Quaternion.LookRotation(direcao);
                GetComponent<Rigidbody>().MoveRotation(novaRotacao);

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget;

                UpdateAnimator(distance);
                pathfinder.speed = 3.5f;

                if (!dead)
                {
                    pathfinder.stoppingDistance = 4;
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
        animationEnemy.SetBool("Firing", false);
        UpdateAnimator(20);

        // while (target != null && distance > 12.3)
        // {
        //     Debug.Log("Ta no idle com a distancia: " + distance);
        //     currentState = State.Idle;
        //     if (currentState == State.Idle)
        //     {
        //         pathfinder.enabled = false;
        //         // animationEnemy.SetBool("Moviment", false);
        //         UpdateAnimator(distance);
        //         animationEnemy.SetBool("Firing", false);
        //     }

        //     yield return new WaitForSeconds(refreshRate);
        // }

    }

    // protected virtual void Move(float move)
    // {
    //     // if (move.magnitude > 0.3)
    //     // {
    //     //     move.Normalize();
    //     // }

    //     this.moveInput = move;

    //     ConvertMoveInput();
    //     UpdateAnimator();
    // }

    // protected virtual void ConvertMoveInput()
    // {
    //     Vector3 localMove = transform.InverseTransformDirection(moveInput);
    //     // turnAmount = localMove.x;

    //     forwardAmount = localMove.z;
    // }

    protected virtual void UpdateAnimator(float move)
    {
        // Debug.Log(forwardAmount);
        animationEnemy.SetFloat("Distance", move);
        // animationEnemy.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    protected virtual void SetupAnimator()
    {
        animationEnemy = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != animationEnemy)
            {
                animationEnemy.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }
}
