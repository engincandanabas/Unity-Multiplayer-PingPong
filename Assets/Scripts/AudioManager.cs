using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Sound[] sounds;
    private AudioSource[] source;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        AudioSourceAdd();
        PlaySound("Music");
    }

    public void AudioSourceAdd()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = this.gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.playOnAwake = false;
            sounds[i].source.loop = sounds[i].loop;
        }
    }
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found!");
        }
        else
        {
            if (Array.Exists(sounds, element => element.name == name))
                Array.Find(sounds, sound => sound.name == name).source.Play();
        }
    }
    public void ToggleSound()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source.volume = (sounds[i].source.volume==1) ? 0 : 1;
        }
        //UIManager.Instance.ChangeSoundIcon((sounds[0].source.volume == 1));
    }
}



[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioSource source;
    public bool loop;
}
