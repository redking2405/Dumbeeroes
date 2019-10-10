using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : ActivableObjects
{
    [SerializeField] GameObject v_ObjectToSpawn;
    [SerializeField] float v_TimeBetweenSpawn; //in seconds
    public bool canSpawn;
    bool hasSpawned;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            if (hasSpawned)
            {
                timer += Time.deltaTime;

                if (timer >= v_TimeBetweenSpawn)
                {
                    hasSpawned = false;
                    timer = 0;
                }
            }

            if (!hasSpawned)
            {
                Spawn();
            }
        }
    }


    void Spawn()
    {
        Instantiate(v_ObjectToSpawn, transform.position, Quaternion.identity);
        hasSpawned = true;
    }

    public override void Activate()
    {
        canSpawn = true;
        base.Activate();
    }

    public override void Deactivate()
    {
        canSpawn = false;
        timer = 0;
        base.Deactivate();
    }
}
