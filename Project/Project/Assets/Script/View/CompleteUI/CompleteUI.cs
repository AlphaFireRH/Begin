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
        clickLock = false;
        clickInsertLock = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override  void Init(ViewData viewData)
    {
        base.Init(viewData);
        clickLock = false;
        clickInsertLock = false;
        scoreLabel.text = GameController.Instance.CurScore4Show();
    }

    private bool clickInsertLock = false;
    public void OnClickAgain()
    {
        if (clickInsertLock)
        {
            return;
        }
        AudioController.Instance.PlaySound(AudioType.click);
        clickInsertLock = true;
        AdController.Instance.ShowInsertAd((int value) =>
        {
            clickInsertLock = false;
        });

        GameController.Instance.StartGame();
        Close();
    }

    private bool clickLock = false;
    public void OnClickContinue()
    {
        if (clickLock)
        {
            return;
        }
        AudioController.Instance.PlaySound(AudioType.click);
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
