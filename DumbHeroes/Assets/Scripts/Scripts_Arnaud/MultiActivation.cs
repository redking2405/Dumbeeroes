using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActivation : ActivableObjects
{
    public List<Interrupteur> v_MyBoolList;
    int countBool=0;
    public List<ActivableObjects> v_ObjectToActivate;
    bool trigger;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (countBool == v_MyBoolList.Count)
        {
            for (int j = 0; j < v_ObjectToActivate.Count; j++)
            {
                v_ObjectToActivate[j].Activate();
            }
            counter = 0;
            trigger = false;
        }

        else
        {
            for (int k = 0; k < v_ObjectToActivate.Count; k++)
            {
                v_ObjectToActivate[k].Deactivate();
            }
            counter = 0;
            trigger = false;
        }
    }

    public override void Activate()
    {
        counter++;

        if (counter==1 && !trigger)
        {
            counter = 0;
            trigger = true;
            countBool++;
        }
        base.Activate();
    }

    public override void Deactivate()
    {
        counter++;
        if(counter==1 & trigger)
        {
            counter = 0;
            trigger = false;
            countBool--;
        }
    
        base.Deactivate();
    }

    

}
