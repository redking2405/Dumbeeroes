using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public int V_playerId;
    Player V_player;
    Rigidbody2D rb;

    [SerializeField]
    float V_moveSpeed;
    [SerializeField]
    float V_armSpeed;
    public Rigidbody2D O_Lhand,O_Rhand;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        V_player = ReInput.players.GetPlayer(V_playerId);
    }
    
    void Update()
    {
        Vector2 aimVector = new Vector2(V_player.GetAxis("ArmX"), V_player.GetAxis("ArmY"));
        AimArm(aimVector);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(V_player.GetAxis("MoveX")*V_moveSpeed, rb.velocity.y);
    }

    void AimArm(Vector2 direction)
    {
        if (direction.magnitude != 0)
        {
            O_Lhand.velocity = direction*V_armSpeed;
            O_Rhand.velocity = direction* V_armSpeed;
        }
        if (V_player.GetButtonDown("Grab"))
        {
            Collider2D[] hitsL = Physics2D.OverlapCircleAll(O_Lhand.transform.position, 0.4f);
            Collider2D[] hitsR = Physics2D.OverlapCircleAll(O_Rhand.transform.position, 0.4f);
            foreach (Collider2D hit in hitsL)
            {
                if(hit.tag == "GrabAble")
                {
                    hit.transform.parent = O_Lhand.transform;
                    hit.GetComponent<Rigidbody2D>().isKinematic = true;
                    break;
                }
            }
            foreach (Collider2D hit in hitsR)
            {
                if (hit.tag == "GrabAble")
                {
                    hit.transform.parent = O_Rhand.transform;
                    hit.GetComponent<Rigidbody2D>().isKinematic = true;
                    break;
                }
            }
        }
        if (V_player.GetButtonUp("Grab"))
        {
            foreach (Transform t in O_Lhand.transform)
            {
                t.parent = null;
                Rigidbody2D tRb = t.GetComponent<Rigidbody2D>();
                tRb.isKinematic = false;
                tRb.velocity = O_Rhand.velocity;

            }
            foreach (Transform t in O_Rhand.transform)
            {
                t.parent = null;
                Rigidbody2D tRb = t.GetComponent<Rigidbody2D>();
                tRb.isKinematic = false;
                tRb.velocity = O_Rhand.velocity;
            }
        }
    }
}
