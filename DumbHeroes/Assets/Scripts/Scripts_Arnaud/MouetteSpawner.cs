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
    // Start is called before the first frame update
    void Start()
    {
        //SpawnMouette();
    }

    // Update is called once per frame
    void Update()
    {
        //Test Debug

        
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
