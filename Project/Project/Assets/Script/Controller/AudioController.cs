using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : SingleMono<AudioController>
{
    public bool soundState = true;


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

                }
                break;
            case AudioType.merge:
                {

                }
                break;
            case AudioType.move:
                {

                }
                break;
            default:
                break;
        }
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