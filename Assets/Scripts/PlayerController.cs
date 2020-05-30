using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : LivingEntity
{
    [Header("Movement Properties")]
    public float playerSpeed = 15f;
    public float playerRotationSpeed = 20f;

    [Header("Turn Player Properties")]
    public Transform turnPlayerTransform;
    public float turnPlayerLagSpeed = 0.5f;

    [Header("Reticle Properties")]
    public Crosshair reticleTransform;

    [Header("Particle Blood")]
    public GameObject VFXBloodParticle;

    private Rigidbody rigidBody;
    private PlayerInputs input;
    private Vector3 finalTurnPlayerLookDir;

    /* Gun variable */
    GunController gunController;
    string weapon;

    /* Animation Variables */
    private Animator animationPlayer;
    private Vector3 move;
    private Vector3 moveInput;
    private float forwardAmount;
    private float turnAmount;

    protected override void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputs>();
        gunController = GetComponent<GunController>();
        SetupAnimator();

        weapon = gunController.equippedGun.ToString();

        if (weapon == "MachineGun(Clone) (Gun)" || weapon == "Shotgun(Clone) (Gun)")
        {
            animationPlayer.SetBool("Rifle", true);
            animationPlayer.SetBool("Pistol", false);
        }
        else{
            animationPlayer.SetBool("Pistol", true);
            animationPlayer.SetBool("Rifle", false);
        }
    }

    void Update()
    {
        // Weapon input
        if (Input.GetButton("Fire1"))
        {
            gunController.OnTriggerHold();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            gunController.OnTriggerRelease();
        }
    }

    void FixedUpdate()
    {
        if (rigidBody && input)
        {
            HandleMovement();
            HandleTurnPlayer();
            HandleReticle();
            
        }
    }

    protected virtual void HandleMovement()
    {
        Vector3 wantedPosition = new Vector3(transform.forward.x, 0, transform.forward.z) * input.FowardInput;
        Vector3 wantedRotation = new Vector3(transform.right.x, 0, transform.right.z) * input.RotationInput;

        Vector3 finalDirection = wantedPosition + wantedRotation;

        if (finalDirection.magnitude > 1)
        {
            finalDirection.Normalize();
        }

        /* Animation Player */
        move = finalDirection;
        Move(move);

        /* Move Player */
        Vector3 moviment = finalDirection * playerSpeed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + moviment);
    }

    protected virtual void HandleTurnPlayer()
    {
        if (turnPlayerTransform)
        {
            Vector3 turnPlayerLookDir = input.ReticlePosition - turnPlayerTransform.position;
            turnPlayerLookDir.y = 0f;

            finalTurnPlayerLookDir = Vector3.Lerp(finalTurnPlayerLookDir, turnPlayerLookDir, Time.deltaTime * turnPlayerLagSpeed);
            turnPlayerTransform.rotation = Quaternion.LookRotation(finalTurnPlayerLookDir);
        }
    }

    protected virtual void HandleReticle()
    {
        if (reticleTransform)
        {
            reticleTransform.transform.position = input.ReticlePosition;
            reticleTransform.DetectTargets(input.ScreenRay);
        }
    }

    protected virtual void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this.moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    protected virtual void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;

        forwardAmount = localMove.z;
    }
    protected virtual void UpdateAnimator()
    {
        animationPlayer.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animationPlayer.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    protected virtual void SetupAnimator()
    {
        animationPlayer = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != animationPlayer)
            {
                animationPlayer.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    public override void TakeHit(float damage, Collision hit)
    {
        if (damage >= health)
        {
            Dead();
        }
        base.TakeHit(damage, hit);
    }

    void Dead()
    {
        animationPlayer.SetTrigger("Death");
        this.enabled = false;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
    }

    public override void BloodParticle(Vector3 pos, Quaternion rot)
    {
        Instantiate(VFXBloodParticle, pos, rot);
    }
}
