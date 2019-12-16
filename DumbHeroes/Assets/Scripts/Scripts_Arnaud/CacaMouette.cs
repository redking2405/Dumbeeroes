using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacaMouette : MonoBehaviour
{
   public void SwitchLayer()
    {
        Invoke("CallSwitch", 0.1f);
    }


    void CallSwitch()
    {
        gameObject.layer = 0;
        gameObject.tag = "CarryAble";
    }
}
