using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteUI : UIViewBase
{
    public Text scoreLabel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(ViewData viewData)
    {
        clickLock = false;
        scoreLabel.text = GameController.Instance.CurScore4Show();
    }

    private void Close()
    {
        UIManager.Instance.CloseUI(this);
    }

    public void OnClickAgain()
    {
        GameController.Instance.StartGame();
        Close();
    }

    private bool clickLock = false;
    public void OnClickContinue()
    {
        clickLock = true;
        AdController.Instance.ShowRewardVideoAd((int value) =>
        {
            clickLock = false;
            if (value == 1)
            {
                GameController.Instance.Continue();
                Close();
            }
        });
    }
}
