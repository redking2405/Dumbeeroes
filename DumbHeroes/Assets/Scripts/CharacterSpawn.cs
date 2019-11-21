using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterSpawn : MonoBehaviour
{
    public static List<PlayerData> PlayerSpawnedList= new List<PlayerData>();
    public GameObject[] PrefabPlayer;
    

    // Start is called before the first frame update
    void Awake()
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
                
                SpawnPlayer(p);
            }
        }
        
    }

    void SpawnPlayer(Player PlayerToSpawn)
    {
        GameObject PlayerObject = Instantiate(PrefabPlayer[PlayerToSpawn.id], transform.GetChild(PlayerToSpawn.id).position , Quaternion.identity);
        PlayerController pc = PlayerObject.GetComponentInChildren<PlayerController>();


        pc.V_playerId = PlayerToSpawn.id;
        PlayerSpawnedList.Add(new PlayerData(PlayerToSpawn, pc.gameObject));
    }

    private void OnDestroy()
    {
        PlayerSpawnedList.Clear();
    }
}
