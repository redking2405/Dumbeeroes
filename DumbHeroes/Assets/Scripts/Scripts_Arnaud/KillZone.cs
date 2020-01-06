using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {

            collision.gameObject.GetComponent<PlayerController>().Respawn();
        }

        if (collision.gameObject.layer == 12 && collision.gameObject.tag == "CarryAble")
        {
            SFXManager.Instance.BoatLevel[4].Play();
            Destroy(collision.gameObject);
        }
    }
}
