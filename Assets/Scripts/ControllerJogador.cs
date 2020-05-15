using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class ControllerJogador : MonoBehaviour
{
    /* Velocidade dos personagens */
    public float Velocidade = 2;
    Vector3 direcao;

    public LayerMask MascaraChao;

    public GameObject TextoGameOver;

    // Update is called once per frame
    void Update()
    {
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ);

        /* Muda a animação entre Idle e Correndo */
        if (direcao != Vector3.zero)
        {
            GetComponent<Animator>().SetBool("Movendo", true);
        } 
        else
        {
            GetComponent<Animator>().SetBool("Movendo", false);
        }
    }

    /* FixedUpdate roda em um tempo fixo e nao em todo frame do jogo como o Update*/
    void FixedUpdate()
    {
        /* Pegando o Rigidbody e movendo a posição que o Rigidbody já está + a posição que eu quero ir */
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + (direcao * Velocidade * Time.deltaTime));

        /* Cria um raio que segue aonde o mouse ta */
        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(raio.origin, raio.direction * 100, Color.red);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        /* Guarda aonde o raio toca */
        // RaycastHit impacto;
        float impacto;

        // if(Physics.Raycast(raio, out impacto, 100, MascaraChao))
        // {
        //     Vector3 posicaoMiraJogador = impacto.point - transform.position;
        //     // Vector3 posicaoMiraJogador = raio.GetPoint(impacto);

        //     // posicaoMiraJogador.z = transform.position.z;
        //     posicaoMiraJogador.y = transform.position.y;
        //     // posicaoMiraJogador.x = transform.position.x;

        //     Quaternion novaRotacao = Quaternion.LookRotation(posicaoMiraJogador);

        //     GetComponent<Rigidbody>().MoveRotation(novaRotacao);

        // }
        if(groundPlane.Raycast(raio, out impacto))
        {
            Vector3 posicaoMiraJogador = raio.GetPoint(impacto);
            Debug.DrawLine(raio.origin, posicaoMiraJogador, Color.red);

            transform.LookAt(new Vector3(posicaoMiraJogador.x, transform.position.y, posicaoMiraJogador.z));
        }
    }
}
