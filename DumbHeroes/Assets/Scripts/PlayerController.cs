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
    Transform O_groundcheck;
    [SerializeField]
    float jumpForce = 0;
    [SerializeField]
    float jumpTime = 0;
    [SerializeField]
    float V_armSpeed = 0;
    [SerializeField] 
    HingeJoint2D O_Lhand,O_Rhand;
    [SerializeField]
    SortingGroup[] O_Rarm, O_Larm;
    [SerializeField]
    SpringJoint2D O_armSrping;
    Vector2 V_previousMidpoinrPos;
    bool grounded = false;
    [SerializeField]
    LayerMask ground;

    bool jump;
    float jumpChrono;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        V_player = ReInput.players.GetPlayer(V_playerId);
    }
    
    void Update()
    {
        Vector2 aimVector = new Vector2(V_player.GetAxis("ArmX"), V_player.GetAxis("ArmY"));
        grounded = Physics2D.Raycast(O_groundcheck.position, -Vector2.up, 0.1f, ground);
        O_armMidpoint.transform.position = new Vector2((O_Lhand.transform.position.x+ O_Rhand.transform.position.x)/2, (O_Lhand.transform.position.y + O_Rhand.transform.position.y)/2);
        AimArm(aimVector.normalized);
        Jump();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) <= V_moveSpeed && grounded)
        {
            rb.AddForce(new Vector2(10000 * V_player.GetAxis("MoveX"), 0), ForceMode2D.Force);
            //rb.velocity = new Vector2(V_player.GetAxis("MoveX") * V_moveSpeed, rb.velocity.y);
        }
    }

    void Jump()
    {
        if (grounded == true && V_player.GetButtonDown("Jump"))
        {
            jump = true;
            jumpChrono = jumpTime;
            Debug.Log("jump");
        }
        if (V_player.GetButton("Jump") && jump)
        {
            if (jumpChrono > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpChrono -= Time.deltaTime;
            }
            else
            {
                jump = false;
            }
        }
        if (V_player.GetButtonUp("Jump"))
        {
            jump = false;
        }
    }

    void AimArm(Vector2 direction)
    {
        if(Vector2.Distance(O_Rhand.transform.position,transform.position) < Vector2.Distance(O_Lhand.transform.position, transform.position))
        {
            foreach (SortingGroup s in O_Larm)
            {
                s.sortingOrder = 1;
            }
            foreach (SortingGroup s in O_Rarm)
            {
                s.sortingOrder = 2;
            }
        }
        else
        {
            foreach (SortingGroup s in O_Larm)
            {
                s.sortingOrder = 2;
            }
            foreach (SortingGroup s in O_Rarm)
            {
                s.sortingOrder = 1;
            }
        }

        if (direction.magnitude != 0)
        {
            O_Lhand.attachedRigidbody.velocity = direction * V_armSpeed;
            O_Rhand.attachedRigidbody.velocity = direction * V_armSpeed;
        }
        //if(Mathf.Sign(O_Lhand.transform.position.x) != V_player.GetAxis("MoveX"))
        //{
        //    O_Lhand.attachedRigidbody.velocity = new Vector2(O_Lhand.attachedRigidbody.velocity.x,2);
        //}
        //if (Mathf.Sign(O_Rhand.transform.position.x) != V_player.GetAxis("MoveX"))
        //{
        //    O_Rhand.attachedRigidbody.velocity = new Vector2(O_Lhand.attachedRigidbody.velocity.x, 2);
        //}
        if (V_player.GetButton("Grab"))
        {
            if (O_Lhand.connectedBody == null)
            {
                //O_armSrping.distance = Vector2.Distance(O_Lhand.transform.position, O_Rhand.transform.position);
                Collider2D[] hits = Physics2D.OverlapCircleAll(O_Lhand.transform.position, 0.1f);
                float distance = Mathf.Infinity;
                Collider2D closest = null;
                foreach (Collider2D hit in hits)
                {
                    if (hit.tag == "GrabAble" && hit.gameObject != gameObject && Vector2.Distance(O_Lhand.transform.position, hit.transform.position) < distance)
                    {
                        closest = hit;
                    }
                }
                if (closest != null)
                {
                    //O_armSrping.enabled = true;
                    //O_armMidpoint.enabled = true;
                    //O_armMidpoint.connectedBody = closest.attachedRigidbody;
                    //O_armMidpoint.connectedAnchor = closest.transform.InverseTransformPoint(O_armMidpoint.transform.position);
                    O_Lhand.connectedBody = closest.attachedRigidbody;
                    O_Lhand.connectedAnchor = closest.transform.InverseTransformPoint(O_Lhand.transform.position);
                    O_Lhand.enabled = true;
                }
            }
            if (O_Rhand.connectedBody == null)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(O_Rhand.transform.position, 0.1f);
                float distance = Mathf.Infinity;
                Collider2D closest = null;
                foreach (Collider2D hit in hits)
                {
                    if (hit.tag == "GrabAble" && hit.gameObject != gameObject && Vector2.Distance(O_Rhand.transform.position, hit.transform.position) < distance)
                    {
                        closest = hit;
                    }
                }
                if (closest != null)
                {
                    O_Rhand.connectedBody = closest.attachedRigidbody;
                    O_Rhand.connectedAnchor = closest.transform.InverseTransformPoint(O_Rhand.transform.position);
                    O_Rhand.enabled = true;
                }
            }
        }
        if (V_player.GetButtonUp("Grab"))
        {
            O_armSrping.enabled = false;
            if (O_Lhand.connectedBody != null)
            {
                O_Lhand.connectedBody.velocity = ((Vector2)O_armMidpoint.transform.position - V_previousMidpoinrPos) / Time.deltaTime;
                O_Lhand.connectedBody = null;
                O_Lhand.enabled = false;
            }
            if (O_Rhand.connectedBody != null)
            {
                O_Rhand.connectedBody.velocity = ((Vector2)O_armMidpoint.transform.position - V_previousMidpoinrPos) / Time.deltaTime;
                O_Rhand.connectedBody = null;
                O_Rhand.enabled = false;
            }
        }
        V_previousMidpoinrPos = O_armMidpoint.transform.position;
    }
}
