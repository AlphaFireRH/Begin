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

    public void OnClickTrems()
    {
        GameController.Instance.StartGame();
    }

    public void OnClickOK()
    {

    }
}
