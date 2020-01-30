using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteSpawner : MonoBehaviour
{
    [SerializeField] GameObject v_MouettePrefab;
    public AnimationCurve SpawnCurve;
    public BoatBehaviour boat;
    public float Spawnrate;
    float spawning;


    void Update()
    {
        spawning += SpawnCurve.Evaluate(boat.boatProgress / boat.boatDistance) *Time.deltaTime/Spawnrate*Random.Range(0f,1f);
        //Debug.Log(spawning);
        if (spawning >= 1)
        {
            SpawnMouette();
            spawning --;
        }

    }

    public void SpawnMouette()
    {
            GameObject mouette=Instantiate(v_MouettePrefab, (Vector2)transform.position + new Vector2(0,Random.Range(-1.5f,1.5f)), Quaternion.identity);
    }
}
