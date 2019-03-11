using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUI : UIViewBase, IPlayUIController
{
    /// <summary>
    /// 地块预设
    /// </summary>
    [SerializeField]
    private GameObject mapItem;

    /// <summary>
    /// 玩法控制器
    /// </summary>
    private IPlayController playCtrl;

    #region 玩法接口
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init(IPlayController IPlayController)
    {
        playCtrl = IPlayController;
    }
    #endregion

    #region 初始化

    #endregion
}