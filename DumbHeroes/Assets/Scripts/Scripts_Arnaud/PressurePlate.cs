using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interrupteur
{

    //[SerializeField] LayerMask v_Mask;
    [SerializeField] int v_WeightNeeded; //in number of objects
    
    List<GameObject> v_ObjectOnPlate = new List<GameObject>();
    float v_WeightOnPlate;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected new void Update()
    {



        if (v_WeightOnPlate > 0)
        {
            renderer.sprite = v_SpritePressed;
            v_IsTriggered = true;
        }

        else
        {
            v_IsTriggered = false;
            renderer.sprite = v_SpriteUnpressed;
        }

        if (v_WeightOnPlate >= v_WeightNeeded && !trigger)
        {
            ActivateObjects();
            trigger = true;
        }

        else if(v_WeightOnPlate<v_WeightNeeded && !trigger)
        {
            DeactivateObjects();
            trigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble" && !v_IsActivated)
        {
            v_IsActivated = true;
            v_WeightOnPlate += collision.attachedRigidbody.mass;
            StartCoroutine(WaitForDeflag());
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble" && !v_IsActivated)
        {
            v_IsActivated = true;
            v_WeightOnPlate -= collision.attachedRigidbody.mass;
            StartCoroutine(WaitForDeflag());

        }
    }

    IEnumerator WaitForDeflag()
    {
        yield return new WaitForSeconds(1.5f);
        v_IsActivated = false;
        trigger = false;
    }

    
}
