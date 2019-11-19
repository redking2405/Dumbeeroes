using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : ActivableObjects
{
    public float v_Angle;
    public float v_Force;
    [SerializeField] Transform v_Position;
    [SerializeField] float v_TimerMax;
    float v_Timer;
    GameObject v_ToLaunch;
    [SerializeField] GameObject v_PrefabCanonBall;
    bool v_Launched;
    bool v_Ready;
    public bool v_Loaded;
    bool canShoot;
    // Start is called before the first frame update
    void Start()
    {
        v_Timer = v_TimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            if (!v_Ready)
            {
                v_Timer -= Time.deltaTime;

                if (v_Timer <= 0)
                {
                    v_Ready = true;
                    v_Launched = false;
                }
            }

            if (v_Ready && !v_Launched)
            {
                Shoot();
            }
        }
        
    }

    public override void Activate()
    {
        canShoot = true;
        base.Activate();
    }


    public override void Deactivate()
    {
        canShoot = false;
        base.Deactivate();
    }
    public void Shoot()
    {
        v_Timer = v_TimerMax;
        if (v_Loaded)
        {

        }

        else
        {
            GameObject canonBall = Instantiate(v_PrefabCanonBall, v_Position.position, Quaternion.identity);
            Rigidbody2D rbd = canonBall.GetComponent<Rigidbody2D>();
            if (rbd)
            {
                //v_Angle = Mathf.Deg2Rad * v_Angle;
                var direction = Quaternion.AngleAxis(v_Angle, Vector3.forward) * Vector3.right;
                direction = direction.normalized;
                rbd.AddForce(direction * v_Force, ForceMode2D.Impulse);

            }

            
        }

        v_Launched = true;
        v_Ready = false;
    }
}
