using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class ControllerInimigo : MonoBehaviour
{
    public GameObject Jogador;
    public float Velocidade = 1;

    public GameObject Bala;
    public GameObject CanoDaArma;

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
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);
        Vector3 direcao = Jogador.transform.position - transform.position;


        /* Inimigo rotaciona para o jogador */
        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        GetComponent<Rigidbody>().MoveRotation(novaRotacao);

        /* Inimigo só se move se estiver a uma certa distancia do jogador */
        if (distancia > 3 && distancia < 10)
        {
            GetComponent<Animator>().SetBool("Movendo", true);
            /* Inimigo se move na direção do jogador */

            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + direcao.normalized * Velocidade * Time.deltaTime);
            /* Posiciona as balas na altura certa */
            // Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);

        }
        else
        {
            GetComponent<Animator>().SetBool("Movendo", false);
            /* Posiciona as balas na altura certa */
            // Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        }
    }
}
