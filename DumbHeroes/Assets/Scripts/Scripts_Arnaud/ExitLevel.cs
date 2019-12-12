using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
public class ExitLevel : ActivableObjects
{
    [SerializeField] LayerMask mask;
    List<PlayerController> players = new List<PlayerController>();

    SpriteRenderer sRenderer;
    public Sprite v_OpenSprite; // Uniquement tant qu'on a pas de niveaux venant après celui ci
    Sprite v_origColor;
    public string v_NextLevelName; // Uniquement quand on a un niveau venant après ou un écran de fin
    bool v_isActive = false;
    [SerializeField] int totalPlayers;
    bool trigger = false;
    private void Awake()
    {
        
        sRenderer = GetComponent<SpriteRenderer>();

        v_origColor=sRenderer.sprite;
    }

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
                FadeInOut.Instance.StartCoroutine(FadeInOut.Instance.FadeAndLoadScene(FadeInOut.FadeDirection.In, v_NextLevelName));
                trigger = true;
            }
        }

        else sRenderer.sprite = v_origColor;
        
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
       
            if (collision.gameObject.layer==8)
            {
                PlayerController player = collision.GetComponent<PlayerController>();


                if (!CheckIfAlreadyInList(player))
                {
                    players.Add(player);
                }

            }
       
        
    }

    

    bool CheckIfAlreadyInList(PlayerController pPlayer)
    {
        bool returnValue=false;
        if (players.Count == 0)
        {
            returnValue = false;
            
        }

        if (players.Count >= 1)
        {
            for(int i=0; i<players.Count; i++)
            {
                if (players[i].V_playerId == pPlayer.V_playerId)
                {
                    returnValue = true;
                }
            }
        }

        return returnValue;
    }


    public override void Activate()
    {
        v_isActive = true;   
        base.Activate();
    }

    public override void Deactivate()
    {
        v_isActive = false;
        base.Deactivate();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
            if (collision.gameObject.layer == 8)
            {

                PlayerController player = collision.GetComponent<PlayerController>();

                if (CheckIfAlreadyInList(player))
                {
                    players.Remove(player);
                }
            }
        
        
    }
}
