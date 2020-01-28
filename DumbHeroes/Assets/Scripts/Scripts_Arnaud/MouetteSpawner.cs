using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteSpawner : MonoBehaviour
{
    [SerializeField] GameObject v_MouettePrefab;
    [SerializeField] float v_SpeedMax;
    float v_Speed;
    [SerializeField] float v_TimeBeforeObjectSpawnMax;
    float v_TimeBeforeObjectSpawn;
    float v_TimeBeforeActivate;
    [SerializeField] float v_TimeBeforeActivateMax;
    public AnimationCurve SpawnCurve;
    public BoatBehaviour boat;
    float v_TimeBeforeMouetteSpawn;
    bool flag;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!flag)
        {
            v_TimeBeforeMouetteSpawn -= Time.deltaTime;

            if (v_TimeBeforeMouetteSpawn <= 0)
            {
                flag = true;
                v_TimeBeforeMouetteSpawn = SpawnCurve.Evaluate(boat.boatProgress / boat.boatDistance);
            }
        }

        if (flag)
        {
            SpawnMouette();
            flag = false;
        }

        
    }

    public void SpawnMouette()
    {
            v_Speed = Mathf.RoundToInt(Random.Range(10, v_SpeedMax));
            v_TimeBeforeActivate = Mathf.RoundToInt(Random.Range(1, v_TimeBeforeActivateMax));
            v_TimeBeforeObjectSpawn = Mathf.RoundToInt(Random.Range(1, v_TimeBeforeObjectSpawnMax));
            GameObject mouette=Instantiate(v_MouettePrefab, transform.position, Quaternion.identity);
            mouette.GetComponent<Mouette>().Initialise(v_Speed, v_TimeBeforeActivate, v_TimeBeforeObjectSpawn);
    }
}
