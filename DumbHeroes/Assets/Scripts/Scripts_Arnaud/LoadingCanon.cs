using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingCanon : MonoBehaviour
{
    Canon v_MyCanon;
    GameObject v_Target;
    
    // Start is called before the first frame update

    private void Awake()
    {
        v_MyCanon = GetComponentInParent<Canon>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CarryAble" || collision.gameObject.tag=="Player")
        {

            if (!v_MyCanon.v_Loaded && !v_MyCanon.v_Ready)
            {
                StartCoroutine(LoadCanon(collision.gameObject));
            }
            
            
            
                
            
            
            
        }

        
    }

    
    
    IEnumerator LoadCanon(GameObject target)
    {
        yield return new WaitForSeconds(0.1f);
        v_MyCanon.v_Loaded = true;
        target.transform.position = v_MyCanon.v_Position.position;
        v_MyCanon.v_ToLaunch = target;
        
       
    }
}
