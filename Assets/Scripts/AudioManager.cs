using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup master;
    string sceneName;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = master;
        }

        if (sceneName == "Main Menu")
        {
            Play("Title", 0);
        }
        else if (sceneName == "Main Game")
        {
            Play("Exploration", 0);
        }
    }

    public void Play(string soundName, float interval)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s != null)
        {
            s.source.Play();
            if (interval > 0)
            {
                StartCoroutine(Repeat(interval, s.source));
            }
        }
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s != null)
        {
            s.source.Stop();
        }
    }

    public void PlayMusic(string soundName)
    {
        StartCoroutine(Music(soundName));
    }

    IEnumerator Music(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        s.source.Play();
        yield return new WaitUntil(() => !s.source.isPlaying);
        StartCoroutine(Music(soundName));
    }

    IEnumerator Repeat(float interval, AudioSource source)
    {
        yield return new WaitForSeconds(interval);
        source.Play();
    }

    bool IsPlaying(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s.source.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}