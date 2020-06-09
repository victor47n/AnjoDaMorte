using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : LivingEntity
{
    public enum State { Idle, Chasing, Attacking }
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    float distance;
    private Vector3 finalTurnEnemyLookDir;
    Rigidbody enemyRigidbody;
    public GameObject VFXBloodParticle;

    float timeBetweenAttacks = 1;
    float damage = 1;
    Transform enemyShotgun;

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

    bool hasTarget;

    [Header("SFX")]
    public AudioClip DamageSound;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        gunController = GetComponent<GunController>();
        animationEnemy = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody>();

        if (GameObject.FindGameObjectWithTag("EnemyShotgun") != null)
        {
            enemyShotgun = GameObject.FindGameObjectWithTag("EnemyShotgun").transform;
        }
        SetupAnimator();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

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


            StartCoroutine(UpdatePath());
        }
    }

    public override void TakeHit(float damage, Collision hit)
    {
        if (damage >= health && !dead)
        {
            AudioController.instance.PlayOneShot(DamageSound);
            Dead();
            GenerateWeapon(gunController.equippedGun);
        }
        base.TakeHit(damage, hit);
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update()
    {
        if (hasTarget)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);

            bool enoughDistance = Time.time > nextAttackTime && distance < 8.05;

            if (enoughDistance == true)
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }

    public override void BloodParticle(Vector3 pos, Quaternion rot)
    {
        Instantiate(VFXBloodParticle, pos, rot);
    }

    void Dead()
    {
        animationEnemy.SetTrigger("Death");
        this.enabled = false;
        enemyRigidbody.constraints = RigidbodyConstraints.None;
        enemyRigidbody.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        pathfinder.enabled = false;
    }

    void GenerateWeapon(Gun dropWeapon)
    {
        dropWeapon.name = dropWeapon.name.Replace("(Clone)", "");
        Gun Weapon = Instantiate(dropWeapon, new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
        Weapon.ActiveLight();
    }

    IEnumerator Attack()
    {
        float attackSpeed = .5f;
        float percent = 0;
        bool hasAppliedDamage = false;

        if (enemyShotgun == null)
        {
            if (distance >= 6.05 && distance < 8.05)
            {
                if (hasTarget)
                {
                    currentState = State.Attacking;
                    pathfinder.enabled = true;

                    animationEnemy.SetBool("Firing", true);
                    UpdateAnimator(distance);

                    while (percent <= 1)
                    {
                        if (percent >= .5f && !hasAppliedDamage)
                        {
                            hasAppliedDamage = true;
                            targetEntity.TakeDamage(damage);
                        }
                        if (hasTarget)
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
                if (hasTarget)
                {
                    currentState = State.Attacking;
                    pathfinder.enabled = false;

                    animationEnemy.SetBool("Firing", true);
                    UpdateAnimator(distance);

                    while (percent <= 1)
                    {
                        if (percent >= .5f && !hasAppliedDamage)
                        {
                            hasAppliedDamage = true;
                            targetEntity.TakeDamage(damage);
                        }

                        if (hasTarget)
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
        }
        else
        {
            if (distance >= 4.05 && distance < 6.05)
            {
                if (hasTarget)
                {
                    currentState = State.Attacking;
                    pathfinder.enabled = true;

                    animationEnemy.SetBool("Firing", true);
                    UpdateAnimator(distance);

                    while (percent <= 1)
                    {
                        if (percent >= .5f && !hasAppliedDamage)
                        {
                            hasAppliedDamage = true;
                            targetEntity.TakeDamage(damage);
                        }

                        if (hasTarget)
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
            else if (distance < 4.05)
            {
                if (hasTarget)
                {
                    currentState = State.Attacking;
                    pathfinder.enabled = false;

                    animationEnemy.SetBool("Firing", true);
                    UpdateAnimator(distance);

                    while (percent <= 1)
                    {
                        if (percent >= .5f && !hasAppliedDamage)
                        {
                            hasAppliedDamage = true;
                            targetEntity.TakeDamage(damage);
                        }
                        
                        if (hasTarget)
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
        }

        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (enemyShotgun != null)
            {
                if (distance > 12.1)
                {
                    currentState = State.Idle;
                }
                else if (distance >= 6.06 && distance <= 12.1)
                {
                    currentState = State.Chasing;
                }
            }
            else
            {
                if (distance > 12.1)
                {
                    currentState = State.Idle;
                }
                else if (distance >= 8.06 && distance <= 12.1)
                {
                    currentState = State.Chasing;
                }
            }

            if (currentState == State.Idle)
            {
                if (!dead)
                {
                    pathfinder.enabled = false;
                    UpdateAnimator(distance);
                    animationEnemy.SetBool("Firing", false);
                }
            }

            if (currentState == State.Chasing)
            {
                pathfinder.enabled = true;

                Vector3 direcao = target.transform.position - transform.position;

                Quaternion novaRotacao = Quaternion.LookRotation(direcao);
                GetComponent<Rigidbody>().MoveRotation(novaRotacao);

                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget;

                UpdateAnimator(distance);

                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
        animationEnemy.SetBool("Firing", false);
        UpdateAnimator(20);

    }

    protected virtual void UpdateAnimator(float move)
    {
        animationEnemy.SetFloat("Distance", move);
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