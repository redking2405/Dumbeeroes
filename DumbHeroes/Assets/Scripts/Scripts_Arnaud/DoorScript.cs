using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : ActivableObjects
{



    public Transform v_Destination;
    Vector3 v_OriginalPosition;
    bool v_Open;
    [SerializeField] float v_TimerMax;
    float v_Timer;
    [SerializeField] bool v_IsMoving;

    // Start is called before the first frame update
    void Start()
    {
        v_OriginalPosition = transform.position;
        v_Timer = v_TimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(v_Open)
        {
            if (v_IsMoving)
            {
                v_Timer -= Time.deltaTime;
                float ratio = v_Timer / 3f;
                ratio = 1f - ratio;
                
                transform.position = Vector3.Lerp(transform.position, v_Destination.position, ratio);
            }
            
            

            if(transform.position == v_Destination.position)
            {
                v_IsMoving = false;
            }
            if (!v_IsMoving)
            {
                v_Timer = v_TimerMax;
            }
        }

        else if (!v_Open)
        {
            if (v_IsMoving)
            {
                v_Timer -= Time.deltaTime;
                float ratio = v_Timer / 3f;
                ratio = 1f - ratio;
                transform.position=Vector3.Lerp(transform.position, v_OriginalPosition, ratio);

            }



            if (transform.position == v_OriginalPosition)
            {
                v_IsMoving = false;
            }
            if (!v_IsMoving)
            {
                v_Timer = v_TimerMax;
            }
        }
    }


    public override void Activate()
    {
        v_Open = true;
        v_IsMoving = true;
        base.Activate();
    }

    public override void Deactivate()
    {
        v_Open = false;
        v_IsMoving = true;
        base.Deactivate();
    }
}
