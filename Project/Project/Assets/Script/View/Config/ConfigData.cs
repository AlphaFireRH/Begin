using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigData
{
    #region UI相关

    /// <summary>
    /// 地图大小
    /// </summary>
    public const float MAP_SIZE = 165f;
    #endregion

    #region 动画相关

    /// <summary>
    /// 块移动时间
    /// </summary>
    public const float GRID_MOVE_TIME = 0.2f;

    /// <summary>
    /// 
    /// </summary>
    public const float GRID_SHOW_TIME = 0.1f;
    #endregion

    #region 玩法相关
    /// <summary>
    /// 炸弹最少需要的块数量
    /// </summary>
    public const int BoomMinGrid = 4;

    /// <summary>
    /// 炸弹爆的块数
    /// </summary>
    public const int BoomGridCount = 2;
    #endregion
}