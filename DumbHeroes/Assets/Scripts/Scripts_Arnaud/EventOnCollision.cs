using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EventOnCollision : MonoBehaviour
{
    public UnityEvent v_Event;
    public float v_Velocity;
    


   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x >= v_Velocity && collision.gameObject.GetComponent<Rigidbody2D>().velocity.y != 0)
        {
            v_Event.Invoke();
        }
       
    }
}
