﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : IPlayController
{
    /// <summary>
    /// 界面
    /// </summary>
    private IPlayUIController playUI;

    /// <summary>
    /// 地图历史记录
    /// </summary>
    private List<MapData> mapDatas = new List<MapData>();

    /// <summary>
    /// 当前地图
    /// </summary>
    private MapData curMapData;

    /// <summary>
    /// 地图大小
    /// </summary>
    private int mapSize;

    /// <summary>
    /// 是否暂停
    /// </summary>
    private bool isPause = false;

    /// <summary>
    /// 当前游戏状态
    /// </summary>
    private GameState state = GameState.None;

    #region 接口
    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="size"></param>
    public void StartGame(int size = ConfigData.MAP_SIZE)
    {
        if (playUI != null)
        {
            UIManager.Instance.CloseUI(ViewID.PlayWindow);
            playUI = null;
        }
        mapSize = size;
        SetDefaultMap();

        playUI = UIManager.Instance.ShowUI(ViewID.PlayWindow) as IPlayUIController;
        playUI.Init(this);
        state = GameState.Play;

        MapTool.SetMapSize(size);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="size"></param>
    public void StartGame(MapData mapData, List<MapData> mapDatas, int size = ConfigData.MAP_SIZE)
    {
        if (playUI != null)
        {
            UIManager.Instance.CloseUI(ViewID.PlayWindow);
            playUI = null;
        }
        mapSize = size;
        if (mapData != null)
        {
            curMapData = mapData;
            if (mapDatas != null)
            {
                this.mapDatas = mapDatas;
            }
            else
            {
                this.mapDatas = new List<MapData>();
            }
        }
        else
        {
            SetDefaultMap();
        }

        playUI = UIManager.Instance.ShowUI(ViewID.PlayWindow) as IPlayUIController;
        playUI.Init(this);
        state = GameState.Play;

        MapTool.SetMapSize(size);
        CheckEndState();
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    public void EndGame()
    {
        if (playUI != null)
        {
            state = GameState.None;
            UIManager.Instance.CloseUI(ViewID.PlayWindow);
            playUI = null;
        }
    }

    /// <summary>
    /// 获取地图数据
    /// </summary>
    /// <returns></returns>
    public MapData GetMapDatas()
    {
        if (curMapData == null)
        {
            return null;
        }
        return curMapData.Clone();
    }

    /// <summary>
    /// 是否结束
    /// </summary>
    /// <returns></returns>
    public bool IsEnd()
    {
        return false;
    }

    /// <summary>
    /// 移动地图
    /// </summary>
    /// <param name="md"></param>
    /// <returns></returns>
    public MapData Move(PlayerOperate md)
    {
        if (!isPause && state == GameState.Play)
        {
            MapData mapData = new MapData();
            if (mapDatas == null)
            {
                mapDatas = new List<MapData>();
            }

            mapData = MergeMove(md);
            if (mapData != curMapData)
            {
                RecoradOperate(mapData);
                InsertANumber();
                CheckEndState();
            }
            return curMapData.Clone();
        }
        else
        {
            return curMapData.Clone();
        }
    }

    /// <summary>
    /// 移动地图
    /// </summary>
    /// <param name="md"></param>
    /// <returns></returns>
    public void UseBoom(Action<MapData> action)
    {
        if (IsCanUseBoom())
        {
            if (GameController.Instance.GetItemCount(ItemID.Boom) > 0
#if UNITY_EDITOR
                || true
#endif
                )
            {
                GameController.Instance.UseItem(ItemID.Boom);
                MapData mapData = new MapData();
                if (mapDatas == null)
                {
                    mapDatas = new List<MapData>();
                }
                state = GameState.Play;
                mapData = Boom();
                RecoradOperate(mapData);
                if (action != null)
                {
                    action(curMapData);
                }
            }
            else
            {
                AdController.Instance.ShowRewardVideoAd((int value) =>
                {

                    if (value == 1)
                    {
                        MapData mapData = new MapData();
                        if (mapDatas == null)
                        {
                            mapDatas = new List<MapData>();
                        }
                        state = GameState.Play;
                        mapData = Boom();
                        RecoradOperate(mapData);
                        if (action != null)
                        {
                            action(curMapData);
                        }
                    }
                });
            }

        }
        else
        {
            //return curMapData.Clone();
        }
    }

    /// <summary>
    /// 移动地图
    /// </summary>
    /// <param name="md"></param>
    /// <returns></returns>
    public void UseBoomImmediately()
    {
        if (IsCanUseBoom())
        {

            MapData mapData = new MapData();
            if (mapDatas == null)
            {
                mapDatas = new List<MapData>();
            }
            state = GameState.Play;
            mapData = Boom();
            RecoradOperate(mapData);
            playUIController.Refresh();

        }
        else
        {
            //return curMapData.Clone();
        }
    }

    /// <summary>
    /// 使用退步道具
    /// </summary>
    /// <returns></returns>
    public void UseGoBackmImmediately()
    {
        if (IsCanUseGoBack())
        {
            MapData mapData = new MapData();
            if (mapDatas == null)
            {
                mapDatas = new List<MapData>();
            }
            if (mapDatas != null && mapDatas.Count > 0)
            {
                state = GameState.Play;
            }
            curMapData = GoBack();
            playUIController.Refresh();
        }
        else
        {
        }
    }

    /// <summary>
    /// 使用退步道具
    /// </summary>
    /// <returns></returns>
    public void UseGoBack(Action<MapData> action)
    {
        if (IsCanUseGoBack())
        {
            if (GameController.Instance.GetItemCount(ItemID.Goback) > 0)
            {
                GameController.Instance.UseItem(ItemID.Goback);
                MapData mapData = new MapData();
                if (mapDatas == null)
                {
                    mapDatas = new List<MapData>();
                }
                if (mapDatas != null && mapDatas.Count > 0)
                {
                    state = GameState.Play;
                }
                curMapData = GoBack();
                if (action != null)
                {
                    action(curMapData);
                }
            }
            else
            {
                AdController.Instance.ShowRewardVideoAd((int value) =>
                {

                    if (value == 1)
                    {
                        MapData mapData = new MapData();
                        if (mapDatas == null)
                        {
                            mapDatas = new List<MapData>();
                        }
                        if (mapDatas != null && mapDatas.Count > 0)
                        {
                            state = GameState.Play;
                        }
                        curMapData = GoBack();
                        if (action != null)
                        {
                            action(curMapData);
                        }
                    }
                });
            }

        }
        else
        {
        }
    }

    /// <summary>
    /// 暂停
    /// </summary>
    /// <param name="isPause"></param>
    public void Pause(bool isPause)
    {
        this.isPause = isPause;
    }

    public int MapSize()
    {
        return mapSize;
    }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    /// <returns>The game state.</returns>
    public GameState GetGameState()
    {
        return state;
    }
    #endregion

    #region 初始化

    /// <summary>
    /// 设置默认地图
    /// </summary>
    /// <returns></returns>
    private void SetDefaultMap()
    {
        curMapData = new MapData();
        curMapData.lastMoveDirection = PlayerOperate.None;
        InsertANumber();
        InsertANumber();
    }

    /// <summary>
    /// 插入一个数字
    /// </summary>
    private void InsertANumber()
    {
        if (curMapData != null && curMapData.gridDatas != null && curMapData.gridDatas.Count <= mapSize * mapSize)
        {
            List<Vector2Int> position = new List<Vector2Int>();
            //找到所有空缺位置
            for (int x = 1; x <= mapSize; x++)
            {
                for (int y = 1; y <= mapSize; y++)
                {
                    bool isNull = true;
                    for (int i = 0; i < curMapData.gridDatas.Count; i++)
                    {
                        if (curMapData.gridDatas[i].Position.x == x && curMapData.gridDatas[i].Position.y == y)
                        {
                            isNull = false;
                            break;
                        }
                    }
                    if (isNull)
                    {
                        var vec = new Vector2Int();
                        vec.x = x;
                        vec.y = y;
                        position.Add(vec);
                    }
                }
            }
            //-------

            //随机插入一个位置
            if (position.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, position.Count);
                GridData gridData = new GridData(position[index].x, position[index].y);

                int maxLadder = 1;

                //取出来应该插入的阶级
                for (int i = 0; i < curMapData.gridDatas.Count; i++)
                {
                    if (maxLadder < curMapData.gridDatas[i].Ladder)
                    {
                        maxLadder = curMapData.gridDatas[i].Ladder;
                    }
                }
                int ladder = 1;
                var item = ConfigController.Instance.GetGridConfigData(maxLadder);
                if (item != null)
                {
                    if (item.CreateMin.Count > 1)
                    {
                        int r = UnityEngine.Random.Range(1, 101);
                        if (r < 80)
                        {
                            ladder = item.CreateMin[0];
                        }
                        else
                        {
                            ladder = item.CreateMin[1];
                        }
                    }
                    else
                    {
                        ladder = item.CreateMin[0];
                    }
                }

                gridData.Ladder = ladder;
                //-------


                //把地图中太小的元素升级
                int min = item.CreateMin[0];
                for (int i = 0; i < item.CreateMin.Count; i++)
                {
                    if (min > item.CreateMin[i])
                    {
                        min = item.CreateMin[i];
                    }
                }
                for (int i = 0; i < curMapData.gridDatas.Count; i++)
                {
                    if (min > curMapData.gridDatas[i].Ladder)
                    {
                        curMapData.gridDatas[i].Ladder = min;
                    }
                }
                //-------

                //设置ID,去除和当前ID与上一步的ID
                List<int> allID = new List<int>();

                for (int i = 0; i < curMapData.gridDatas.Count; i++)
                {
                    if (!allID.Contains(curMapData.gridDatas[i].ID))
                    {
                        allID.Add(curMapData.gridDatas[i].ID);
                    }
                }

                if (mapDatas.Count > 0)
                {
                    for (int i = 0; i < mapDatas[mapDatas.Count - 1].gridDatas.Count; i++)
                    {
                        if (!allID.Contains(mapDatas[mapDatas.Count - 1].gridDatas[i].ID))
                        {
                            allID.Add(mapDatas[mapDatas.Count - 1].gridDatas[i].ID);
                        }
                    }
                }

                int id = 1;
                do
                {
                    if (!allID.Contains(id))
                    {
                        break;
                    }
                    id++;
                } while (true);

                gridData.ID = id;
                //-------


                curMapData.gridDatas.Add(gridData);
            }
        }
    }

    #endregion

    #region 道具&操作
    /// <summary>
    /// 合并和移动地图
    /// </summary>
    /// <returns></returns>
    private MapData MergeMove(PlayerOperate MoveDirection)
    {
        List<int> mergeIDs = new List<int>();
        bool isMerge = false;
        MapData ret = new MapData();
        ret.Score = curMapData.Score;
        ret.lastMoveDirection = MoveDirection;
        Dictionary<int, Dictionary<int, GridData>> allMapData = new Dictionary<int, Dictionary<int, GridData>>();
        for (int i = 0; i < curMapData.gridDatas.Count; i++)
        {
            var item = curMapData.gridDatas[i].Clone();
            item.MergeID = 0;
            item.GridType = GridType.Old;
            item.FromPosition = new Vector2Int(0, 0);
            if (!allMapData.ContainsKey(item.Position.x))
            {
                allMapData.Add(item.Position.x, new Dictionary<int, GridData>());
            }
            allMapData[item.Position.x][item.Position.y] = item;
        }
        bool isMove = false;
        //滑动的时候先合并在进行移动
        switch (MoveDirection)
        {
            case PlayerOperate.ToLeft:
                //合并
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = 1; x <= mapSize; x++)
                    {
                        for (int targetX = x + 1; targetX <= mapSize; targetX++)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                                && (allMapData.ContainsKey(targetX) && allMapData[targetX].ContainsKey(y) && allMapData[targetX][y] != null))
                            {
                                if (allMapData[x][y].Ladder == allMapData[targetX][y].Ladder
                                    && allMapData[x][y].Ladder > 0)
                                {
                                    AddScore(ret, allMapData[x][y].Ladder);
                                    allMapData[x][y].Ladder = AddLevel(allMapData[x][y].Ladder);
                                    allMapData[x][y].MergeID = allMapData[targetX][y].ID;
                                    mergeIDs.Add(allMapData[targetX][y].ID);
                                    allMapData[targetX][y] = null;
                                    isMerge = true;
                                    break;
                                }
                            }
                            if (allMapData.ContainsKey(targetX) && allMapData[targetX].ContainsKey(y) && allMapData[targetX][y] != null)
                            {
                                break;
                            }
                        }
                    }
                }
                //移动
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = 1; x <= mapSize; x++)
                    {
                        for (int tempx = 1; tempx <= x; tempx++)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                           && (!allMapData.ContainsKey(tempx) || !allMapData[tempx].ContainsKey(y) || allMapData[tempx][y] == null))
                            {
                                if (!allMapData.ContainsKey(tempx))
                                {
                                    allMapData.Add(tempx, new Dictionary<int, GridData>());
                                }
                                isMove = true;
                                allMapData[tempx][y] = allMapData[x][y];
                                allMapData[tempx][y].Position = new Vector2Int(tempx, y);
                                allMapData[tempx][y].FromPosition = allMapData[tempx][y].Position;
                                allMapData[x][y] = null;
                                break;
                            }
                        }
                    }
                }
                if (mergeIDs.Count == 0 && !isMove)
                {
                    ret = curMapData;
                }
                break;
            case PlayerOperate.ToRight:
                //合并
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = mapSize; x > 0; x--)
                    {
                        for (int targetX = x - 1; targetX >= 0; targetX--)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                                && (allMapData.ContainsKey(targetX) && allMapData[targetX].ContainsKey(y) && allMapData[targetX][y] != null))
                            {
                                if (allMapData[x][y].Ladder == allMapData[targetX][y].Ladder
                                    && allMapData[x][y].Ladder > 0)
                                {
                                    AddScore(ret, allMapData[x][y].Ladder);
                                    allMapData[x][y].Ladder = AddLevel(allMapData[x][y].Ladder);
                                    allMapData[x][y].MergeID = allMapData[targetX][y].ID;
                                    mergeIDs.Add(allMapData[targetX][y].ID);
                                    allMapData[targetX][y] = null;
                                    isMerge = true;
                                    break;
                                }
                            }
                            if (allMapData.ContainsKey(targetX) && allMapData[targetX].ContainsKey(y) && allMapData[targetX][y] != null)
                            {
                                break;
                            }

                        }
                    }
                }
                //移动
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = mapSize; x >= 1; x--)
                    {
                        for (int tempx = mapSize; tempx > x; tempx--)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                           && (!allMapData.ContainsKey(tempx) || !allMapData[tempx].ContainsKey(y) || allMapData[tempx][y] == null))
                            {
                                if (!allMapData.ContainsKey(tempx))
                                {
                                    allMapData.Add(tempx, new Dictionary<int, GridData>());
                                }
                                isMove = true;
                                allMapData[tempx][y] = allMapData[x][y];
                                allMapData[tempx][y].Position = new Vector2Int(tempx, y);
                                allMapData[tempx][y].FromPosition = allMapData[tempx][y].Position;
                                allMapData[x][y] = null;
                                break;
                            }
                        }
                    }
                }
                if (mergeIDs.Count == 0 && !isMove)
                {
                    ret = curMapData;
                }
                break;
            case PlayerOperate.ToUp:
                //合并
                for (int x = 1; x <= mapSize; x++)
                {
                    for (int y = mapSize; y >= 1; y--)
                    {
                        for (int targetY = y - 1; targetY >= 1; targetY--)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                                && (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(targetY) && allMapData[x][targetY] != null))
                            {
                                if (allMapData[x][y].Ladder == allMapData[x][targetY].Ladder
                                    && allMapData[x][y].Ladder > 0)
                                {
                                    AddScore(ret, allMapData[x][y].Ladder);
                                    allMapData[x][y].Ladder = AddLevel(allMapData[x][y].Ladder);
                                    allMapData[x][y].MergeID = allMapData[x][targetY].ID;
                                    mergeIDs.Add(allMapData[x][targetY].ID);
                                    //mergeIDs.Add(allMapData[x][y].ID);
                                    allMapData[x][targetY] = null;
                                    isMerge = true;
                                    break;
                                }

                            }
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(targetY) && allMapData[x][targetY] != null)
                            {
                                break;
                            }
                        }
                    }
                }
                //移动
                for (int x = 1; x <= mapSize; x++)
                {
                    for (int y = mapSize; y >= 1; y--)
                    {
                        for (int tempY = mapSize; tempY > y; tempY--)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                           && (!allMapData.ContainsKey(x) || !allMapData[x].ContainsKey(tempY) || allMapData[x][tempY] == null))
                            {
                                if (!allMapData.ContainsKey(x))
                                {
                                    allMapData.Add(x, new Dictionary<int, GridData>());
                                }
                                isMove = true;
                                allMapData[x][tempY] = allMapData[x][y];
                                allMapData[x][tempY].Position = new Vector2Int(x, tempY);
                                allMapData[x][tempY].FromPosition = allMapData[x][y].Position;
                                allMapData[x][y] = null;
                                break;
                            }
                        }
                    }
                }
                if (mergeIDs.Count == 0 && !isMove)
                {
                    ret = curMapData;
                }
                break;
            case PlayerOperate.ToDown:
                //合并
                for (int x = 1; x <= mapSize; x++)
                {
                    for (int y = 1; y <= mapSize; y++)
                    {
                        for (int targetY = y + 1; targetY <= mapSize; targetY++)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                                && (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(targetY) && allMapData[x][targetY] != null))
                            {
                                if (allMapData[x][y].Ladder == allMapData[x][targetY].Ladder
                                    && allMapData[x][y].Ladder > 0)
                                {
                                    AddScore(ret, allMapData[x][y].Ladder);
                                    allMapData[x][y].Ladder = AddLevel(allMapData[x][y].Ladder);
                                    allMapData[x][y].MergeID = allMapData[x][targetY].ID;
                                    mergeIDs.Add(allMapData[x][targetY].ID);
                                    allMapData[x][targetY] = null;
                                    isMerge = true;
                                    break;
                                }
                                if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(targetY) && allMapData[x][targetY] != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                //移动
                for (int x = 1; x <= mapSize; x++)
                {
                    for (int y = 1; y <= mapSize; y++)
                    {
                        for (int tempY = 1; tempY < y; tempY++)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y) && allMapData[x][y] != null
                           && (!allMapData.ContainsKey(x) || !allMapData[x].ContainsKey(tempY) || allMapData[x][tempY] == null))
                            {
                                if (!allMapData.ContainsKey(x))
                                {
                                    allMapData.Add(x, new Dictionary<int, GridData>());
                                }
                                isMove = true;
                                allMapData[x][tempY] = allMapData[x][y];
                                allMapData[x][tempY].Position = new Vector2Int(x, tempY);
                                allMapData[x][tempY].FromPosition = allMapData[x][y].Position;
                                allMapData[x][y] = null;
                                break;
                            }
                        }
                    }
                }
                if (mergeIDs.Count == 0 && !isMove)
                {
                    ret = curMapData;
                }
                break;
            case PlayerOperate.None:
                ret = curMapData;
                break;
        }
        if (ret != curMapData)
        {
            ret.gridDatas = new List<GridData>();
            foreach (var itemDic in allMapData)
            {
                foreach (var item in itemDic.Value)
                {
                    if (item.Value != null)
                    {
                        ret.gridDatas.Add(item.Value);
                    }
                }
            }
        }
        if (isMerge)
        {
            AudioController.Instance.PlaySound(AudioType.merge);
        }
        else if (isMove)
        {
            AudioController.Instance.PlaySound(AudioType.move);
        }

        return ret;
    }

    public int AddLevel(int level)
    {
        return level + 1;
    }

    private int GetID()
    {
        return 0;
    }

    /// <summary>
    /// 增加分数
    /// </summary>
    /// <param name="ladder">Ladder.</param>
    private void AddScore(MapData data, int ladder)
    {
        //string old = data.Score.ToString();
        data.Score += MyInt.Pow2(ladder);
        //string newValue = data.Score.ToString();
        //Debug.LogError(string.Format("{0} + {1} = {2}", old, 1 << ladder, newValue));

        GameController.Instance.SetScore(data.Score);
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    /// <returns></returns>
    private MapData Boom()
    {
        MapData mapData = curMapData.Clone();
        if (mapData.gridDatas.Count > ConfigData.BOOM_MIN_GRID)
        {
            List<List<GridData>> minGridData = new List<List<GridData>>();
            //1.排序
            for (int i = 0; i < mapData.gridDatas.Count; i++)
            {
                bool isInsert = false;
                for (int j = 0; j < minGridData.Count; j++)
                {
                    if (minGridData[j][0].Ladder > 0 && minGridData[j][0].Ladder == mapData.gridDatas[i].Ladder)
                    {
                        minGridData[j].Add(mapData.gridDatas[i]);
                        isInsert = true;
                        break;
                    }
                }

                if (!isInsert)
                {
                    List<GridData> datas = new List<GridData>();
                    datas.Add(mapData.gridDatas[i]);
                    minGridData.Add(datas);
                }
            }
            for (int i = 0; i < minGridData.Count; i++)
            {
                for (int j = i + 1; j < minGridData.Count; j++)
                {
                    if (minGridData[i][0].Ladder > minGridData[j][0].Ladder)
                    {
                        var temp = minGridData[i];
                        minGridData[i] = minGridData[j];
                        minGridData[j] = temp;
                    }
                }
            }
            List<GridData> deletGridData = new List<GridData>();

            //2.随机取最小的两个,删除
            while (deletGridData.Count < ConfigData.BOOM_GRID_COUNT && minGridData.Count > 0)
            {
                if (minGridData[0].Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, minGridData[0].Count);
                    deletGridData.Add(minGridData[0][index].Clone());

                    var item = minGridData[0][index];
                    minGridData[0].Remove(item);
                    mapData.gridDatas.Remove(item);
                    if (mapData.gridDatas.Count <= 1)
                    {
                        break;
                    }
                }
                else
                {
                    minGridData.RemoveAt(0);
                }
            }
        }
        return mapData;
    }

    /// <summary>
    /// 使用退步
    /// </summary>
    /// <returns></returns>
    private MapData GoBack()
    {
        if (mapDatas.Count > 0)
        {
            var cur = mapDatas[mapDatas.Count - 1];
            mapDatas.Remove(cur);
            return cur;
        }
        return curMapData;
    }

    /// <summary>
    /// 能否使用炸弹
    /// </summary>
    /// <returns><c>true</c>, if can use boom was ised, <c>false</c> otherwise.</returns>
    public bool IsCanUseBoom()
    {
        return curMapData.gridDatas.Count > ConfigData.BOOM_MIN_GRID;
    }

    /// <summary>
    /// 能否使用回退
    /// </summary>
    /// <returns><c>true</c>, if can use go back was ised, <c>false</c> otherwise.</returns>
    public bool IsCanUseGoBack()
    {
        return mapDatas.Count > 0;
    }

    /// <summary>
    /// 分数
    /// </summary>
    /// <returns>The score.</returns>
    public MyInt Score()
    {
        return curMapData.Score;
    }
    #endregion

    #region 其他
    /// <summary>
    /// 检测游戏状态
    /// </summary>
    private void CheckEndState()
    {
        if (state == GameState.Play && curMapData.gridDatas != null && curMapData.gridDatas.Count == mapSize * mapSize)
        //if (state == GameState.Play && curMapData.gridDatas != null)
        {
            Dictionary<int, Dictionary<int, GridData>> map = new Dictionary<int, Dictionary<int, GridData>>();
            for (int i = 0; i < curMapData.gridDatas.Count; i++)
            {
                if (!map.ContainsKey(curMapData.gridDatas[i].Position.x))
                {
                    map.Add(curMapData.gridDatas[i].Position.x, new Dictionary<int, GridData>());
                }
                map[curMapData.gridDatas[i].Position.x][curMapData.gridDatas[i].Position.y] = curMapData.gridDatas[i];
            }
            bool isHas = false;
            for (int i = 0; i < curMapData.gridDatas.Count; i++)
            {
                var item = curMapData.gridDatas[i];
                if (map.ContainsKey(item.Position.x + 1)
                    && map[item.Position.x + 1].ContainsKey(item.Position.y)
                    && map[item.Position.x + 1][item.Position.y].Ladder == curMapData.gridDatas[i].Ladder)
                {
                    isHas = true;
                    break;
                }
                if (map.ContainsKey(item.Position.x - 1)
                  && map[item.Position.x - 1].ContainsKey(item.Position.y)
                  && map[item.Position.x - 1][item.Position.y].Ladder == curMapData.gridDatas[i].Ladder)
                {
                    isHas = true;
                    break;
                }
                if (map.ContainsKey(item.Position.x)
                  && map[item.Position.x].ContainsKey(item.Position.y + 1)
                  && map[item.Position.x][item.Position.y + 1].Ladder == curMapData.gridDatas[i].Ladder)
                {
                    isHas = true;
                    break;
                }
                if (map.ContainsKey(item.Position.x)
                  && map[item.Position.x].ContainsKey(item.Position.y - 1)
                  && map[item.Position.x][item.Position.y - 1].Ladder == curMapData.gridDatas[i].Ladder)
                {
                    isHas = true;
                    break;
                }
            }

            if (!isHas)
            {
                state = GameState.GameOver;
                GameController.Instance.EndGame(this);
                GameController.Instance.StartCoroutine(ShowUI(ViewID.CompleteUI));
            }

        }

        //判断胜利
        int maxLadder = 1;
        for (int i = 0; i < curMapData.gridDatas.Count; i++)
        {
            if (maxLadder < curMapData.gridDatas[i].Ladder)
            {
                maxLadder = curMapData.gridDatas[i].Ladder;
            }
        }

        bool isMax = true;
        var dic = ConfigController.Instance.GetGridConfigDataDic();

        foreach (var item in dic)
        {
            if (item.Value.Ladder > maxLadder)
            {
                isMax = false;
                break;
            }
        }

        if (isMax)
        {
            state = GameState.Victory;
            GameController.Instance.EndGame(this);

            GameController.Instance.StartCoroutine(ShowUI(ViewID.CompleteFinish));
        }
        ////判断胜利
        //var configItem = ConfigController.Instance.GetGridConfigData(maxLadder + 1);

        //if (configItem == null)
        //{
        //    state = GameState.Victory;
        //    GameController.Instance.EndGame(this);

        //    GameController.Instance.StartCoroutine(ShowUI(ViewID.CompleteFinish));
        //}
    }

    public IEnumerator ShowUI(ViewID viewID)
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.ShowUI(viewID);
    }

    /// <summary>
    /// 记录操作
    /// </summary>
    /// <param name="mapData"></param>
    private void RecoradOperate(MapData mapData)
    {
        if (mapData != curMapData)
        {
            mapDatas.Add(curMapData);
        }
        while (mapDatas != null && mapDatas.Count > 0 && mapDatas.Count > ConfigData.RECORD_COUNT)
        {
            mapDatas.RemoveAt(0);
        }
        curMapData = mapData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public MapData GetCurSaveData()
    {
        if (state == GameState.GameOver)
        {
            return null;
        }
        return curMapData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<MapData> GetCurSaveDatas()
    {
        if (state == GameState.GameOver)
        {
            return null;
        }
        return mapDatas;
    }
    IPlayUIController playUIController;
    public void Resister(IPlayUIController playUIController)
    {
        this.playUIController = playUIController;
    }
    #endregion
}
