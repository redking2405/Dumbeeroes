using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterSpawn : MonoBehaviour
{
    public List<Player> PlayerToSpawnList= new List<Player>();
    public GameObject PrefabPlayer;
    public Transform SpawnerPosition;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AddToPlayers();

    }

    void AddToPlayers()
    {
        foreach (Player p in ReInput.players.AllPlayers)
        {
            if (!PlayerToSpawnList.Contains(p) && p.GetAnyButton())
            {
                PlayerToSpawnList.Add(p);
                SpawnPlayer(p);
            }
        }
        
    }

    void SpawnPlayer(Player PlayerToSpawn)
    {
        GameObject PlayerObject = Instantiate(PrefabPlayer, SpawnerPosition.position, Quaternion.identity);
        Debug.LogWarning("! player Controler ref temporarily disabeled for tests !");
        //PlayerObject.GetComponent<PlayerController>().V_playerId = PlayerToSpawn.id;
    }
}
