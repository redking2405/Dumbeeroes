using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupteur : MonoBehaviour
{
    [SerializeField] protected Sprite v_SpriteUnpressed;
    [SerializeField] protected Sprite v_SpritePressed;
    protected SpriteRenderer renderer;
    [SerializeField] protected ActivableObjects[] v_ObjectToActivate;
    protected bool v_IsActivated;
    bool v_ReInitialised = false;
    public bool v_IsTriggered = false;
    protected bool trigger = false;
    protected void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GrabAble" || collision.gameObject.tag == "CarrryAble")
        {
            if (v_ReInitialised)
            {
                v_IsActivated = false;
                v_IsTriggered = false;
                renderer.sprite = v_SpriteUnpressed;
                DeactivateObjects();


            }
            else
            {
                v_IsActivated = true;
                v_IsTriggered = true;
                renderer.sprite = v_SpritePressed;
                ActivateObjects();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble" || collision.gameObject.tag == "CarrryAble")
        {
            
            v_ReInitialised = !v_ReInitialised;
            //trigger = false;
        }
    }

    public void ActivateObjects()
    {
        for (int i = 0; i < v_ObjectToActivate.Length; i++)
        {
            v_ObjectToActivate[i].Activate();
        }
    }

    public void DeactivateObjects()
    {
        for (int i = 0; i < v_ObjectToActivate.Length; i++)
        {
            v_ObjectToActivate[i].Deactivate();
        }
    }
}
