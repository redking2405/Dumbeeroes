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
    bool v_ReInitialised;
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
        if (v_IsActivated && !trigger)
        {
            ActivateObjects();
            trigger = true;
        }

        else if(!v_IsActivated && !trigger)
        {
            DeactivateObjects();
            trigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GrabAble" || collision.gameObject.tag == "Player")
        {
            if (v_ReInitialised)
            {
                v_IsActivated = false;
                v_IsTriggered = false;
                renderer.sprite = v_SpriteUnpressed;
                trigger = false;

            }
            else
            {
                v_IsActivated = true;
                v_IsTriggered = true;
                renderer.sprite = v_SpritePressed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble" || collision.gameObject.tag == "Player")
        {
            v_ReInitialised = true;
            trigger = false;
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
