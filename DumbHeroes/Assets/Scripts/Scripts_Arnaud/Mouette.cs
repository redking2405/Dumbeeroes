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
    float timer;
    float dropTimer;
    public float bomberTimer;
    public float bombForce;
    public float bombRange;
    bool bomber = false;
    bool isBomber = false;

    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        carry = Instantiate(prefabObjectSpawn[Random.Range(0,prefabObjectSpawn.Count)],carryPoint.position,Quaternion.identity,carryPoint).GetComponent<Rigidbody2D>();
        carry.isKinematic = true;
        carry.GetComponent<Collider2D>().enabled = false;
        dropTimer = Random.Range(1.5f, 3.5f);
        SFXManager.Instance.BoatLevel[3].Play();
        if(Random.Range(0f,1f) > 0.5f)
        {
            isBomber = true;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (carry != null )
        {
            if (timer >= dropTimer)
            {
                Drop();
            }
        }
        if(!bomber && isBomber)
        {
            if(timer >= bomberTimer)
            {
                bomber = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (rbd.gravityScale == 0)
        {
            if (!bomber)
            {
                rbd.velocity = new Vector2(-speed, 0);
            }
            else
            {
                rbd.velocity = (new Vector2(-2, -7.5f) - (Vector2)transform.position).normalized * speed*3;
            }
        }
    }

    public void Drop()
    {
        if (carry != null)
        {
            carry.transform.parent = null;
            carry.isKinematic = false;
            carry.velocity = rbd.velocity;
            carry.GetComponent<Collider2D>().enabled = true;
            carry = null;
        }
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

        if (!bomber && (collision.gameObject.tag == "CarryAble" || collision.gameObject.tag == "Player"))
        {
            Hit();
            SFXManager.Instance.BoatLevel[4].Play();
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRange);
            foreach (Collider2D hit in colliders)
            {
                if (hit.gameObject.tag == "CarryAble" || hit.gameObject.tag == "Player")
                {
                    if (hit.gameObject.tag == "Player")
                    {
                        hit.gameObject.GetComponent<PlayerController>().DropObject();
                    }
                    Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                    if (rb != null)
                        rb.velocity = rb.transform.position - transform.position * bombForce;
                }
            }
            ParticleSystem p = GetComponentInChildren<ParticleSystem>();
            p.Play();
            Hit();
        }
    }
}
