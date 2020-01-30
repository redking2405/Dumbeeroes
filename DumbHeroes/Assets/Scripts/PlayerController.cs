using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Rewired;

public class PlayerController : MonoBehaviour
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

    enum Objectweight { Light, Medium, Heavy };

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
    Transform O_Lhand, O_Rhand, AimArrow;
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
    [SerializeField]
    GameObject ThrowEffects;

    public bool CarriedbyPlayer;
    bool charging;
    float charge;
    float originalMass;
    bool regrab = true;
    float recRotation;
    float grabpointDist;
    Vector2 LastAim = Vector2.right;

    int grabbedRecLayer;

    bool jump;
    float jumpChrono;

    bool isMovementLocked = false;
    bool isUnlockedOnGround = true;

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
        if(isMovementLocked && grounded && isUnlockedOnGround)
        {
            isMovementLocked = false;
        }
        anims.SetBool("floor", grounded);
        AimArm(aimVector.normalized);
        flip();
        Jump();
        Reload();
    }
    private void Reload()
    {
        if (V_player.GetButtonDown("Reload"))
        {
            FadeInOut.Instance.StartCoroutine(FadeInOut.Instance.FadeAndLoadScene(FadeInOut.FadeDirection.In, SceneManager.GetActiveScene().name));
        }
    }
    private void FixedUpdate()
    {
        //Debug.Log("Here : " + rb.velocity.x + ", " + V_moveSpeed  + ", " + CarriedbyPlayer);
        if (!isMovementLocked && ! CarriedbyPlayer)
        {
            rb.velocity = new Vector2(V_player.GetAxis("MoveX") * V_moveSpeed, rb.velocity.y);
            anims.SetFloat("velocity", Mathf.Abs(V_player.GetAxis("MoveX")));
        }
    }

    void Jump()
    {
        if (grounded == true && V_player.GetButtonDown("Jump"))
        {
            jump = true;
            jumpChrono = 0;
            if (SFXManager.Instance)
            {
                SFXManager.Instance.Character1[3].Play();
            }
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

    void flip()
    {
        if (O_armMidpoint.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (!CarriedObject || CarriedObject.tag != "CarryAble")
            {
                O_armMidpoint.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (O_armMidpoint.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (!CarriedObject || CarriedObject.tag != "CarryAble")
            {
                O_armMidpoint.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void AimArm(Vector2 direction)
    {
        if (direction.magnitude != 0)
        {
            LastAim = direction;
        }
        //O_armMidpoint.attachedRigidbody.velocity = LastAim * V_armSpeed;
        DistanceJoint2D anchor = O_armMidpoint.GetComponent<DistanceJoint2D>();
        Vector2 targetArmPos = (Vector2)transform.position + anchor.connectedAnchor/2 + LastAim * (anchor.distance-0.1f);
        O_armMidpoint.attachedRigidbody.MovePosition(Vector2.Lerp(O_armMidpoint.transform.position,targetArmPos,Time.deltaTime*V_armSpeed));
        Vector2 dir = O_armMidpoint.transform.position - O_armMidpoint.GetComponent<DistanceJoint2D>().connectedBody.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (CarriedObject != null && CarriedObject.tag == "CarryAble")
        {
            O_armMidpoint.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            O_armMidpoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            O_armMidpoint.transform.rotation = Quaternion.identity;
        }

        if (V_player.GetButtonDown("Grab"))
        {
            if (CarriedObject != null)
            {
                regrab = false;
                if (charging)
                {
                    ThrowObject();
                }
                else
                {
                    DropObject();
                }
            }

            StartCoroutine(ShakeController(1, 0.1f, 1, false));
        }
        if (V_player.GetButtonUp("Grab"))
        {
            regrab = true;
        }
        if (V_player.GetButton("Grab"))
        {
            if (CarriedObject == null && regrab)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(O_armMidpoint.transform.position, 0.1f);
                float distance = Mathf.Infinity;
                Collider2D closest = null;
                foreach (Collider2D hit in hits)
                {
                    if ((hit.tag == "GrabAble" || hit.tag == "CarryAble" || (hit.tag == "Player" && !CarriedbyPlayer)) && hit.gameObject != gameObject && Vector2.Distance(O_armMidpoint.transform.position, hit.transform.position) < distance)
                    {
                        closest = hit;
                    }
                }
                if (closest != null)
                {
                    regrab = false;
                    anims.SetBool("grab", true);
                    O_armMidpoint.connectedBody = closest.attachedRigidbody;
                    grabbedRecLayer = CarriedObject.gameObject.layer;
                    CarriedObject.gameObject.layer = 9;
                    if(closest.tag == "CarryAble")
                    {
                        O_armMidpoint.connectedAnchor = Vector2.zero;
                        CarriedObject.transform.parent = O_armMidpoint.transform;

                    }
                    else
                    {
                        O_armMidpoint.connectedAnchor = closest.transform.InverseTransformPoint(O_armMidpoint.transform.position);
                        if (closest.tag == "Player")
                        {
                            O_armMidpoint.connectedAnchor = Vector2.zero;
                            originalMass = closest.attachedRigidbody.mass;
                            closest.attachedRigidbody.mass = 10f;
                            closest.GetComponent<PlayerController>().CarriedbyPlayer = true;
                        }
                    }
                    O_armMidpoint.enabled = true;
                }
            }
        }


        if (V_player.GetButton("Throw"))
        {
            if (CarriedObject != null)
            {
                charging = true;
                charge += Time.deltaTime;
                float cprc = charge / throwTimeMax;
                V_player.SetVibration(0, vibrateCurve.Evaluate(cprc));
                float arrowScale = Mathf.Lerp(0, 1f, cprc);
                AimArrow.transform.localScale = new Vector3(arrowScale,arrowScale,1);
                //V_player.SetVibration(1, vibrateCurve.Evaluate(cprc));
                O_armMidpoint.GetComponent<DistanceJoint2D>().distance = Mathf.Lerp(grabpointDist, 1f, cprc);

               // O_armMidpoint.attachedRigidbody.velocity = LastAim * V_armSpeed;
                AimArrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(LastAim.y, LastAim.x));
                Debug.DrawLine(O_armMidpoint.transform.position, ((Vector2)O_armMidpoint.transform.position + LastAim * 2), Color.red);
            }
        }
        if (V_player.GetButtonUp("Throw"))
        {
            if (CarriedObject != null && charging)
            {
                ThrowObject();
            }
        }
    }

    void ThrowObject()
    {
        StartCoroutine(ShakeController(1, 0.3f, 1, false));
        if (CarriedObject.tag == "CarryAble" || CarriedObject.tag == "Player" || grounded)
        {
            CarriedObject.AddForceAtPosition(LastAim * throwCurve.Evaluate(Mathf.Min(throwTimeMax, charge / throwTimeMax)) * throwForce, CarriedObject.position + O_armMidpoint.connectedAnchor, ForceMode2D.Impulse);
            Instantiate(ThrowEffects, CarriedObject.transform);
            ThrownObjectEffect fx = CarriedObject.gameObject.GetComponent<ThrownObjectEffect>();
            if (!fx)
            {
                if (CarriedObject.tag == "Carryable")
                {
                    SFXManager.Instance.GeneralSound[1].Play();
                }
                else SFXManager.Instance.Character1[0].Play();
                //CarriedObject.gameObject.AddComponent<ThrownObjectEffect>();
            }
            else
            {
                fx.trail.emitting = true;
            }
        }
        O_armMidpoint.GetComponent<DistanceJoint2D>().distance = grabpointDist;
        charging = false;
        charge = 0;
        DropObject();
    }

    public void DropObject()
    {
        AimArrow.transform.localScale = Vector3.zero;
        anims.SetBool("grab", false);
        CarriedObject.gameObject.layer = grabbedRecLayer;
        if (CarriedObject.tag == "CarryAble")
        {
            CarriedObject.transform.parent = null;
            SFXManager.Instance.Character2[1].Play();
        }
        if (CarriedObject.tag == "Player")
        {
            CarriedObject.mass = originalMass;
            PlayerController otherPc = CarriedObject.GetComponent<PlayerController>();
            otherPc.CarriedbyPlayer = false;
            otherPc.SetMovementLocked(true);
        }
        V_player.SetVibration(0, 0);
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
        GUI.Label(new Rect(20, 15, 100, 100), "" + (int)rb.velocity.x);
    }

    void OutlineCheck()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(O_armMidpoint.transform.position, 0.1f);
        float distance = Mathf.Infinity;
        Collider2D closest = null;
        foreach (Collider2D hit in hits)
        {
            if (hit.tag == "CarryAble" && hit.gameObject != gameObject && Vector2.Distance(O_armMidpoint.transform.position, hit.transform.position) < distance)
            {
                closest = hit;
            }
        }
        if (closest != null)
        {
            if (closest.GetComponent<Outline>())
            {
                closest.GetComponent<Outline>().OutlineDisplay(true);

            }
        }
    }

    private void LateUpdate()
    {
        OutlineCheck();
    }

    public void Respawn()
    {
        //Rajouter Anim éventuelle
        gameObject.GetComponentInParent<Transform>().position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, gameObject.GetComponentInParent<Transform>().position.z);

        
    }

    public void SetMovementLocked(bool isUnlockedOnGround_ = true) {
        isMovementLocked = true;
        isUnlockedOnGround = isUnlockedOnGround_;
        grounded = false;
    }
}
