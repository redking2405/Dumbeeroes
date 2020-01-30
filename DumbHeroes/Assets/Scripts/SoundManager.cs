using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;



    public List<AudioSource> musicList;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        PlayMusic(SceneManager.GetActiveScene().name);
    }


    void PlayMusic(string SceneName)
    {
        switch (SceneName)
        {
            case ("0_x Levels"):
                musicList[0].Play();
                break;
            case ("1_0 Throw Friends"):
                musicList[0].Play();
                break;
            case ("2_0 Basketball"):
                musicList[1].Play();
                break;
            case ("3_0 Levels"):
                musicList[1].Play();
                break;
            case ("3_1 Levels"):
                musicList[1].Play();
                break;
            case ("3_2 Levels"):
                musicList[1].Play();
                break;
            case ("3_3 Levels"):
                musicList[1].Play();
                break;
                /*case ("4_0 I should buy a boat"):
                    musicList[1].Play();
                    break;*/


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
