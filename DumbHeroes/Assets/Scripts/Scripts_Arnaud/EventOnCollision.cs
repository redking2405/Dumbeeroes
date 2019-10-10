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
        Debug.Log("Pan dans ta face");
        if (collision.rigidbody.velocity.magnitude > 0) ;
        {
            Debug.Log("Tu fais mal méchant");
            v_Event.Invoke();
        }
       
    }
}
