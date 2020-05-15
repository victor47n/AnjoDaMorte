using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaJogador : MonoBehaviour
{
    Rigidbody rigidBody;
    public float Velocidade = 1;

    Vector3 olharNaPosicao;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit impacto;

        if (Physics.Raycast(raio, out impacto, 100))
        {
            olharNaPosicao = impacto.point;
        }

        Vector3 olharNaDirecao = olharNaPosicao - transform.position;
        olharNaDirecao.y = 0;

        transform.LookAt(transform.position + olharNaDirecao, Vector3.up);
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0, vertical);

        rigidBody.AddForce(movimento * Velocidade / Time.deltaTime);
    }
}
