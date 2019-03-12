using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 地块
/// </summary>
public class MapGrid : MonoBehaviour
{


    /// <summary>
    /// recttransform
    /// </summary>
    [SerializeField]
    private RectTransform rectTransform;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private Text text;

    /// <summary>
    /// 地图容量
    /// </summary>
    private int mapSize;

    /// <summary>
    /// The grid.
    /// </summary>
    private GridData gridData;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="mapSize">Map size.</param>
    public void Init(int mapSize)
    {
        this.mapSize = mapSize;
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="grid">Grid.</param>
    /// <param name="gridMaps">Grid maps.</param>
    public void RefreshData(GridData grid, Dictionary<int, MapGrid> gridMaps, bool isNew = false)
    {
        if (gridData == null || true)
        {
            if (isNew)
            {
                //新的直接展示出来就行
                var vec = MapTool.GetPosition(grid.Position.x, grid.Position.y);
                rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
            }
            else
            {
                //之前的可能需要移动位子，需要判断
                var vec = MapTool.GetPosition(grid.Position.x, grid.Position.y);
                rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
                if (grid.MergeID > 0 && gridMaps.ContainsKey(grid.MergeID))
                {
                    var gridItem = gridMaps[grid.MergeID];
                    gridItem.MyDestroy();
                    gridMaps.Remove(grid.MergeID);
                }
            }
            text.text = (1 << grid.Ladder).ToString();
            gridData = grid;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void MyDestroy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}