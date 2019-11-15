using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer v_AudioMixer;
    public AudioMixerGroup v_SFX;
    public AudioMixerGroup v_Music;
    public Slider masterVolume;
    public Slider sfVolume;
    public Slider muVolume;


    private void Start()
    {
        masterVolume.onValueChanged.AddListener(delegate { SetVolume(); });
        muVolume.onValueChanged.AddListener(delegate { SetMusic(); });
        sfVolume.onValueChanged.AddListener(delegate { SetSFX(); });
    }

    public void SetVolume()
    {
        v_AudioMixer.SetFloat("volume", masterVolume.value);
    }

    public void SetSFX()
    {
        v_SFX.audioMixer.SetFloat("sfVolume", sfVolume.value);
    }

    public void SetMusic()
    {
        v_Music.audioMixer.SetFloat("muVolume", muVolume.value);
    }
}
