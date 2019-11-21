using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonPeriodic : Canon
{
    [SerializeField] float v_AngleMax;
    [SerializeField] float v_AngleMin;
    bool v_WideOpen;
    protected float v_TimerRotate;
    [SerializeField] protected float v_TimerRotateMax;
    // Start is called before the first frame update
    protected override void Start()
    {
        v_Angle = v_AngleMin;
        v_TimerRotate = v_TimerRotateMax;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(v_Angle, Vector3.forward) * Vector3.right, Color.red);
        if (!v_WideOpen)
        {
            v_TimerRotate -= Time.deltaTime;
            float ratio = v_TimerRotate / 3f;
            ratio = 1f - ratio;
            v_Angle = Mathf.Lerp(v_Angle, v_AngleMax, ratio);
            if (v_Angle == v_AngleMax)
            {
                v_WideOpen = true;
                v_TimerRotate = v_TimerRotateMax;
            }
        }


        if (v_WideOpen)
        {
            v_TimerRotate -= Time.deltaTime;
            float ratio = v_TimerRotate / 3f;
            ratio = 1f - ratio;
            v_Angle = Mathf.Lerp(v_Angle, v_AngleMin, ratio);
            if (v_Angle == v_AngleMin)
            {
                v_WideOpen = false;
                v_TimerRotate = v_TimerRotateMax;
            }
        }
       
        base.Update();
    }
}
