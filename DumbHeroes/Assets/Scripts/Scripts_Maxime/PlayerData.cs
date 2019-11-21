using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerData
{
    public Player player;
    public GameObject playerObject;

    public PlayerData(Player pl,GameObject po){
        player = pl;
        playerObject = po;
    }

}
