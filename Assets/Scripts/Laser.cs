using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Color corLaser = Color.red;
    public int DistanciaDoLaser = 100;
    public float LarguraInicial = 0.02f, LarguraFinal = 0.1f;
    private GameObject luzColisao;

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        luzColisao = new GameObject();
        luzColisao.AddComponent<Light>();
        luzColisao.GetComponent<Light>().intensity = 8;
        luzColisao.GetComponent<Light>().bounceIntensity = 8;
        luzColisao.GetComponent<Light>().range = LarguraFinal * 2;
        luzColisao.GetComponent<Light>().color = corLaser;
        //
        // LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        // lineRenderer.SetColors(corLaser, corLaser);
        // lineRenderer.SetWidth(LarguraInicial, LarguraFinal);
        lineRenderer.startWidth = LarguraInicial;
        lineRenderer.endWidth = LarguraFinal;
        lineRenderer.startColor = corLaser;
        lineRenderer.endColor = corLaser;
        lineRenderer.positionCount = 2;

    }
    void Update()
    {
        Vector3 PontoFinalDoLaser = transform.position + transform.forward * DistanciaDoLaser;
        RaycastHit PontoDeColisao;
        if (Physics.Raycast(transform.position, transform.forward, out PontoDeColisao, DistanciaDoLaser))
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, PontoDeColisao.point);
            float distancia = Vector3.Distance(transform.position, PontoDeColisao.point) - 0.03f;
            luzColisao.transform.position = transform.position + transform.forward * distancia;
        }
        else
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, PontoFinalDoLaser);
            luzColisao.transform.position = PontoFinalDoLaser;
        }
    }
}
