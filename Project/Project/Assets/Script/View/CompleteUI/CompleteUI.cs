using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteUI : UIViewBase
{
    public Text scoreLabel;

    /// <summary>
    /// 
    /// </summary>
    private float timer;

    /// <summary>
    /// 
    /// </summary>
    public const float TIME_CD = 5f;

    // Use this for initialization
    void Start()
    {
        clickLock = false;
        clickInsertLock = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRefreshBtn();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init(ViewData viewData)
    {
        base.Init(viewData);
        clickLock = false;
        clickInsertLock = false;
        scoreLabel.text = GameController.Instance.CurScore4Show();
        RefreshBtnState();
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


    private void UpdateRefreshBtn()
    {
        timer += Time.deltaTime;
        if (timer > ConfigData.REWARD_VIDEO_CHECK_TIME)
        {
            timer = 0;
            RefreshBtnState();
        }
    }
    /// <summary>
    /// 回退按钮
    /// </summary>
    [SerializeField]
    private Image btnBG;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private List<Sprite> btnStateSprite;

    private void RefreshBtnState()
    {
        bool isCanShowRV = AdController.Instance.RewardVideoAdCanShow();
        if (isCanShowRV)
        {
            btnBG.sprite = btnStateSprite[0];
        }
        else
        {
            btnBG.sprite = btnStateSprite[1];
        }
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
