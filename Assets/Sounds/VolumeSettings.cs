using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{

    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider soundEffectsSlider;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(musicVolumeController);
        soundEffectsSlider.onValueChanged.AddListener(soundEffectsVolumeController);
    }

    public void musicVolumeController(float value)
    {
        mixer.SetFloat("Music", Mathf.Log10(value) * 20);
    }

    public void soundEffectsVolumeController(float value)
    {
        mixer.SetFloat("SoundEffects", Mathf.Log10(value) * 20);

    }



}
