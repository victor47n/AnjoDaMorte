using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{

    public float speed;
    public float fireRate;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    float damage = 1;

    private Vector3 firingPoint;
    [SerializeField]
    private float maxProjectileDistance = 0;

    void Start()
    {
        firingPoint = transform.position;

        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();

            if (psMuzzle != null)
            {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    void Update()
    {
        if (Vector3.Distance(firingPoint, transform.position) > maxProjectileDistance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (speed != 0)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                // transform.position += transform.forward * (speed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Sem velocidade");
            }
        }
    }

    void OnCollisionEnter(Collision co)
    {
        speed = 0;

        ContactPoint contact = co.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (hitPrefab != null)
        {
            var hitVFX = Instantiate(hitPrefab, pos, rot);

            var psHit = hitVFX.GetComponent<ParticleSystem>();

            if (psHit != null)
            {
                IDamageable damageableObject = co.collider.GetComponent<IDamageable>();
                if (damageableObject != null)
                {
                    damageableObject.TakeHit(damage, co);
                }
                Destroy(hitVFX, psHit.main.duration);
            }
            else
            {
                IDamageable damageableObject = co.collider.GetComponent<IDamageable>();
                if (damageableObject != null)
                {
                    damageableObject.TakeHit(damage, co);
                }

                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, psChild.main.duration);
            }
        }

        Destroy(gameObject);
    }
}
