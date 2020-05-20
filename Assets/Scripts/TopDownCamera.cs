using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;

    public float height = 10f;
    public float distance = 20f;

    [SerializeField]
    private float angle = 45f;

    [SerializeField]
    private float smoothSpeed = 0.5f;

    private Vector3 refVelocity;

    void Start()
    {
        HandleCamera();
    }

    void Update()
    {
        HandleCamera();
    }

    protected virtual void HandleCamera()
    {
        if (!target)
        {
            return;
        }

        /* Build World position vector */
        Vector3 worldPosition = (Vector3.forward * distance) + (Vector3.up * height);
        // Debug.DrawLine(target.position, worldPosition, Color.red);

        /* Build our Rotated vector */
        Vector3 rotated = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        // Debug.DrawLine(target.position, rotated, Color.green);

        /* Move our position */
        Vector3 flatTargetPosition = target.position;
        flatTargetPosition.y = 0f;
        Vector3 finalPosition = flatTargetPosition + rotated;
        // Debug.DrawLine(target.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(flatTargetPosition);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color (0f, 1f, 0f, 0.2f);
        if (target)
        {
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawSphere(target.position, 0.1f);
        }
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
