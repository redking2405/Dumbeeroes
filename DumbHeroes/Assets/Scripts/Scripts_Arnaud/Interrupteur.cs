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
    protected bool v_IsTriggered;
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
        if (v_IsActivated)
        {
            ActivateObjects();
        }

        else DeactivateObjects();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GrabAble")
        {
            if (v_ReInitialised)
            {
                v_IsActivated = false;
                renderer.sprite = v_SpriteUnpressed;

            }
            else
            {
                v_IsActivated = true;
                renderer.sprite = v_SpritePressed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble")
        {
            v_ReInitialised = true;
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
