using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectEffect : MonoBehaviour
{
    ParticleSystem collparticles;
    TrailRenderer trail;
    Rigidbody2D rb;
    bool destroying;

    private void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        collparticles = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D p in collision.contacts)
        {
            collparticles.transform.position = p.point;
            collparticles.transform.rotation = Quaternion.FromToRotation(collparticles.transform.up, p.normal) * collparticles.transform.rotation;
            collparticles.Play();
        }
    }

    private void Update()
    {
        if(rb.velocity.magnitude < 1 && ! destroying)
        {
            StartCoroutine(Delete());
        }
    }

    IEnumerator Delete()
    {
        destroying = true;
        trail.emitting = false;
        collparticles.Stop();
        yield return new WaitForSeconds(2f);
        Destroy(trail.gameObject);
        Destroy(this);
    }
}
