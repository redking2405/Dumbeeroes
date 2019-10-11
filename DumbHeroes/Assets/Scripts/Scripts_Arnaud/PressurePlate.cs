using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    //[SerializeField] LayerMask v_Mask;
    [SerializeField] int v_WeightNeeded; //in number of objects
    [SerializeField] Sprite v_SpriteUnpressed;
    [SerializeField] Sprite v_SpritePressed;
    SpriteRenderer renderer;
    List<GameObject> v_ObjectOnPlate = new List<GameObject>();
    float v_WeightOnPlate;
    [SerializeField] ActivableObjects[] v_ObjectToActivate;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        if (v_WeightOnPlate > 0)
        {
            renderer.sprite = v_SpritePressed;
        }

        else renderer.sprite = v_SpriteUnpressed; 


        if (v_WeightOnPlate >= v_WeightNeeded)
        {
            ActivateObjects();
        }

        else
        {
            DeactivateObjects();
           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble")
        {
            v_WeightOnPlate += collision.attachedRigidbody.mass;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble")
        {
            v_WeightOnPlate -= collision.attachedRigidbody.mass;
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
