using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigData
{
    #region UI相关
    /// <summary>
    /// 地图UI像素大小
    /// </summary>
    public const float MAP_UI_SIZE = 165f;
    #endregion

    #region 动画相关
    /// <summary>
    /// 块移动时间
    /// </summary>
    public const float GRID_MOVE_TIME = 0.2f;

    /// <summary>
    /// 动画块缩放时间
    /// </summary>
    public const float GRID_SHOW_TIME = 0.1f;
    #endregion

    #region 玩法相关
    /// <summary>
    /// 炸弹最少需要的块数量
    /// </summary>
    public const int BOOM_MIN_GRID = 4;

    /// <summary>
    /// 炸弹爆的块数
    /// </summary>
    public const int BOOM_GRID_COUNT = 2;

    /// <summary>
    /// The conat.
    /// </summary>
    public const int RECORD_COUNT = 10;

    /// <summary>
    /// 地图大小
    /// </summary>
    public const int MAP_SIZE = 4;
    #endregion

    #region 存档相关


    /// <summary>
    /// 
    /// </summary>
    public const int DEFAULT_BOOM_COUNT = 3;

    /// <summary>
    /// 
    /// </summary>
    public const int DEFAULT_GOBACK_COUNT = 3;

    /// <summary>
    /// 存档路径
    /// </summary>
    public const string SAVE_FILE_PATH = "/Save.txt";

    #endregion
}