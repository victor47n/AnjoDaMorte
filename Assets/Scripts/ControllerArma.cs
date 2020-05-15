using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerArma : MonoBehaviour
{
    public GameObject Bala;
    public GameObject CanoDaArma;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* Fazendo o jogador atirar */
        if(Input.GetButtonDown("Fire1"))
        {
            /* Posiciona as balas na altura certa */
            Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        }
    }
}
