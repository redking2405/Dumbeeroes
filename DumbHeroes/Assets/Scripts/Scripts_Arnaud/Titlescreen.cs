using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class Titlescreen : MonoBehaviour
{

    public string v_NextSceneName;
    private int v_NumberOfPlayers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuController.v_Menus[0].v_Ready)
        {
            SceneManager.LoadScene(v_NextSceneName);
        }

        if (MenuController.v_Menus[0].v_back)
        {
            Application.Quit();
        }
    }
}
