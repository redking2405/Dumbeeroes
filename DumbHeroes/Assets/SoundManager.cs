using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public List<AudioSource> musicList;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (musicList.Count > 0)
        {
            musicList[0].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
