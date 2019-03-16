using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static AudioController Instance { get { return _instance; } }

    private static AudioController _instance = null;

    public AudioSource soundSource;
    public bool soundState = true;
    public List<AudioClip> clipList = new List<AudioClip>();

    void Awake()
    {
        _instance = this;
    }

    public void PlaySound(AudioType type)
    {
        if (!soundState)
        {
            return;
        }
        switch (type)
        {
            case AudioType.click:
                {
                    AudioPlay(clipList[0]);
                }
                break;
            case AudioType.merge:
                {
                    AudioPlay(clipList[2]);
                }
                break;
            case AudioType.move:
                {
                    AudioPlay(clipList[1]);
                }
                break;
            default:
                break;
        }
    }

    private void AudioPlay(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}

public enum AudioType
{
    /// <summary>
    /// 点击
    /// </summary>
    click,
    /// <summary>
    /// 有合并
    /// </summary>
    merge,
    /// <summary>
    /// 移动（未合并）
    /// </summary>
    move
}