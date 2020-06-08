using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("Weapon firing properties")]
    public List<GameObject> shootingPoint = new List<GameObject>();
    public List<GameObject> vfx = new List<GameObject>();
    private GameObject effectToSpawn;
    private float timeToFire = 0;
    public float firingSpeed;

    
    public enum FireMode { Auto, Burst, Single }
    public FireMode fireMode;
    public int burstCount;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;

    Light dropLight;

    [Header("SFX")]
    public AudioClip ShootSound;

    void Start()
    {
        effectToSpawn = vfx[0];
        shotsRemainingInBurst = burstCount;
    }

    void Awake()
    {
        dropLight = GetComponent<Light>();
        dropLight.enabled = false;
    }

    public void ActiveLight()
    {
        dropLight.enabled = true;
    }
    void Shoot()
    {
        GameObject vfx;

        if (timeToFire + firingSpeed <= Time.time)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < shootingPoint.Count; i++)
            {
                timeToFire = Time.time;

                if (shootingPoint != null)
                {
                    /* Posiciona as balas na altura certa */
                    vfx = Instantiate(effectToSpawn, shootingPoint[i].transform.position, shootingPoint[i].transform.rotation);
                    // if (fireMode.Equals("Auto") || fireMode.Equals("Single"))
                    // {
                    //     FindObjectOfType<AudioManager>().Play("Shooting");
                    // }
                    // if (fireMode.Equals("Burst"))
                    // {
                    //     FindObjectOfType<AudioManager>().Play("ShootingShotgun");
                    // }
                    
                    // AudioController.instance.volume = 0.3f;
                    
                    AudioController.instance.PlayOneShot(ShootSound);
                }
                else
                {
                    Debug.Log("No fire point");
                }
            }
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        // AudioController.instance.PlayOneShot(ShootSound);
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }

    private void OnTriggerStay(Collider co)
    {
        if (co.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                co.GetComponent<PlayerController>().PickUp(gameObject.GetComponent<Gun>());
                Destroy(gameObject);
            }
        }
    }

    public void DestroyGun(Gun gun)
    {
        Destroy(gun.gameObject);
    }

    public void Teste(Gun seila)
    {
        seila.GetComponent<Animation>().Play("WeaponDropped");
    }
}