using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Rewired;

public class PlayerController_Sigle_Catapult : MonoBehaviour
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
    Transform O_Lhand, O_Rhand;
    [SerializeField]
    SortingGroup[] O_Rarm, O_Larm;
    Vector2 V_previousMidpoinrPos;
    bool grounded = false;
    [SerializeField]
    LayerMask ground;
    [SerializeField]
    AnimationCurve throwCurve;
    [SerializeField]
    float throwForce;
    [SerializeField]
    float throwTimeMax;

    bool charging;
    float charge;
    float recRotation;
    Vector2 recDirection;
    Vector2 LastAim;

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
        AimArm(aimVector.normalized);
        ArmLayer();
        Jump();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) <= V_moveSpeed && grounded)
        {
            rb.AddForce(new Vector2(10000 * V_player.GetAxis("MoveX"), 0), ForceMode2D.Force);
        }
    }

    void Jump()
    {
        if (grounded == true && V_player.GetButtonDown("Jump"))
        {
            jump = true;
            jumpChrono = jumpTime;
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

    void ArmLayer()
    {
        if (Vector2.Distance(O_Rhand.position, transform.position) < Vector2.Distance(O_Lhand.position, transform.position))
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
    }

    void AimArm(Vector2 direction)
    {
        if (direction.magnitude != 0 && !charging)
        {
            LastAim = direction;
        }
        O_armMidpoint.attachedRigidbody.velocity = LastAim * V_armSpeed;

        if (V_player.GetButton("Grab"))
        {
            if (O_armMidpoint.connectedBody == null)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(O_armMidpoint.transform.position, 0.1f);
                float distance = Mathf.Infinity;
                Collider2D closest = null;
                foreach (Collider2D hit in hits)
                {
                    if (hit.tag == "GrabAble" && hit.gameObject != gameObject && Vector2.Distance(O_armMidpoint.transform.position, hit.transform.position) < distance)
                    {
                        closest = hit;
                    }
                }
                if (closest != null)
                {
                    O_armMidpoint.connectedBody = closest.attachedRigidbody;
                    O_armMidpoint.connectedAnchor = closest.transform.InverseTransformPoint(O_armMidpoint.transform.position);
                    O_armMidpoint.enabled = true;
                }
            }
            if (O_armMidpoint.connectedBody != null && charging)
            {
                charging = true;
                charge += Time.deltaTime;

                float cprc = charge / throwTimeMax;
                Debug.Log(cprc);
                O_armMidpoint.GetComponent<DistanceJoint2D>().distance = Mathf.Lerp(2.5f, 0.5f,cprc);
                Debug.DrawLine(O_armMidpoint.transform.position, ((Vector2)O_armMidpoint.transform.position+ recDirection),Color.red);
            }
        }
        if (V_player.GetButtonTimedPressDown("Grab", 0.3f))
        {
            if(O_armMidpoint.connectedBody != null)
            {
                charging = true;
                recDirection = (O_armMidpoint.transform.position-transform.position);
            }
        }
        if (V_player.GetButtonUp("Grab"))
        {
            if (O_armMidpoint.connectedBody != null && charging)
            {
                O_armMidpoint.connectedBody.velocity = recDirection*throwCurve.Evaluate(Mathf.Min(throwTimeMax,charge/throwTimeMax))*throwForce;
                O_armMidpoint.GetComponent<DistanceJoint2D>().distance = 2.5f;
                O_armMidpoint.connectedBody = null;
                O_armMidpoint.enabled = false;
                charging = false;
                charge = 0;
            }
        }
        V_previousMidpoinrPos = O_armMidpoint.transform.localPosition;
    }
}
