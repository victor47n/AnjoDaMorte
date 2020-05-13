using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class ControllerInimigo : MonoBehaviour
{
    public GameObject jogador;
    public float velocidade = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        float distancia = Vector3.Distance(transform.position, jogador.transform.position);

        /* Inimigo só se move se estiver a uma certa distancia do jogador */
        if(distancia > 3)
        {
            GetComponent<Animator>().SetBool("Movendo", true);
            /* Inimigo se move na direção do jogador */
            Vector3 direcao = jogador.transform.position - transform.position;
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + direcao.normalized * velocidade * Time.deltaTime);

            /* Inimigo rotaciona para o jogador */
            Quaternion novaRotacao = Quaternion.LookRotation(direcao);
            GetComponent<Rigidbody>().MoveRotation(novaRotacao);
        }
        else
        {
            GetComponent<Animator>().SetBool("Movendo", false);
        }
    }
}
