using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObjectEffect : MonoBehaviour
{
    ParticleSystem collparticles;
    public TrailRenderer trail;
    Rigidbody2D rb;
    const float groundedDuration = 0.2f;
    Dictionary<GameObject, float> contactTimesPerObject = new Dictionary<GameObject, float>();

    private void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        collparticles = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!contactTimesPerObject.ContainsKey(collision.gameObject) || Time.time - contactTimesPerObject[collision.gameObject] > groundedDuration &&  rb.velocity.magnitude > 1)
        {
            foreach (ContactPoint2D p in collision.contacts)
            {
                collparticles.transform.position = p.point;
                collparticles.transform.rotation = Quaternion.FromToRotation(collparticles.transform.up, p.normal) * collparticles.transform.rotation;
                collparticles.Play();
            }
        }
        contactTimesPerObject[collision.gameObject] = Time.time;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        contactTimesPerObject[collision.gameObject] = Time.time;
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 1)
        {
            trail.emitting = false;
        }
    }
}
