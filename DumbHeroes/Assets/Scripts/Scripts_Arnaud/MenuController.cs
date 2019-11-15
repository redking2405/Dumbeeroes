using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuController : MonoBehaviour
{
    public static List<MenuController> v_Menus = new List<MenuController>();
    protected Player player;
    public bool v_Ready;
    public bool v_back;
    [SerializeField] protected int playerID;
    public int GetPlayerID()
    {
        return playerID;
    }

    public void SetPlayerID(int newID)
    {
        playerID = newID;
        player = ReInput.players.GetPlayer(playerID);
    }

    protected virtual void Awake()
    {
        v_Menus.Add(this);
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(ReInput.players.GetPlayer(playerID).GetButtonDown("Go"))
        {
            if (!v_Ready)
            {
                Ready();
            }

            else UnReady();
            
        }

        if (ReInput.players.GetPlayer(playerID).GetButtonDown("Back"))
        {
            if (!v_back)
            {
                Backing();
            }

            else UnBacking();
            
        }
    }

    protected virtual void Ready()
    {
        v_Ready = true;
    }

    protected virtual void UnReady()
    {
        v_Ready = false;
    }
    protected virtual void Backing()
    {
        v_back = true;
    }

    protected virtual void UnBacking()
    {
        v_back = false;
    }


    protected virtual void OnDestroy()
    {
        v_Menus.Remove(this);
    }
}
