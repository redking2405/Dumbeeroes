using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenoController : MonoBehaviour
{
    public string v_SkinSelectorScene;
    public string v_OptionScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch()
    {
        SceneManager.LoadScene(v_SkinSelectorScene);
    }

    public void Option()
    {
        SceneManager.LoadScene(v_OptionScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
