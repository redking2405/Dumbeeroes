using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_Ragdoll : MonoBehaviour
{
    public GameObject[] v_gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Ragdoll()
    {
        for(int i=0; i<v_gameObjects.Length; i++)
        {
            v_gameObjects[i].GetComponent<HingeJoint2D>().enabled = true;
            v_gameObjects[i].GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
}
