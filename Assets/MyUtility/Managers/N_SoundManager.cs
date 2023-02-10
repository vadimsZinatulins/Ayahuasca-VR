using System;
using System.Collections;
using System.Collections.Generic;
using MyUtility.ObjectPool;
using UnityEngine;
using Utility;

public class N_SoundManager : MonoBehaviour
{
    public static N_SoundManager instance;
    [SerializeField] private N_ObjectPool audioPool;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    #region AudioPoolFunctions

    public AudioSource GetAudioSource(AudioClip clip, bool spatial3D = true)
    {
        if (clip != null)
        {
            var audioGO = audioPool.GetObject();
            AudioSource audio = audioGO.GetComponent<AudioSource>();
            audio.clip = clip;
            audio.spatialBlend = (spatial3D) ? 1.0f : 0.0f;
            audioGO.name = $"AS_VFX{clip.name.ToUpperInvariant().Replace(" ", "")}";
            return audio;
        }

        return null;
    }

    public AudioSource GetAudioSource(AudioClip clip, float time, bool spatial3D = true)
    {
        var a = GetAudioSource(clip, spatial3D);
        if (a)
        {
            PoolTimer timer = a.gameObject.AddComponent<PoolTimer>();
            timer.StartTimer(time);
        }

        return a;
    }

    #endregion
}