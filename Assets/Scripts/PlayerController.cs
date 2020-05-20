using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInputs))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    public float playerSpeed = 15f;
    public float playerRotationSpeed = 20f;

    [Header("Turn Player Properties")]
    public Transform turnPlayerTransform;
    public float turnPlayerLagSpeed = 0.5f;

    [Header("Reticle Properties")]
    public Transform reticleTransform;

    private Rigidbody rigidBody;
    private PlayerInputs input;
    private Vector3 finalTurnPlayerLookDir;

    /* Gun variable */
    GunController gunController;

    /* Animation Variables */
    private Animator animationPlayer;
    private Vector3 move;
    private Vector3 moveInput;
    private float forwardAmount;
    private float turnAmount;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputs>();
        gunController = GetComponent<GunController>();
        SetupAnimator();
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
        // Vector3 wantedPosition = transform.position + (transform.forward * input.FowardInput * playerSpeed * Time.deltaTime);
        // rigidBody.MovePosition(wantedPosition);

        // Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (input.RotationInput * playerRotationSpeed * Time.deltaTime));
        // rigidBody.MoveRotation(wantedRotation);

        Vector3 wantedPosition = new Vector3(transform.forward.x, 0, transform.forward.z) * input.FowardInput;
        Vector3 wantedRotation = new Vector3(transform.right.x, 0, transform.right.z) * input.RotationInput;

        Vector3 finalDirection = wantedPosition + wantedRotation;

        if (finalDirection.magnitude > 1)
        {
            finalDirection.Normalize();
        }

        move = finalDirection;
        Move(move);

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
            Debug.Log(finalTurnPlayerLookDir);
            turnPlayerTransform.rotation = Quaternion.LookRotation(finalTurnPlayerLookDir);
        }
    }

    protected virtual void HandleReticle()
    {
        if (reticleTransform)
        {
            reticleTransform.position = input.ReticlePosition;
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
}
