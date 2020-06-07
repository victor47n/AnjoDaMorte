using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarPosition : MonoBehaviour
{
    public float distY;
    public float distZ;
    public Transform owner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = Camera.main.transform.eulerAngles;
        if(owner != null){
            Vector3 pos = new Vector3(owner.position.x, owner.position.y + distY, owner.position.z + distZ);
            transform.position = pos;
        }
    }
}
