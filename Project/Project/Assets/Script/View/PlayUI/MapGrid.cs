using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

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

    [SerializeField]
    private GameObject Point;

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
    public void RefreshData(GridData grid, ref Dictionary<int, MapGrid> gridMaps, bool isNew = false, bool showAnimation = true)
    {
        if (gridData == null || true)
        {
            if (isNew)
            {
                if (showAnimation)
                {
                    StartCoroutine(DelayCall(ConfigData.GRID_MOVE_TIME));
                }
                else
                {
                    DelayCall(-1);
                }
            }
            else
            {
                gameObject.SetActive(true);
                //之前的可能需要移动位子，需要判断
                var vec = MapTool.GetPosition(grid.Position.x, grid.Position.y);
                //rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
                rectTransform.DOAnchorPos(new Vector2(vec.x, vec.y), ConfigData.GRID_MOVE_TIME);
                if (grid.MergeID > 0 && gridMaps.ContainsKey(grid.MergeID))
                {
                    var gridItem = gridMaps[grid.MergeID];
                    gridItem.DoTweenAndDestroy(new Vector2(vec.x, vec.y));
                    gridMaps.Remove(grid.MergeID);
                    //Debug.Log("删除的:" + grid.MergeID);
                }
            }
            //text.text = ((1 << grid.Ladder) + " " + grid.ID).ToString();
            text.text = (1 << grid.Ladder).ToString();
            gridData = grid;
        }
    }

    private IEnumerator DelayCall(float time)
    {
        if (Point != null)
        {
            Point.SetActive(false);
        }
        if (time > 0)
        {
            yield return new WaitForSeconds(time);
        }
        if (Point != null)
        {
            Point.SetActive(true);
            //新的直接展示出来就行
            var vec = MapTool.GetPosition(gridData.Position.x, gridData.Position.y);
            rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
        }
    }


    public void DoTweenAndDestroy(Vector3 position)
    {
        if (rectTransform != null)
        {
            rectTransform.DOAnchorPos(position, ConfigData.GRID_MOVE_TIME).onComplete += () =>
            {
                MyDestroy();
            };
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