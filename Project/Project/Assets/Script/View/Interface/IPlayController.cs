using System.Collections.Generic;

public interface IPlayController
{
    /// <summary>
    /// 是否结束
    /// </summary>
    /// <returns></returns>
    bool IsEnd();

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
    /// <param name="md"></param>
    /// <returns></returns>
    MapData UseBoom();

    /// <summary>
    /// 使用退步道具
    /// </summary>
    /// <returns></returns>
    MapData UseGoBack();
}