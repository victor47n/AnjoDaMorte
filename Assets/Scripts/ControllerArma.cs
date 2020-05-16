using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerArma : MonoBehaviour
{
    public GameObject Bala;
    public GameObject CanoDaArma;
    public List<GameObject> vfx = new List<GameObject>();
    // public RotateToMouse rotateToMouse;

    private GameObject effectToSpawn;
    public float timeToFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        /* Fazendo o jogador atirar */
        if(Input.GetButtonDown("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<MoveProjetil>().fireRate;
            /* Chama a função de criar as balas */
            SpawnVFX();
        }
    }

    void SpawnVFX()
    {
        GameObject vfx;

        if (CanoDaArma != null)
        {
            /* Posiciona as balas na altura certa */
            vfx = Instantiate(effectToSpawn, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
        }
        else
        {
            Debug.Log("No fire point");
        }
    }
}
