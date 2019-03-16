using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateUI : UIViewBase
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

    public void OnClickBtn0()
    {
        Application.OpenURL("https://youradchoices.com/");
        Close();
    }

    public void OnClickBtn1()
    {
        Application.OpenURL("http://www.networkadvertising.org/");
        Close();
    }

    private void Close()
    {
        UIManager.Instance.CloseUI(this);
    }
}
