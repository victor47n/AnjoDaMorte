using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public List<GameObject> shootingPoint = new List<GameObject>();
    public List<GameObject> vfx = new List<GameObject>();
    private GameObject effectToSpawn;
    private float timeToFire = 0;
    public float firingSpeed;

    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;
    public int burstCount;

    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;


    void Start()
    {
        effectToSpawn = vfx[0];
        shotsRemainingInBurst = burstCount;
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
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }

    public void Test(Collider co)
    {
        if (co.tag == "Player")
        {
            co.GetComponent<PlayerController>().PickUp(gameObject.GetComponent<Gun>());
            Destroy(gameObject);
        }
    }
}
