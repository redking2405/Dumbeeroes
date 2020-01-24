using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ExitBougeCam : ExitLevel
{

    public Transform[] startingPosPlayers;
    public Transform CameraPos;
    public GameObject[] PrefabPlayer;
    public static List<PlayerData> PlayerSpawnedList = new List<PlayerData>();
    // Start is called before the first frame update
    void Start()
    {
        totalPlayers = CharacterSpawn.PlayerSpawnedList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (v_isActive)
        {
            sRenderer.sprite = v_OpenSprite;
            SFXManager.Instance.TutorialLevel[3].Play();
            if (players.Count >= totalPlayers && !trigger)
            {
                FadeInOut.Instance.StartCoroutine(FadeInOut.Instance.FadeAndBougeCam(FadeInOut.FadeDirection.In, CameraPos));

                Invoke("PlacePlayers", 0.7f);

                trigger = true;
            }
        }

        else sRenderer.sprite = v_origColor;
    }

    void PlacePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            rbds[i].velocity = Vector2.zero;
            Destroy(players[i].gameObject);
           
        }
        StartCoroutine(RestartPlayers());
    }

    IEnumerator RestartPlayers()
    {
        yield return new WaitForSeconds(0.5f);
        AddToPlayers();
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
        GameObject PlayerObject = Instantiate(PrefabPlayer[PlayerToSpawn.id], startingPosPlayers[PlayerToSpawn.id].position, Quaternion.identity);
        PlayerController pc = PlayerObject.GetComponentInChildren<PlayerController>();


        pc.V_playerId = PlayerToSpawn.id;
        PlayerSpawnedList.Add(new PlayerData(PlayerToSpawn, pc.gameObject));
    }

    private void OnDestroy()
    {
        PlayerSpawnedList.Clear();
    }
}
