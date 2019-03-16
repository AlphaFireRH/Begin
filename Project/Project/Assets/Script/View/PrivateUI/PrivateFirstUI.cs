﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateFirstUI : UIViewBase
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickClose()
    {
        Close();
    }

    public void OnClickOk()
    {
        AdController.Instance.SetUserPrivateChoose(true);
        Close();
    }

    public void OnClickTerms()
    {
        UIManager.Instance.ShowUI(ViewID.WebViewAgreeUI);

        Close();
    }

    private void Close()
    {
        UIManager.Instance.CloseUI(this);
    }
}
