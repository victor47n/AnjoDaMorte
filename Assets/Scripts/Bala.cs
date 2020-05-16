using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public float Velocidade = 10;
    // public GameObject Jogador;

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Velocidade * Time.deltaTime);
    }

    /* Quando a bala entrar em colisao com o inimigo a bala sera destruida */
    void OnTriggerEnter(Collider objetoDeColisao)
    {
        /* Só destroi o objeto que tem a tag inimigo */
        if(objetoDeColisao.tag == "Inimigo")
        {
            Destroy(objetoDeColisao.gameObject);
        }
        
        if(objetoDeColisao.tag == "Player")
        {
            Time.timeScale = 0;
            // Jogador.GetComponent<ControllerJogador>().TextoGameOver.SetActive(true);
        }

        Destroy(gameObject);

    }
}
