using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterSpawn : MonoBehaviour
{
    public List<Player> PlayerToSpawnList= new List<Player>();
    public GameObject[] PrefabPlayer;
    

    // Start is called before the first frame update
    void Start()
    {
        AddToPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddToPlayers()
    {
        foreach (Player p in ReInput.players.AllPlayers)
        {
            // !PlayerToSpawnList.Contains(p) /*&& p.GetAnyButton()*/
            if (p.controllers.joystickCount > 0)
            {
                PlayerToSpawnList.Add(p);
                SpawnPlayer(p);
            }
        }
        
    }

    void SpawnPlayer(Player PlayerToSpawn)
    {
        GameObject PlayerObject = Instantiate(PrefabPlayer[PlayerToSpawn.id], transform.GetChild(PlayerToSpawn.id).position , Quaternion.identity);
        PlayerObject.GetComponentInChildren<PlayerController>().V_playerId = PlayerToSpawn.id;
    }
}
