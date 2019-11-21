using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    public Material outlineMat;
    private Material baseMat;
    private bool lastCall;
    
    // Start is called before the first frame update
    void Start()
    {
        baseMat = this.GetComponent<SpriteRenderer>().material;
        if(outlineMat==null)
        {
            outlineMat = baseMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        OutlineDisplay(false);
    }

    public void OutlineDisplay(bool outlined)
    {
        if (outlined == true && lastCall!=true)
        {
            this.GetComponent<SpriteRenderer>().material = outlineMat;

            lastCall = outlined;
        }
        if(outlined == false && lastCall!=false)
        {
            this.GetComponent<SpriteRenderer>().material = baseMat;

            lastCall = outlined;

        }
    }
}
