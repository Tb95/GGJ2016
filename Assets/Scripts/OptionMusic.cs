using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class OptionMusic : MonoBehaviour {

    public AudioMixer mainMixer;

    public void ChangeMusicVolume(Slider slider)
    {
        mainMixer.SetFloat("musicVol", slider.value);
    }

    public void ChangeSFXVolume(Slider slider)
    {
        mainMixer.SetFloat("sfxVol", slider.value);
    }
}
