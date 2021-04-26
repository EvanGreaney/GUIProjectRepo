using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    //method that allows the user to control the volume accurately using a slider
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue)* 20);
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
