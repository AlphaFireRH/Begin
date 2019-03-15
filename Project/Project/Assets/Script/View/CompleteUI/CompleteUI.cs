using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteUI : UIViewBase
{
    public Text scoreLabel;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(ViewData viewData)
    {
        scoreLabel.text = "";
    }

    public void OnClickAgain()
    {
        GameController.Instance.StartGame();
    }

    public void OnClickContinue()
    {

    }
}
