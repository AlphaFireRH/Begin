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
    #region 组件
    /// <summary>
    /// recttransform
    /// </summary>
    [SerializeField]
    private RectTransform rectTransform;

    /// <summary>
    /// 显示文字
    /// </summary>
    [SerializeField]
    private Text text;

    /// <summary>
    /// 挂点
    /// </summary>
    [SerializeField]
    private GameObject point;
    #endregion

    #region 数据
    /// <summary>
    /// The grid.
    /// </summary>
    private GridData gridData;

    /// <summary>
    /// 是否正在播放动画
    /// </summary>
    private bool isPlayAnimation;
    #endregion

    #region 刷新数据
    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="grid">当前数据</param>
    /// <param name="gridMaps">所有地块</param>
    /// <param name="isNew">是否是新的</param>
    /// <param name="showAnimation">是否有动画</param>
    public void RefreshData(GridData grid, ref Dictionary<int, MapGrid> gridMaps, bool isNew = false, bool showAnimation = true)
    {
        if (gridData == null || true)
        {
            gridData = grid;
            if (isNew)
            {
                if (showAnimation)
                {
                    StartCoroutine(DelayCall(ConfigData.GRID_MOVE_TIME));
                }
                else
                {
                    //新的直接展示出来就行
                    var vec = MapTool.GetPosition(grid.Position.x, grid.Position.y);
                    rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
                    RefreshData();
                }
            }
            else
            {
                if (showAnimation)
                {
                    gameObject.SetActive(true);
                    //之前的可能需要移动位子，需要判断
                    var vec = MapTool.GetPosition(grid.Position.x, grid.Position.y);
                    if (showAnimation)
                    {
                        isPlayAnimation = true;
                        rectTransform.DOAnchorPos(new Vector2(vec.x, vec.y), ConfigData.GRID_MOVE_TIME).onComplete +=
                        () =>
                        {
                            isPlayAnimation = false;
                            RefreshData();
                        };
                        if (grid.MergeID > 0 && gridMaps.ContainsKey(grid.MergeID))
                        {
                            var gridItem = gridMaps[grid.MergeID];
                            gridItem.DoTweenAndDestroy(new Vector2(vec.x, vec.y));
                            gridMaps.Remove(grid.MergeID);
                        }
                    }
                    else
                    {
                        rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
                    }
                }
                else
                {
                    var vec = MapTool.GetPosition(grid.Position.x, grid.Position.y);
                    rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
                    gridData = grid;
                    RefreshData();
                }
            }
        }
    }

    /// <summary>
    /// 刷新显示数据
    /// </summary>
    private void RefreshData()
    {
        if (gridData != null)
        {
            text.text = (1 << gridData.Ladder).ToString();
        }
    }
    #endregion


    #region 动画相关
    /// <summary>
    /// 延迟播放展示动画
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator DelayCall(float time)
    {
        isPlayAnimation = true;
        if (point != null)
        {
            point.SetActive(false);
        }
        if (time > 0)
        {
            yield return new WaitForSeconds(time);
        }
        if (point != null)
        {
            point.SetActive(true);
            point.transform.localScale = Vector3.zero;
            point.transform.DOScale(Vector3.one, ConfigData.GRID_SHOW_TIME).onComplete += () =>
            {
                RefreshData();
            };
            //新的直接展示出来就行
            var vec = MapTool.GetPosition(gridData.Position.x, gridData.Position.y);
            rectTransform.anchoredPosition = new Vector2(vec.x, vec.y);
        }
        isPlayAnimation = false;
    }

    /// <summary>
    /// 播放移动动画并销毁
    /// </summary>
    /// <param name="position"></param>
    public void DoTweenAndDestroy(Vector3 position)
    {
        if (rectTransform != null)
        {
            isPlayAnimation = true;
            rectTransform.DOAnchorPos(position, ConfigData.GRID_MOVE_TIME).onComplete += () =>
            {
                isPlayAnimation = false;
                MyDestroy();
            };
        }
    }

    /// <summary>
    /// 销毁动画
    /// </summary>
    public void DoDestroy()
    {
        point.transform.localScale = Vector3.one;
        point.transform.DOScale(Vector3.zero, ConfigData.GRID_SHOW_TIME).onComplete += () =>
        {
            MyDestroy();
        };
    }
    #endregion

    #region 其他
    /// <summary>
    /// 是否正在播放动画
    /// </summary>
    /// <returns></returns>
    public bool IsPlayAnimation()
    {
        return isPlayAnimation;
    }

    /// <summary>
    /// 获取展示数据
    /// </summary>
    /// <returns></returns>
    public GridData GetGridData()
    {
        return gridData;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void MyDestroy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
    #endregion

}