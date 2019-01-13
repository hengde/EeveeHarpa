using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private List<AudioSource> effectChannels;
    private GameObject effectsHolder;
    private int effectChannelSize = 100;
    private int effectChannelIndex = 0;
    private TaskManager _tm;
    private AudioManager instance;
    private AudioSource _burbling, _audioBed;
    
    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        effectsHolder = new GameObject("Effect Tracks");
 
        effectsHolder.transform.parent = transform;

        effectChannels = new List<AudioSource>();

        for (int i = 0; i < effectChannelSize; i++)
        {
            GameObject channel = new GameObject("Effect Channel " + i);
            channel.transform.parent = effectsHolder.transform;
            
            effectChannels.Add(channel.AddComponent<AudioSource>());
            effectChannels[i].loop = false;
        }
    }

    private void Start()
    {
        _tm = new TaskManager();
        
        var burblingGO = new GameObject("Burbling Audio Bed");
        burblingGO.transform.parent = transform;
        _burbling = burblingGO.AddComponent<AudioSource>();
        _burbling.clip = Services.Clips.Burbles;
        _burbling.loop = true;
        _burbling.volume = 0.45f;
        _burbling.Play();
        
        var audioBedGO = new GameObject("Audio Bed");
        audioBedGO.transform.parent = transform;
        _audioBed = audioBedGO.AddComponent<AudioSource>();
        _audioBed.clip = Services.Clips.AudioBed;
        _audioBed.loop = true;
        _burbling.volume = 0.25f;
        //_audioBed.Play();
    }
    
    private void Update()
    {
        _tm.Update();
    }
    
    private System.Action _ParameterizeAction(System.Action<AudioClip, float> function, AudioClip clip, float volume)
    {
        System.Action to_return = () =>
        {
            function(clip, volume);
        };
        
        return to_return;
    }

    public void PlaySoundEffect(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        AudioSource to_play = effectChannels[effectChannelIndex];
        effectChannelIndex = (effectChannelIndex + 1) % effectChannelSize;

        if (to_play.isPlaying)
        {
            GameObject channel = new GameObject("Effect Channel");
            channel.transform.parent = effectsHolder.transform;
            effectChannels.Insert(effectChannelIndex, channel.AddComponent<AudioSource>());
            effectChannels[effectChannelIndex].loop = false;

            to_play = effectChannels[effectChannelIndex];
            effectChannelIndex = (effectChannelIndex + 1) % effectChannelSize;
        }

        to_play.volume = volume;
        to_play.clip = clip;
        to_play.pitch = pitch;
        to_play.Play();
    }

    IEnumerator YieldForSync(System.Action callback, float delayTime)
    {
        float timeElapsed = 0.0f;
        bool waiting = true;
        while (waiting)
        {
            timeElapsed += Time.deltaTime;
            
            if (timeElapsed > delayTime)
                waiting = false;
            else
                yield return false;
        }
        callback();
    }
/*
    public void SlowDownMainTrack()
    {
        StartCoroutine(Coroutines.DoOverEasedTime(Services.Clock.QuarterLength(), Easing.QuadEaseOut,
            t =>
            {
                mainLoopSource.pitch = Mathf.Lerp(1.0f, 0.3f, t);
            }));
    }
*/
}
