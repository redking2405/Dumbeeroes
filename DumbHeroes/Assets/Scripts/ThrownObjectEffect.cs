using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectEffect : MonoBehaviour
{
    public ParticleSystem collparticles;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D p in collision.contacts)
        {
            collparticles.transform.position = p.point;
            collparticles.transform.rotation = Quaternion.FromToRotation(collparticles.transform.up, p.normal) * collparticles.transform.rotation;
            collparticles.Play();
        }

    }
}
