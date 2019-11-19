using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanon : MonoBehaviour
{
    Canon v_MyCanon;
    Rigidbody2D v_Target;

    // Start is called before the first frame update

    private void Awake()
    {
        v_MyCanon = GetComponentInParent<Canon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GrabAble")
        {
            v_MyCanon.v_Loaded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GrabAble")
        {
            v_MyCanon.v_Loaded = false;
        }
    }
}
