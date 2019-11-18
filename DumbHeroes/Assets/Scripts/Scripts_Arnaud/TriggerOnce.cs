using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnce : Interrupteur
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (v_IsActivated)
        {
            ActivateObjects();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GrabAble")
        {
            v_IsActivated = true;
        }
    }
}
