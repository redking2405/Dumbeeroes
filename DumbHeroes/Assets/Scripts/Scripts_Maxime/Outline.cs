using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    public Material outlineMat;
    private Material baseMat;
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
        this.GetComponent<SpriteRenderer>().material = baseMat;
    }
}
