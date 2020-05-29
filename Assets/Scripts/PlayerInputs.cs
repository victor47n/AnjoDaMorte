using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("Input Properties")]
    public Camera myCamera;
    public LayerMask terrain;

    private Vector3 reticlePosition;
    public Vector3 ReticlePosition
    {
        get { return reticlePosition; }
    }

    private Vector3 reticleNormal;
    public Vector3 ReticleNormal
    {
        get { return reticleNormal; }
    }

    private float forwardInput;
    public float FowardInput
    {
        get { return forwardInput; }
    }

    private float rotationInput;
    public float RotationInput
    {
        get { return rotationInput; }
    }

    private Ray screenRay;
    public Ray ScreenRay
    {
        get { return screenRay; }
    }

    void Update()
    {
        if (myCamera)
        {
            HandleInputs();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(reticlePosition, 0.1f);
    }

    protected virtual void HandleInputs()
    {
        screenRay = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(screenRay, out hit, terrain))
        {
            reticlePosition = hit.point;
            reticleNormal = hit.normal;
        }

        forwardInput = Input.GetAxis("Vertical");
        rotationInput = Input.GetAxis("Horizontal");
    }
}
