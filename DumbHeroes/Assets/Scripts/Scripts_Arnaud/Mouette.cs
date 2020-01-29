using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouette : MonoBehaviour
{
    [SerializeField]GameObject prefabObjectSpawn;
    [SerializeField] Transform carryPoint;
    public float speed;
    Rigidbody2D rbd;
    Rigidbody2D carry;
    float dropTimer;

    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        carry = Instantiate(prefabObjectSpawn,carryPoint.position,Quaternion.identity,carryPoint).GetComponent<Rigidbody2D>();
        carry.isKinematic = true;
        carry.GetComponent<Collider2D>().enabled = false;
        dropTimer = Random.Range(3.5f, 4.5f);
    }

    private void Update()
    {
        if(carry != null )
        {
            if (dropTimer >= 0)
            {
                dropTimer -= Time.deltaTime;
            }
            else
            {
                Drop();
            }
        }
    }

    private void FixedUpdate()
    {
        if (rbd.gravityScale == 0)
        {
            rbd.velocity = new Vector2(-speed, 0);
        }
    }

    public void Drop()
    {
        carry.transform.parent = null;
        carry.isKinematic = false;
        carry.velocity = rbd.velocity;
        carry.GetComponent<Collider2D>().enabled = true;
        carry = null;
    }

    public void Hit()
    {
        GetComponent<Collider2D>().enabled = false;
        rbd.gravityScale = 1;
        carry = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CarryAble")
        {
            Hit();
        }
    }
}
