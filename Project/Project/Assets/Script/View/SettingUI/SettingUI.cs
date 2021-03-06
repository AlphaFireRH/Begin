﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIViewBase
{
    public List<Sprite> centerBg = new List<Sprite>();
    public Image centerBgImage;
    public RectTransform centerObj;
    public Image centerSliderBgImage;
    public RectTransform centerSliderObj;

    // Use this for initialization
    void Start()
    {
        ShowSoundState();
        ShowSilderState();
        clickLock = false;
    }
    void OnEnable()
    {
        clickLock = false;
    }
    // Update is called once per frame
    void Update()
    {

    }

    //public void OnClickMusic()
    //{
    //    AudioController.Instance.soundState = !AudioController.Instance.soundState;
    //}

    public void OnClickSound()
    {
        AudioController.Instance.soundState = !AudioController.Instance.soundState;
        AudioController.Instance.PlaySound(AudioType.click);
        ShowSoundState();
    }


    private void ShowSoundState()
    {
        if (AudioController.Instance.soundState)
        {
            centerBgImage.sprite = centerBg[0];
            centerObj.localPosition = new Vector3(29, -7.5f, 0);
        }
        else
        {
            centerBgImage.sprite = centerBg[1];
            centerObj.localPosition = new Vector3(-29, -7.5f, 0);
        }
    }

    public void OnClickSilder()
    {
        if (GameController.Instance.TouchType == TouchType.Auto)
        {
            GameController.Instance.TouchType = TouchType.Normal;
        }
        else
        {
            GameController.Instance.TouchType = TouchType.Auto;
        }

        AudioController.Instance.PlaySound(AudioType.click);
        ShowSilderState();
    }

    private void ShowSilderState()
    {
        if (GameController.Instance.TouchType == TouchType.Auto)
        {
            centerSliderBgImage.sprite = centerBg[0];
            centerSliderObj.localPosition = new Vector3(29, -7.5f, 0);
        }
        else
        {
            centerSliderBgImage.sprite = centerBg[1];
            centerSliderObj.localPosition = new Vector3(-29, -7.5f, 0);
        }
    }

    private bool clickLock = false;
    public void OnClickAgain()
    {
        if (clickLock)
        {
            return;
        }
        AudioController.Instance.PlaySound(AudioType.click);
        clickLock = true;
        AdController.Instance.ShowInsertAd((int value) =>
        {
            clickLock = false;
        });

        GameController.Instance.StartGame();
        Close();
    }

    public void OnClickContinue()
    {
        AudioController.Instance.PlaySound(AudioType.click);
        Close();
    }

    public void OnClickPrivate()
    {
        AudioController.Instance.PlaySound(AudioType.click);
        UIManager.Instance.ShowUI(ViewID.WebViewUI);
        Close();
    }

    public void OnClickMyData()
    {
        AudioController.Instance.PlaySound(AudioType.click);
        
        UIManager.Instance.ShowUI(ViewID.PrivateUI);
        Close();
    }

    public void OnClickClose()
    {
        AudioController.Instance.PlaySound(AudioType.click);
        Close();
    }

    private void Close()
    {
        UIManager.Instance.CloseUI(this);
    }
}
