using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    Vector2 V_previousMidpoinrPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        V_player = ReInput.players.GetPlayer(V_playerId);
    }
    
    void Update()
    {
        Vector2 aimVector = new Vector2(V_player.GetAxis("ArmX"), V_player.GetAxis("ArmY"));
        O_armMidpoint.transform.position = new Vector2((O_Lhand.transform.position.x+ O_Rhand.transform.position.x)/2, (O_Lhand.transform.position.y + O_Rhand.transform.position.y)/2);
        AimArm(aimVector.normalized);
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
            O_Rhand.velocity = direction*V_armSpeed;
        }
        if (V_player.GetButton("Grab"))
        {
            if (O_armMidpoint.connectedBody == null)
            {
                O_armSrping.distance = Vector2.Distance(O_Lhand.transform.position, O_Rhand.transform.position);
                Collider2D[] hits = Physics2D.OverlapCircleAll(O_armMidpoint.transform.position, 0.1f);
                float distance = Mathf.Infinity;
                Collider2D closest = null;
                foreach (Collider2D hit in hits)
                {
                    Debug.Log(hit.gameObject.name);
                    if (hit.tag == "GrabAble" && hit.gameObject != gameObject && Vector2.Distance(O_armMidpoint.transform.position, hit.transform.position) < distance)
                    {
                        closest = hit;
                    }
                }
                if (closest != null)
                {
                    O_armSrping.enabled = true;
                    O_armMidpoint.enabled = true;
                    O_armMidpoint.connectedBody = closest.attachedRigidbody;
                    O_armMidpoint.connectedAnchor = closest.transform.InverseTransformPoint(O_armMidpoint.transform.position);
                }
            }
        }
        if (V_player.GetButtonUp("Grab"))
        {
            O_armSrping.enabled = false;
            if (O_armMidpoint.connectedBody != null)
            {
                O_armMidpoint.connectedBody.velocity = ((Vector2)O_armMidpoint.transform.position - V_previousMidpoinrPos)/ Time.deltaTime;
                O_armMidpoint.connectedBody = null;
                O_armMidpoint.enabled = false;
            }
        }
        V_previousMidpoinrPos = O_armMidpoint.transform.position;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), ""+ O_Rhand.angularVelocity);
    }
}
