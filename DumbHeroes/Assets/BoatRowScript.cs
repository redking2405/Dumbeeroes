using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRowScript : MonoBehaviour
{
    float lastangle;
    [SerializeField]
    BoatBehaviour boat;

    private void FixedUpdate()
    {
        if(transform.localEulerAngles.z > lastangle)
        {
            boat.RowTheBoat((transform.localEulerAngles.z - lastangle)/100);
        }
        lastangle = transform.localEulerAngles.z;
    }
}
