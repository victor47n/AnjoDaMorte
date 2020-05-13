using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class ControllerJogador : MonoBehaviour
{
    /* Velocidade dos personagens */
    public float velocidade = 2;
    Vector3 direcao;

    // Update is called once per frame
    void Update()
    {
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ);

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
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + (direcao * velocidade * Time.deltaTime));
    }
}
