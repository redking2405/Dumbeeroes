using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    /*[SerializeField] LayerMask mask;
    List<PlayerController> players = new List<PlayerController>();
    SpriteRenderer sRenderer;
    public Color D_FinishColor; // Uniquement tant qu'on a pas de niveaux venant après celui ci
    Color v_origColor;
    public string v_NextLevelName; // Uniquement quand on a un niveau venant après ou un écran de fin
    private void Awake()
    {
        
        sRenderer = GetComponent<SpriteRenderer>();

        v_origColor=sRenderer.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(v_NextLevelName);
        if (players.Count >= 1)
        {

            sRenderer.color = D_FinishColor;
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
    }*/
}
