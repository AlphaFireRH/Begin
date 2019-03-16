﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : UIViewBase
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickMusic()
    {
        
    }

    public void OnClickSound()
    {

    }

    public void OnClickAgain()
    {
        GameController.Instance.StartGame();
    }

    public void OnClickContinue()
    {

    }

    public void OnClickPrivate()
    {
        UIManager.Instance.ShowUI(ViewID.PrivateUI);
        Close();
    }

    public void OnClickMyData()
    {
        UIManager.Instance.ShowUI(ViewID.WebViewUI);
        Close();
    }

    private void Close()
    {
        UIManager.Instance.CloseUI(this);
    }
}