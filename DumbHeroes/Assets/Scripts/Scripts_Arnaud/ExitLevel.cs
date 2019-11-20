using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ExitLevel : ActivableObjects
{
    [SerializeField] LayerMask mask;
    List<PlayerController> players = new List<PlayerController>();

    SpriteRenderer sRenderer;
    public Sprite v_OpenSprite; // Uniquement tant qu'on a pas de niveaux venant après celui ci
    Sprite v_origColor;
    public string v_NextLevelName; // Uniquement quand on a un niveau venant après ou un écran de fin
    bool v_isActive = false;
    private void Awake()
    {
        
        sRenderer = GetComponent<SpriteRenderer>();

        v_origColor=sRenderer.sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (v_isActive)
        {
           
            if (players.Count >= 1)
            {
                SceneManager.LoadScene(v_NextLevelName);
                sRenderer.sprite = v_OpenSprite;
            }
        }
        
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            PlayerController player = collision.GetComponent<PlayerController>();


            if(!CheckIfAlreadyInList(player))
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
