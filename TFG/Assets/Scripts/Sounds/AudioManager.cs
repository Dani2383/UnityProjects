using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] music;
    public Sound[] footSteps;
    public Slider sliderEffects, sliderMusic, sliderSteps;
    public float sliderValueEffects, sliderValueMusic, sliderValueSteps;

    public static AudioManager instance;
    void Awake()
    {

        if(instance == null) { 
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in music) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in footSteps) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        sliderValueEffects = 0.5f;
        sliderValueMusic = 0.5f;
        sliderValueSteps = 0.5f;

    }

    void Update(){
        if(sliderEffects == null){
            try{
                sliderEffects = GameObject.FindWithTag("SliderEffects").GetComponent<Slider>();
                sliderEffects.value = sliderValueEffects;
                sliderEffects.onValueChanged.AddListener(delegate { SetVolumeEffects();});
            } catch(Exception e){};
        }

        if (sliderMusic == null) {
            try {
                sliderMusic = GameObject.FindWithTag("SliderMusic").GetComponent<Slider>();
                sliderMusic.value = sliderValueMusic;
                sliderMusic.onValueChanged.AddListener(delegate { SetVolumeMusic(); });
            } catch (Exception e) { };
        }

        if (sliderSteps == null) {
            try {
                sliderSteps = GameObject.FindWithTag("SliderSteps").GetComponent<Slider>();
                sliderSteps.value = sliderValueSteps;
                sliderSteps.onValueChanged.AddListener(delegate { SetVolumeSteps(); });
            } catch (Exception e) { };
        }
    }

    public void SetVolumeEffects(){
        Debug.Log(sliderEffects.value);
        foreach (Sound s in sounds) {
            s.source.volume = sliderEffects.value;
        }
        sliderValueEffects = sliderEffects.value;
    }

    public void SetVolumeMusic() {
        Debug.Log(sliderMusic.value);
        foreach (Sound s in music) {
            s.source.volume = sliderMusic.value;
        }
        sliderValueMusic = sliderMusic.value;
    }

    public void SetVolumeSteps() {
        Debug.Log(sliderSteps.value);
        foreach (Sound s in footSteps) {
            s.source.volume = sliderSteps.value;
        }
        sliderValueSteps = sliderSteps.value;
    }
    public void PlaySound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("�" + name + " no se ha encontrado!");
            return;
        }
        s.source.Play();
    }

    public void StopSound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("�" + name + " no se ha encontrado!");
            return;
        }
        s.source.Stop();
    }

    public void PlayMusic(string name) {
        Sound s = Array.Find(music, music => music.name == name);
        if (s == null) {
            Debug.LogWarning("�" + name + " no se ha encontrado!");
            return;
        }
        s.source.Play();
    }

    public void StopMusic(string name) {
        Sound s = Array.Find(music, music => music.name == name);
        if (s == null) {
            Debug.LogWarning("�" + name + " no se ha encontrado!");
            return;
        }
        s.source.Stop();
    }

    public void PlayFootSteps(string name) {
        Sound s = Array.Find(footSteps, footSteps => footSteps.name == name);
        if (s == null) {
            Debug.LogWarning("�" + name + " no se ha encontrado!");
            return;
        }
        s.source.Play();
    }
}
