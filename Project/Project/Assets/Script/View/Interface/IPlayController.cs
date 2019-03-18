using System;
using System.Collections.Generic;

public interface IPlayController
{
    /// <summary>
    /// 地图大小
    /// </summary>
    /// <returns></returns>
    int MapSize();

    /// <summary>
    /// 是否结束
    /// </summary>
    /// <returns></returns>
    bool IsEnd();

    /// <summary>
    /// Resister the specified playUIController.
    /// </summary>
    /// <param name="playUIController">Play UIC ontroller.</param>
    void Resister(IPlayUIController playUIController);

    /// <summary>
    /// 移动方向
    /// </summary>
    /// <param name="md"></param>
    /// <returns></returns>
    MapData Move(PlayerOperate md);

    /// <summary>
    /// 是否暂停
    /// </summary>
    /// <param name="isPause"></param>
    void Pause(bool isPause);

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="size"></param>
    void StartGame(int size = 4);

    /// <summary>
    /// 结束游戏
    /// </summary>
    void EndGame();

    /// <summary>
    /// 获取地图数据
    /// </summary>
    /// <returns></returns>
    MapData GetMapDatas();

    /// <summary>
    /// 移动地图
    /// </summary>
    /// <returns></returns>
    void UseBoom(Action<MapData> action);

    /// <summary>
    /// 能否使用炸弹
    /// </summary>
    /// <returns></returns>
    bool IsCanUseBoom();

    /// <summary>
    /// 能否使用goback
    /// </summary>
    /// <returns></returns>
    bool IsCanUseGoBack();

    /// <summary>
    /// 使用退步道具
    /// </summary>
    /// <returns></returns>
    void UseGoBack(Action<MapData> action);

    /// <summary>
    /// 获取存档数据
    /// </summary>
    /// <returns></returns>
    MapData GetCurSaveData();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<MapData> GetCurSaveDatas();
}