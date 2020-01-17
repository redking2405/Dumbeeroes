using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectEffect : MonoBehaviour
{
    public ParticleSystem collparticles;
    public TrailRenderer trail;

    private void Awake()
    {
        trail = gameObject.GetComponent<TrailRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ponk");
        foreach(ContactPoint2D p in collision.contacts)
        {
            collparticles.transform.position = p.point;
            collparticles.transform.rotation = Quaternion.FromToRotation(collparticles.transform.up, p.normal) * collparticles.transform.rotation;
            collparticles.Play();
        }
        //trail.emitting = false;
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this);
    }
}
