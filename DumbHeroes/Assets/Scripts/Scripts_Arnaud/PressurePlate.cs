using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interrupteur
{

    //[SerializeField] LayerMask v_Mask;
    [SerializeField] int v_WeightNeeded; //in number of objects
    
    List<GameObject> v_ObjectOnPlate = new List<GameObject>();
    [SerializeField]float v_WeightOnPlate;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected new void Update()
    {

        if (v_WeightOnPlate < 0) //empecher les mass négatives qui peuvent foutre la merde
        {
            v_WeightOnPlate = 0;
        }

        if (v_WeightOnPlate > 0)
        {
            renderer.sprite = v_SpritePressed;
            
        }

        else
        {
            v_IsTriggered = false;
            renderer.sprite = v_SpriteUnpressed;
        }

        if (v_WeightOnPlate >= v_WeightNeeded && !v_IsTriggered)
        {
            v_IsTriggered = true;
            ActivateObjects();
            
        }

        else if(v_WeightOnPlate<v_WeightNeeded && !trigger)
        {
            DeactivateObjects();
            trigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "GrabAble" || collision.gameObject.tag=="CarryAble"))
        {
            
            v_WeightOnPlate += collision.attachedRigidbody.mass;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "GrabAble" || collision.gameObject.tag == "CarryAble"))
        {
           
            v_WeightOnPlate -= collision.attachedRigidbody.mass;
            

        }
    }

   

    
}
