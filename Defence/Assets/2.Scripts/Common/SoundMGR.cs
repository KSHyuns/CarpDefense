using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SoundMGR : Singleton<SoundMGR>
{
    [Title("사운드 소스")]
    public AudioSource[] audioSource;

    [Title("사운드 딕셔너리")]
    public SoundData soundData;

    public enum playProperty {None ,  Play , Stop }

    public playProperty property = playProperty.Play;

    //0번 BGM
    //1번 SFX

    public void SoundPlay(audioName key)
    {
        if (property == playProperty.Stop) return;

        soundData sData = soundData.SoundDic[key];

        if (sData.property == audioProperty.Bgm)
        {
            if (audioSource[0].isPlaying) audioSource[0].Stop();
                
            audioSource[0].clip = sData.clip;
            audioSource[0].volume = sData.volume;
            audioSource[0].loop = sData.loop;
            audioSource[0].Play();
        }
        else if (sData.property == audioProperty.Sfx)
        {
            audioSource[1].volume = sData.volume;
            audioSource[1].loop = sData.loop;
            audioSource[1].PlayOneShot(sData.clip);
        }
    }

    public void SoundAllStop()
    {
        property = playProperty.Stop;
        audioSource[0].Pause();
        audioSource[1].Stop();
    }

    public void SoundAllPlay()
    {
        property = playProperty.Play;
        audioSource[0].Play();
    }

    public override void Awake()
    {
        base.Awake();
    }


}

public enum audioProperty
{ 
    none , Bgm , Sfx
}

public enum audioName
{
    popup_on,  BungBBang  , death , Draw , Game , hit , Intro , Main , MenuBtn , music_on , reward_btn , selectbung , selectbunng_btn , Shop
}

public struct soundData
{
    public AudioClip clip;
    public audioProperty property;
    public float volume;
    public bool loop;
}
