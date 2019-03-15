
using UnityEngine;
/// <summary>
/// 地块数据
/// </summary>
public class GridData
{
    public GridData(int x, int y)
    {
        if (Position == null)
        {
            Position = new Vector2Int();
        }
        Position.x = x;
        Position.y = y;

        FromPosition = Position;
    }

    /// <summary>
    /// ID 从1开始(当前布局和上一次布局删除和添加的不会有相同ID)
    /// </summary>
    public int ID;

    /// <summary>
    /// X坐标从1开始 y坐标从1开始
    /// </summary>
    public Vector2Int Position;

    /// <summary>
    /// 阶梯 如果是0则没有数字
    /// </summary>
    public int Ladder = 0;

    /// <summary>
    /// From position.
    /// </summary>
    public Vector2Int FromPosition;

    /// <summary>
    /// 合并的ID(如果是小于等于0则上次没有合并)
    /// </summary>
    public int MergeID = -1;

    /// <summary>
    /// 克隆数据
    /// </summary>
    /// <returns></returns>
    public GridData Clone()
    {
        GridData grid = new GridData(Position.x, Position.y);
        grid.ID = ID;
        grid.Ladder = Ladder;
        grid.MergeID = MergeID;

        return grid;
    }
}