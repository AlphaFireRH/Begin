using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapTool
{
    private static int mapSize = 4;

    /// <summary>
    /// 设置地图大小
    /// </summary>
    /// <param name="mapSize"></param>
    public static void SetMapSize(int curMapSize = 4)
    {
        mapSize = curMapSize;
    }

    /// <summary>
    /// 获取坐标
    /// </summary>
    /// <returns>The position.</returns>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public static Vector2 GetPosition(int x, int y)
    {
        Vector2 ret = new Vector2();
        ret.x = (x - mapSize / 2 - 0.5f) * ConfigData.MAP_UI_SIZE;
        ret.y = (y - mapSize / 2 - 0.5f) * ConfigData.MAP_UI_SIZE;
        return ret;
    }
}
