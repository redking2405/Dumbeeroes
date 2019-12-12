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
    int v_NumberOfMouetteToSpawn;
    [SerializeField]int v_NumberOfMouetteToSpawnMax;
    [SerializeField] BoatBehaviour v_Progres;
    [SerializeField]float v_TimeBeforeMouetteSpawnMax0;
    [SerializeField]float v_TimeBeforeMouetteSpawnMax25;
    [SerializeField]float v_TimeBeforeMouetteSpawnMax50;
    [SerializeField]float v_TimeBeforeMouetteSpawnMax75;
    float v_TimeBeforeMouetteSpawn;
    bool flag;
    // Start is called before the first frame update
    void Start()
    {
        v_TimeBeforeMouetteSpawn = v_TimeBeforeMouetteSpawnMax0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag)
        {
            v_TimeBeforeMouetteSpawn -= Time.deltaTime;

            if (v_TimeBeforeMouetteSpawn <= 0)
            {
                flag = true;
                if(v_Progres.boatProgress>25f && v_Progres.boatProgress <= 50f)
                {
                    v_TimeBeforeMouetteSpawn = v_TimeBeforeMouetteSpawnMax25;
                }

                else if(v_Progres.boatProgress>50f && v_Progres.boatProgress <= 75f)
                {
                    v_TimeBeforeMouetteSpawn = v_TimeBeforeMouetteSpawnMax50;
                }

                else if (v_Progres.boatProgress > 75f)
                {
                    v_TimeBeforeMouetteSpawn = v_TimeBeforeMouetteSpawnMax75;
                }

                else
                {
                    v_TimeBeforeMouetteSpawn = v_TimeBeforeMouetteSpawnMax0;
                }
                
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
        v_NumberOfMouetteToSpawn = Mathf.RoundToInt(Random.Range(0, v_NumberOfMouetteToSpawnMax));

        for(int i=0; i<v_NumberOfMouetteToSpawn; i++)
        {
            v_Speed = Mathf.RoundToInt(Random.Range(0, v_SpeedMax));
            v_TimeBeforeActivate = Mathf.RoundToInt(Random.Range(0, v_TimeBeforeActivateMax));
            v_TimeBeforeObjectSpawn = Mathf.RoundToInt(Random.Range(0, v_TimeBeforeObjectSpawnMax));
            GameObject mouette=Instantiate(v_MouettePrefab, transform.position, Quaternion.identity);
            mouette.GetComponent<Mouette>().Initialise(v_Speed, v_TimeBeforeActivate, v_TimeBeforeObjectSpawn);
        }
    }
}
