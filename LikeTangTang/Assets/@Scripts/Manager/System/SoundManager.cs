using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager
{
    private AudioSource[] audioSources = new AudioSource[(int)Define.Sound.Max];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    GameObject soundRoot = null;
    public void Init()
    {
        if(soundRoot == null)
        {
            soundRoot = GameObject.Find("@SoundRoot");
            if(soundRoot == null)
            {
                soundRoot = new GameObject { name = "@SoundRoot" };
                UnityEngine.Object.DontDestroyOnLoad(soundRoot);

                string[] soundTypeNames = Enum.GetNames(typeof(Define.Sound));
                for(int count = 0; count < soundTypeNames.Length-1; count++)
                {
                    GameObject go = new GameObject { name = soundTypeNames[count] };
                    audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = soundRoot.transform;
                }
                audioSources[(int)Define.Sound.Bgm].loop = true;
                audioSources[(int)Define.Sound.SubBgm].loop = true;

            }
        }
    }
    public void Clear()
    {
        foreach (AudioSource audio in audioSources)
            audio.Stop();

        audioClips.Clear();
    }

    public void Play(Define.Sound _sound, string _label, float _pitch = 1f)
    {
        AudioSource audio = audioSources[(int)_sound];

        if(_sound == Define.Sound.Bgm)
        {
            LoadAudioClip(_label, (audioClip) =>
            {
                if (audio.isPlaying)
                    audio.Stop();

                audio.clip = audioClip;
                if (Manager.GameM.BGMOn)
                    audio.Play();
            });
        }
        else if(_sound == Define.Sound.SubBgm)
        {
            LoadAudioClip(_label, (audioClip) =>
            {
                if (audio.isPlaying)
                    audio.Stop();

                audio.clip = audioClip;
                if (Manager.GameM.EffectSoundOn)
                    audio.Play();
            });
        }
        else
        {
            LoadAudioClip(_label, (audioClip) =>
            {
                audio.pitch = _pitch;
                if (Manager.GameM.EffectSoundOn)
                    audio.PlayOneShot(audioClip);
            });
        }
    }

    public void PlayButtonClick()
    {//TODO : 다 하고 고치자
        Play(Define.Sound.Effect, "ButtonClick");
    }

    public void PlayPopupClose()
    {
        Play(Define.Sound.Effect, "PopupClose");
    }

    
    public void Stop(Define.Sound _sound)
    {
        AudioSource audio = audioSources[(int)_sound];
        audio.Stop();
    }


    public void LoadAudioClip(string _key, Action<AudioClip> _callback)
    {
        AudioClip audioClip = null;
        if(audioClips.TryGetValue(_key, out audioClip))
        {
            _callback?.Invoke(audioClip);
            return;
        }

        audioClip = Manager.ResourceM.Load<AudioClip>(_key);
        if (!audioClips.ContainsKey(_key))
            audioClips.Add(_key, audioClip);

        _callback?.Invoke(audioClip);
    }
}
