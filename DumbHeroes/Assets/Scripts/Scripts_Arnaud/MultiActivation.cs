using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActivation : ActivableObjects
{
    public List<bool> v_MyBoolList;
    int countBool=0;
    public List<ActivableObjects> v_ActivableObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (bool b in v_MyBoolList)
        {
            int i = 0;
            if (b)
            {
                i++;
                countBool++;
            }

            else
            {
                i++;
                continue;
            }

            



        }

        if (countBool == v_MyBoolList.Count)
        {
            for (int j = 0; j < v_MyBoolList.Count; j++)
            {
                v_ActivableObjects[j].Activate();
            }
        }

        else
        {
            for (int k = 0; k < v_MyBoolList.Count; k++)
            {
                v_ActivableObjects[k].Deactivate();
            }
        }
    }

    public override void Activate()
    {
        int i = 0;
        foreach(bool b in v_MyBoolList)
        {
            if (b)
            {
                i++;
                continue;
            }

            else
            {
                v_MyBoolList[i] = true;
            }
            
              
            
        }
        base.Activate();
    }

    public override void Deactivate()
    {
        int i = 0;
        foreach (bool b in v_MyBoolList)
        {
            if (!b)
            {
                i++;
                continue;
            }

            else
            {
                v_MyBoolList[i] = false;
            }



        }
        base.Deactivate();
    }
}
