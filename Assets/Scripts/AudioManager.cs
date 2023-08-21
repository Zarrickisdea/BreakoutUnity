using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private SoundType[] background, effects;
    [SerializeField] private AudioSource[] sources;

    [Serializable]
    public class SoundType
    {
        public BackgroundSound bgm;
        public Effects sfx;
        public AudioClip clip;
    }

    public enum BackgroundSound
    {
        None,
        Menu,
        Gameplay,
        Over
    }

    public enum Effects
    {
        None,
        Hit,
        Bounce,
        Button
    }

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
    }

    public void PlayEffect(Effects effect)
    {
        AudioClip clip = getSoundClip(effect);
        if (clip != null)
        {
            sources[0].PlayOneShot(clip);
        }
    }

    public void PlayOtherEffect(Effects effect)
    {
        AudioClip clip = getSoundClip(effect);
        if (clip != null)
        {
            sources[1].PlayOneShot(clip);
        }
    }

    public void PlayBGM(BackgroundSound sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            sources[2].clip = clip;
            sources[2].Play();
        }
    }

    public void StopMusic()
    {
        sources[2].Stop();
    }

    private AudioClip getSoundClip(Enum soundType)
    {
        if (soundType is BackgroundSound)
        {
            BackgroundSound bgmType = (BackgroundSound)soundType;
            SoundType bgmItem = Array.Find(background, item => item.bgm == bgmType);
            if (bgmItem != null)
            {
                return bgmItem.clip;
            }
        }
        else if (soundType is Effects)
        {
            Effects sfxType = (Effects)soundType;
            SoundType sfxItem = Array.Find(effects, item => item.sfx == sfxType);
            if (sfxItem != null)
            {
                return sfxItem.clip;
            }
        }

        return null;
    }

}

