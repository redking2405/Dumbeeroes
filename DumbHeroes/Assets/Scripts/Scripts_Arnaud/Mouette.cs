using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouette : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabObjectSpawn = new List<GameObject>();
    [SerializeField] Transform carryPoint;
    public float speed;
    Rigidbody2D rbd;
    Rigidbody2D carry;
    float dropTimer;

    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        carry = Instantiate(prefabObjectSpawn[Random.Range(0,prefabObjectSpawn.Count)],carryPoint.position,Quaternion.identity,carryPoint).GetComponent<Rigidbody2D>();
        carry.isKinematic = true;
        carry.GetComponent<Collider2D>().enabled = false;
        dropTimer = Random.Range(3.5f, 4.5f);
        SFXManager.Instance.BoatLevel[3].Play();
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
        Drop();
        GetComponent<Collider2D>().enabled = false;
        rbd.gravityScale = 1;
        carry = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CarryAble" || collision.gameObject.tag == "Player")
        {
            Hit();
            SFXManager.Instance.BoatLevel[4].Play();
        }
    }
}
