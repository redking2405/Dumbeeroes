using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    //[SerializeField] LayerMask v_Mask;
    [SerializeField] int v_WeightNeeded; //in number of objects
    List<GameObject> v_ObjectOnPlate = new List<GameObject>();
    [SerializeField] ActivableObjects[] v_ObjectToActivate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (v_ObjectOnPlate.Count >= v_WeightNeeded)
        {
            ActivateObjects();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble")
        {
            v_ObjectOnPlate.Add(collision.gameObject);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble")
        {
            v_ObjectOnPlate.Remove(collision.gameObject);
        }
    }


    public void ActivateObjects()
    {
        for(int i=0; i<v_ObjectToActivate.Length; i++)
        {
            v_ObjectToActivate[i].Activate();
        }
    }

    public void DeactivateObjects()
    {
        for(int i=0; i<v_ObjectToActivate.Length; i++)
        {
            v_ObjectToActivate[i].Deactivate();
        }
    }
}
