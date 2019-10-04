using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public int V_playerId;
    Player V_player;
    Rigidbody2D rb;
    public FixedJoint2D O_armMidpoint;

    [SerializeField]
    float V_moveSpeed;
    [SerializeField]
    float V_armSpeed;
    [SerializeField]
    Rigidbody2D O_Lhand,O_Rhand;
    [SerializeField]
    SpringJoint2D O_armSrping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        V_player = ReInput.players.GetPlayer(V_playerId);
    }
    
    void Update()
    {
        Vector2 aimVector = new Vector2(V_player.GetAxis("ArmX"), V_player.GetAxis("ArmY"));
        O_armMidpoint.transform.position = new Vector2((O_Lhand.transform.position.x+ O_Rhand.transform.position.x)/2, (O_Lhand.transform.position.y + O_Rhand.transform.position.y)/2);
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
            O_armSrping.distance = Vector2.Distance(O_Lhand.transform.position, O_Rhand.transform.position);
            Collider2D[] hits = Physics2D.OverlapCircleAll(O_armMidpoint.transform.position, 0.5f);
            foreach (Collider2D hit in hits)
            {
                Debug.Log(hit.gameObject.name);
                if (hit.tag == "GrabAble")
                {
                    O_armSrping.enabled = true;
                    O_armMidpoint.enabled = true;
                    O_armMidpoint.connectedBody = hit.attachedRigidbody;
                    break;
                }
            }
        }
        if (V_player.GetButtonUp("Grab"))
        {
            O_armSrping.enabled = false;
            if (O_armMidpoint.connectedBody != null)
            {
                O_armMidpoint.connectedBody.velocity = O_Lhand.velocity;
                O_armMidpoint.connectedBody = null;
                O_armMidpoint.enabled = false;
            }
        }
    }
}
