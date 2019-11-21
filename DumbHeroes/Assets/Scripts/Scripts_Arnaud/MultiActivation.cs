using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActivation : ActivableObjects
{
    public List<Interrupteur> v_MyInteList;
    public List<bool> v_MyBoolList;
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

        if (CheckIfIsOpen())
        {
            for(int i = 0; i<v_ObjectToActivate.Count; i++)
            {
                v_ObjectToActivate[i].Activate();
            }
        }

        else
        {
            for (int j = 0; j < v_ObjectToActivate.Count; j++)
            {
                v_ObjectToActivate[j].Deactivate();
            }
        }
    }

    bool CheckIfIsOpen()
    {
        bool valeurRetour =false;

        for(int i=0; i < v_MyInteList.Count; i++)
        {
            
            v_MyBoolList[i] = v_MyInteList[i].v_IsTriggered;
            
        }
        countBool = 0;
        for (int j=0; j<v_MyBoolList.Count; j++)
        {
            
            if (v_MyBoolList[j])
            {
                countBool++;
            }
        }
        if (countBool == v_MyBoolList.Count)
        {
            valeurRetour = true;
        }
        return valeurRetour;
    }


   

    

}
