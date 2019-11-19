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
    public Animator anims;
    public Rigidbody2D CarriedObject
    {
        get { return O_armMidpoint.connectedBody; }
    }

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
    AnimationCurve throwCurve, vibrateCurve, jumpCurve;
    [SerializeField]
    float throwForce;
    [SerializeField]
    float throwTimeMax;

    bool charging;
    bool regrab = true;
    float charge;
    float recRotation;
    float grabpointDist;
    Vector2 recDirection;
    Vector2 LastAim;

    bool jump;
    float jumpChrono;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        V_player = ReInput.players.GetPlayer(V_playerId);
        anims = GetComponentInParent<Animator>();
        grabpointDist = O_armMidpoint.GetComponent<DistanceJoint2D>().distance;
    }

    void Update()
    {
        Vector2 aimVector = new Vector2(V_player.GetAxis("ArmX"), V_player.GetAxis("ArmY"));
        grounded = Physics2D.Raycast(O_groundcheck.position, -Vector2.up, 0.1f, ground);
        anims.SetBool("floor", grounded);
        AimArm(aimVector.normalized);
        ArmLayer();
        Jump();
    }

    private void FixedUpdate()
    {
        //if (Mathf.Abs(rb.velocity.x) <= V_moveSpeed)
        //{
        //rb.AddForce(new Vector2(10000 * V_player.GetAxis("MoveX"), 0), ForceMode2D.Force);
        //Physics2DExtensions.AddForce(rb, new Vector2(10000 * V_player.GetAxis("MoveX"), 0), ForceMode.Force);
        //Physics2DExtensions.AddForce(rb, 10000 * V_player.GetAxis("MoveX"),)
        anims.SetFloat("velocity", Mathf.Abs(V_player.GetAxis("MoveX")));
            rb.velocity = new Vector2(V_moveSpeed * V_player.GetAxis("MoveX"), rb.velocity.y);
        //}
    }

    

    void Jump()
    {
        if (grounded == true && V_player.GetButtonDown("Jump"))
        {
            jump = true;
            jumpChrono = 0;
        }
        if (V_player.GetButton("Jump") && jump)
        {
            if (jumpChrono < jumpTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpCurve.Evaluate(jumpChrono/jumpTime)*jumpForce);
                jumpChrono += Time.deltaTime;
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
        anims.SetBool("jump", jump);
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
                    anims.SetBool("grab", true);
                    O_armMidpoint.connectedBody = closest.attachedRigidbody;
                    O_armMidpoint.connectedAnchor = closest.transform.InverseTransformPoint(O_armMidpoint.transform.position);
                    O_armMidpoint.enabled = true;
                }
            }
        }
        if (V_player.GetButton("Throw"))
        {
            if (O_armMidpoint.connectedBody != null)
            {
                charging = true;
                charge += Time.deltaTime;
                float cprc = charge / throwTimeMax;
                V_player.SetVibration(0, vibrateCurve.Evaluate(cprc));
                //V_player.SetVibration(1, vibrateCurve.Evaluate(cprc));
                O_armMidpoint.GetComponent<DistanceJoint2D>().distance = Mathf.Lerp(grabpointDist, 1.4f, cprc);
                Debug.DrawLine(O_armMidpoint.transform.position, ((Vector2)O_armMidpoint.transform.position + recDirection * 2), Color.red);
            }
        }
        if (V_player.GetButtonUp("Grab"))
        {
            regrab = true;
            if (O_armMidpoint.connectedBody != null)
            {
                if (charging)
                {
                    Debug.Log("dropthrow");
                    ThrowObject();
                }
                else
                {
                    Debug.Log("drop");
                    DropObject();
                }
            }
        }
        if (V_player.GetButtonUp("Throw"))
        {
            if (O_armMidpoint.connectedBody != null && charging)
            {
                Debug.Log("throw");
                ThrowObject();
            }
        }
    }

    void ThrowObject()
    {
        StartCoroutine(ShakeController(1, 0.3f, 1, false));
        regrab = false;
        O_armMidpoint.connectedBody.velocity = recDirection * throwCurve.Evaluate(Mathf.Min(throwTimeMax, charge / throwTimeMax)) * throwForce;
        O_armMidpoint.GetComponent<DistanceJoint2D>().distance = grabpointDist;
        charging = false;
        charge = 0;
        DropObject();
    }

    void DropObject()
    {
        anims.SetBool("grab", false);
        V_player.SetVibration(0, 0);
        O_armMidpoint.connectedBody = null;
        O_armMidpoint.enabled = false;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 15, 100, 100), "" + (int)rb.velocity.x);
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

    void OutlineCheck()
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
            if(closest.GetComponent<Outline>())
            {
                closest.GetComponent<SpriteRenderer>().material = closest.GetComponent<Outline>().outlineMat;
            }
        }
    }

    private void LateUpdate()
    {
        OutlineCheck();
    }
}
