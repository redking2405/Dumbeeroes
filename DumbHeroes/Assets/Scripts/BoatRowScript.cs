using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRowScript : MonoBehaviour
{
    float lastangle;
    [SerializeField]
    BoatBehaviour boat;
    [SerializeField]
    ParticleSystem rowparticle;

    private void FixedUpdate()
    {
        if(transform.localEulerAngles.z < lastangle)
        {
            float magnitude = lastangle - transform.localEulerAngles.z;
            boat.RowTheBoat((magnitude) /100);
            if (magnitude > 1)
            {
                rowparticle.Play();
            }
        }
        lastangle = transform.localEulerAngles.z;
    }
}
