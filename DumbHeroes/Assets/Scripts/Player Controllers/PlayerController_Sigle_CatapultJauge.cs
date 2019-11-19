using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Rewired;

public class PlayerController_Sigle_CatapultJauge : MonoBehaviour
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
    bool grounded = false;
    [SerializeField]
    LayerMask ground;
    [SerializeField]
    AnimationCurve throwCurve,vibrationCurve;
    [SerializeField]
    float throwForce;
    [SerializeField]
    float throwTimeMax;

    bool charging;
    bool regrab = true;
    float charge;
    float recRotation;
    Vector2 recDirection;
    Vector2 LastAim;
    float lowestpress;

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
        if (Mathf.Abs(rb.velocity.x) <= V_moveSpeed)
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
        if (direction.magnitude != 0)
        {
            LastAim = direction;
        }
        O_armMidpoint.attachedRigidbody.velocity = LastAim * V_armSpeed;
        recDirection = (O_armMidpoint.transform.position - transform.position).normalized;
        if (V_player.GetButton("Grab") && regrab)
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
        }
        //if (V_player.GetButton("Throw"))
        //{
        //    if(O_armMidpoint.connectedBody != null)
        //    {
        //            charging = true;
        //            charge += Time.deltaTime;

        //            float cprc = charge / throwTimeMax;
        //            O_armMidpoint.GetComponent<DistanceJoint2D>().distance = Mathf.Lerp(2.5f, 1f, cprc);
        //            Debug.DrawLine(O_armMidpoint.transform.position, ((Vector2)O_armMidpoint.transform.position + recDirection * 2), Color.red);
        //    }
        //}

        Debug.DrawLine(O_armMidpoint.transform.position, ((Vector2)O_armMidpoint.transform.position + recDirection * V_player.GetAxis("Throw")), Color.red);
        O_armMidpoint.GetComponent<DistanceJoint2D>().distance = Mathf.Lerp(2.5f, 1f, V_player.GetAxis("Throw"));
        if (V_player.GetButtonUp("Grab"))
        {
            regrab = true;
            if (O_armMidpoint.connectedBody != null)
            {
                if (V_player.GetAxis("Throw") > 0.01f)
                {
                    ThrowObject();
                }
                else
                {
                    DropObject();
                }
            }
        }
        float axis = V_player.GetAxis("Throw");
        if (O_armMidpoint.connectedBody)
        {
            if (axis < Mathf.Max(0, lowestpress - 0.1f))
            {
                ThrowObject();
            }
            else
            {
                V_player.SetVibration(0, vibrationCurve.Evaluate(axis));
                lowestpress = axis;
            }
        }
    }

    void ThrowObject()
    {
        StartCoroutine(ShakeController(1, 0.3f, 1, false));
        regrab = false;
        O_armMidpoint.connectedBody.velocity = recDirection * throwCurve.Evaluate(lowestpress) * throwForce;
        O_armMidpoint.GetComponent<DistanceJoint2D>().distance = 2.5f;
        charging = false;
        charge = 0;
        DropObject();
    }

    void DropObject()
    {
        V_player.SetVibration(0, 0);
        lowestpress = 0;
        O_armMidpoint.connectedBody = null;
        O_armMidpoint.enabled = false;
    }

    IEnumerator ShakeController(float power, float time, int motor, bool both)
    {
        if (both)
        {
            V_player.SetVibration(0, power);
            V_player.SetVibration(1, power);
            yield return new WaitForSeconds(time);
            V_player.SetVibration(0, 0);
            V_player.SetVibration(1, 0);
            yield break;
        }
        else
        {
            V_player.SetVibration(motor, power);
            yield return new WaitForSeconds(time);
            V_player.SetVibration(motor, 0);
            yield break;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 15, 100, 100), "" + V_player.GetAxis("Throw"));
        GUI.Label(new Rect(20, 30, 100, 100), "" + lowestpress);
        GUI.Label(new Rect(20, 0, 100, 100), "" + (int)rb.velocity.x);
    }
}
