using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioClip> SfxClip;
    public List<AudioClip> BgmLevel;
    public List<AudioClip> AmbLevel;

    [Header("components")]
    public AudioSource SfxSource;
    public AudioSource BgmSource;
    public AudioSource AmbSource;

    public AudioMixer Master;

    /// <summary>
    /// 0 click, 1 ready, 2 heal, 3 attacking, 4 dying
    /// </summary>
    /// <param name="index"></param>
    public void PlaySfx(int index)
    {
        SfxSource.PlayOneShot(SfxClip[index]);
    }

    public void PlayBgmLevel(int index)
    {
        BgmSource.clip = BgmLevel[index];
        BgmSource.Play();
    }

    public void PlayAmb(int index)
    {
        AmbSource.clip = AmbLevel[index];
        AmbSource.Play();
    }

    public void StopAmb()
    {
        AmbSource.Stop();
    }

    public void SetSfxVolume(float volume)
    {
        var vol = (1 - Mathf.Sqrt(volume)) * -80f;
        Master.SetFloat("SFX", vol);
    }

    public void SetBgmVolume(float volume)
    {
        var vol = (1 - Mathf.Sqrt(volume)) * -80f;
        Master.SetFloat("BGM", vol);
    }

    public void SetAmbVolume(float volume)
    {
        var vol = (1 - Mathf.Sqrt(volume)) * -80f;
        Master.SetFloat("AMB", vol);
    }

    public void SetSfxState(bool _state)
    {
        Master.SetFloat("SFX", _state ? 0 : -80);
    }

    public void SetBgmState(bool _state)
    {
        Master.SetFloat("BGM", _state ? 0 : -80);
    }
}