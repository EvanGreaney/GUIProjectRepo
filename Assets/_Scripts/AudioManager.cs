using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        // checks to see if there is an audio manager in the DontDestroyOnLoad
        // if it exists , keeps it otherwise destroys it
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
           s.source= gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    private void Start()
    {   
        Play("BackgroundMusic");
    }
    // method that when called, plays a sound clip from array of sounds
    public void Play(string name)
    {
      Sound s =  Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
