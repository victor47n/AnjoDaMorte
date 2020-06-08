using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : LivingEntity, IPickUp
{
    [Header("Movement Properties")]
    public float playerSpeed = 15f;
    public float playerRotationSpeed = 20f;
    private PlayerController playerController;

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
    RaycastHit hit;
    Ray ray;

    [Header("SFX")]
    public AudioClip DamageSound;

    protected override void Start()
    {
        base.Start();
        
        rigidBody = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputs>();
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        SetupAnimator();

        weapon = gunController.equippedGun.ToString();

        if (weapon == "MachineGun(Clone) (Gun)" || weapon == "Shotgun(Clone) (Gun)")
        {
            animationPlayer.SetBool("Rifle", true);
            animationPlayer.SetBool("Pistol", false);
            playerController.playerSpeed = 5;
        }
        else
        {
            Debug.Log("entrou aqui");
            animationPlayer.SetBool("Pistol", true);
            animationPlayer.SetBool("Rifle", false);
            playerController.playerSpeed = 5;
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            Instantiate(gunController.equippedGun, new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
            gunController.equippedGun.DestroyGun(gunController.equippedGun);
            animationPlayer.SetBool("Rifle", false);
            animationPlayer.SetBool("Pistol", false);
            playerController.playerSpeed = 6;
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

    public override void TakeDamage(float damage)
    {
        if (damage >= health)
        {
            AudioController.instance.PlayOneShot(DamageSound);
            // FindObjectOfType<AudioManager>().Play("PlayerDeath");
            Dead();
        }
        base.TakeDamage(damage);
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

    public void PickUp(Gun guntoEquip)
    {
        guntoEquip.name = guntoEquip.name.Replace("(Clone)", "");
        animationPlayer.SetTrigger("Aiming");

        if (guntoEquip.ToString() == "MachineGun (Gun)" || guntoEquip.ToString() == "Shotgun (Gun)")
        {
            animationPlayer.SetBool("Pistol", false);
            animationPlayer.SetBool("Rifle", true);
            playerController.playerSpeed = 5;
        }
        else
        {
            animationPlayer.SetBool("Pistol", true);
            animationPlayer.SetBool("Rifle", false);
            playerController.playerSpeed = 5;
        }
        gunController.EquipGun(guntoEquip);
    }
}
