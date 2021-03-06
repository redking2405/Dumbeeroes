﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager Instance;
    public AudioSource MenuSound;
    public List<AudioSource> Character1;
    public List<AudioSource> Character2;
    public List<AudioSource> Character3;
    public List<AudioSource> Character4;
    public List<AudioSource> BoatLevel;
    public List<AudioSource> TutorialLevel;
    public List<AudioSource> GeneralSound;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
