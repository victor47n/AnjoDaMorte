using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class ControllerJogador : MonoBehaviour
{
    Rigidbody rigidBody;
    Animator animacao;
    /* Velocidade dos personagens */
    public float Velocidade = 2;
    Vector3 olharNaPosicao;
    public LayerMask MascaraChao;
    public GameObject TextoGameOver;
    GunController gunController;

    Transform cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;

    float forwardAmount;
    float turnAmount;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        SetupAnimator();
        gunController = GetComponent<GunController>();

        cam = Camera.main.transform;
    }

    void Update()
    {
        HandleRotatationInput();

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

    /* FixedUpdate roda em um tempo fixo e nao em todo frame do jogo como o Update*/
    void FixedUpdate()
    {
        HandleMovementInput();
        // HandleShootInput();
    }

    void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (cam != null)
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
            move = vertical * camForward + horizontal * cam.right;
        }
        else
        {
            move = vertical * Vector3.forward + horizontal * Vector3.right;
        }

        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        Move(move);

        Vector3 movimento = new Vector3(horizontal, 0, vertical);
        movimento.Normalize();

        rigidBody.AddForce(movimento * Velocidade / Time.deltaTime);
        // transform.Translate(movimento * Velocidade / Time.deltaTime, Space.World);
    }

    void HandleRotatationInput()
    {
        /* Cria um raio que segue aonde o mouse ta */
        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(raio.origin, raio.direction, Color.red);

        /* Guarda aonde o raio toca */
        // RaycastHit impacto;
        RaycastHit impacto;

        if (Physics.Raycast(raio, out impacto, 100, MascaraChao))
        {
            olharNaPosicao = impacto.point;
        }

        Vector3 olharNaDirecao = olharNaPosicao - transform.position;
        olharNaDirecao.y = 0;

        transform.LookAt(transform.position + olharNaDirecao, Vector3.up);
    }

    // void HandleShootInput()
    // {
    //     if (Input.GetButton("Fire1"))
    //     {
    //         ControllerArma.Instance.OnTriggerHold();
    //     }

    //     if (Input.GetButtonUp("Fire1"))
    //     {
    //         ControllerArma.Instance.OnTriggerRelease();
    //     }
    // }

    void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this.moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;

        forwardAmount = localMove.z;
    }
    void UpdateAnimator()
    {
        animacao.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animacao.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    void SetupAnimator()
    {
        animacao = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != animacao)
            {
                animacao.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }
}
