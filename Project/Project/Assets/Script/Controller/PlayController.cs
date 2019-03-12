using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 
                               _ooOoo_
                              o8888888o 
                              88" . "88
                              (| -_- |)
                              O\  =  /O
                           ____/`---'\____
                         .'  \\|     |//  `.
                        /  \\|||  :  |||//  \
                       /  _||||| -:- |||||-  \
                       |   | \\\  -  /// |   |
                       | \_|  ''\---/''  |   |
                       \  .-\__  `-`  ___/-. /
                     ___`. .'  /--.--\  `. . __
                  ."" '<  `.___\_<|>_/___.'  >'"".
                 | | :  `- \`.;`\ _ /`;.`/ - ` : | |
                 \  \ `-.   \_ __\ /__ _/   .-` /  /
            ======`-.____`-.___\_____/___.-`____.-'======
                               `=---='
            ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                       佛祖保佑 永无BUG
                       永不逆向 永不报错
                       永不破解

*/

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
    /// 
    /// </summary>
    private MapData curMapData;

    /// <summary>
    /// 地图大小
    /// </summary>
    private int mapSize;

    /// <summary>
    /// 记录地图次数
    /// </summary>
    private int recordCount = 10;

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
    public void StartGame(int size = 4)
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
        if (!isPause)
        {
            MapData mapData = new MapData();
            if (mapDatas == null)
            {
                mapDatas = new List<MapData>();
            }

            mapData = MergeMove(md);
            RecoradOperate(mapData);
            InsertANumber();
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
    public MapData UseBoom()
    {
        if (!isPause)
        {
            MapData mapData = new MapData();
            if (mapDatas == null)
            {
                mapDatas = new List<MapData>();
            }

            mapData = Boom();
            RecoradOperate(mapData);
            return curMapData.Clone();
        }
        else
        {
            return curMapData.Clone();
        }
    }

    /// <summary>
    /// 使用退步道具
    /// </summary>
    /// <returns></returns>
    public MapData UseGoBack()
    {
        if (!isPause)
        {
            MapData mapData = new MapData();
            if (mapDatas == null)
            {
                mapDatas = new List<MapData>();
            }

            curMapData = GoBack();
            return curMapData.Clone();
        }
        else
        {
            return curMapData.Clone();
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
        //return;
        //if (mapSize > 0)
        //{
        //    int startID = 1;
        //    for (int i = startID; i <= mapSize; i++)
        //    {
        //        for (int j = startID; j <= mapSize; j++)
        //        {
        //            GridData item = new GridData(i, j);
        //            item.Ladder = 0;
        //            curMapData.gridDatas.Add(item);
        //        }
        //    }

        //    //随机两个开始点，可能重复
        //    int index1 = Random.Range(0, curMapData.gridDatas.Count);
        //    int index2 = Random.Range(0, curMapData.gridDatas.Count);
        //    curMapData.gridDatas[index1].Ladder = 1;
        //    curMapData.gridDatas[index2].Ladder = 1;
        //}
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
                        if (curMapData.gridDatas[i].Position.x == x && curMapData.gridDatas[i].Position.y != y)
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

            //随机插入一个位置
            if (position.Count > 0)
            {
                int index = Random.Range(0, position.Count);
                GridData gridData = new GridData(position[index].x, position[index].y);
                gridData.Ladder = 1;
                curMapData.gridDatas.Add(gridData);

                int indexID = 1;
                int listIndex = 0;
                while (curMapData.gridDatas.Count > 0
                    && indexID < mapSize * mapSize * 2 + 10)
                {
                    if (listIndex < curMapData.gridDatas.Count)
                    {
                        if (curMapData.gridDatas[listIndex].ID == indexID)
                        {
                            listIndex = 0;
                            indexID++;
                            continue;
                        }
                    }
                    if (mapDatas.Count > 0 && listIndex < mapDatas[mapDatas.Count - 1].gridDatas.Count)
                    {
                        if (mapDatas[mapDatas.Count - 1].gridDatas[listIndex].ID == indexID)
                        {
                            listIndex = 0;
                            indexID++;
                            continue;
                        }
                    }

                    if (mapDatas.Count == 0 || listIndex >= mapDatas[mapDatas.Count - 1].gridDatas.Count)
                    {
                        gridData.ID = indexID;
                        break;
                    }
                }
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
        MapData ret = new MapData();
        ret.lastMoveDirection = MoveDirection;
        Dictionary<int, Dictionary<int, GridData>> allMapData = new Dictionary<int, Dictionary<int, GridData>>();
        for (int i = 0; i < curMapData.gridDatas.Count; i++)
        {
            var item = curMapData.gridDatas[i].Clone();
            if (!allMapData.ContainsKey(item.Position.x))
            {
                allMapData.Add(item.Position.x, new Dictionary<int, GridData>());
            }
            allMapData[item.Position.x][item.Position.y] = item;
        }
        //滑动的时候先合并在进行移动
        switch (MoveDirection)
        {
            case PlayerOperate.ToLeft:
                //合并
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = 1; x <= mapSize; x++)
                    {
                        int targetX = x + 1;
                        if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                            && allMapData.ContainsKey(targetX) && allMapData[targetX].ContainsKey(y))
                        {
                            if (allMapData[x][y].MergeID > 0
                                && allMapData[targetX][y].MergeID > 0
                                && allMapData[x][y].Ladder == allMapData[targetX][y].Ladder
                                && allMapData[x][y].Ladder > 0)
                            {
                                allMapData[x][y].Ladder++;
                                allMapData[targetX][y] = new GridData(targetX, y);
                            }
                        }
                    }
                }

                //移动
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = 1; x <= mapSize; x++)
                    {
                        for (int tempx = x + 1; tempx <= mapSize; tempx++)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                                && allMapData.ContainsKey(tempx) && allMapData[tempx].ContainsKey(y))
                            {
                                if (allMapData[x][y].Ladder <= 0
                                    && allMapData[tempx][y].Ladder > 0)
                                {
                                    allMapData[x][y].FromID = allMapData[tempx][y].ID;
                                    allMapData[x][y].Ladder = allMapData[tempx][y].Ladder;
                                    allMapData[tempx][y] = new GridData(tempx, y);
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case PlayerOperate.ToRight:
                //合并
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = mapSize - 1; x >= 0; x--)
                    //for (int x = 1; x <= mapSize; x++)
                    {
                        int targetX = x - 1;
                        if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                            && allMapData.ContainsKey(targetX) && allMapData[targetX].ContainsKey(y))
                        {
                            if (allMapData[x][y].MergeID > 0
                                && allMapData[targetX][y].MergeID > 0
                                && allMapData[x][y].Ladder == allMapData[targetX][y].Ladder
                                && allMapData[x][y].Ladder > 0)
                            {
                                allMapData[x][y].Ladder++;
                                allMapData[targetX][y] = new GridData(targetX, y);
                            }
                        }
                    }
                }

                //移动
                for (int y = 1; y <= mapSize; y++)
                {
                    for (int x = mapSize - 1; x >= 0; x--)
                    {
                        for (int tempx = x - 1; tempx >= 0; tempx--)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                                && allMapData.ContainsKey(tempx) && allMapData[tempx].ContainsKey(y))
                            {
                                if (allMapData[x][y].Ladder <= 0
                                   && allMapData[tempx][y].Ladder > 0)
                                {
                                    allMapData[x][y].FromID = allMapData[tempx][y].ID;
                                    allMapData[x][y].Ladder = allMapData[tempx][y].Ladder;
                                    allMapData[tempx][y] = new GridData(tempx, y);
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case PlayerOperate.ToUp:
                //合并
                for (int x = 0; x < mapSize; x++)
                {
                    for (int y = mapSize - 1; y >= 0; y--)
                    {
                        int targetY = y - 1;
                        if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                            && allMapData.ContainsKey(x) && allMapData[x].ContainsKey(targetY))
                        {
                            if (allMapData[x][y].MergeID > 0
                                && allMapData[x][targetY].MergeID > 0
                                && allMapData[x][y].Ladder == allMapData[x][targetY].Ladder
                                && allMapData[x][y].Ladder > 0)
                            {
                                allMapData[x][y].Ladder++;
                                allMapData[x][targetY] = new GridData(x, targetY);
                            }
                        }
                    }
                }

                //移动
                for (int x = 0; x <= mapSize; x++)
                {
                    for (int y = mapSize; y >= 0; y--)
                    {
                        for (int tempy = y - 1; tempy >= 0; tempy--)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                               && allMapData[x].ContainsKey(tempy))
                            {
                                if (allMapData[x][y].Ladder <= 0
                                   && allMapData[x][tempy].Ladder > 0)
                                {
                                    allMapData[x][y].FromID = allMapData[x][tempy].ID;
                                    allMapData[x][y].Ladder = allMapData[x][tempy].Ladder;
                                    allMapData[x][tempy] = new GridData(x, tempy);
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case PlayerOperate.ToDown:
                //合并
                for (int x = 0; x < mapSize; x++)
                {
                    for (int y = 0; y < mapSize; y++)
                    {
                        int targetY = y + 1;
                        if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                            && allMapData.ContainsKey(x) && allMapData[x].ContainsKey(targetY))
                        {
                            if (allMapData[x][y].MergeID > 0
                                && allMapData[x][targetY].MergeID > 0
                                && allMapData[x][y].Ladder == allMapData[x][targetY].Ladder
                                && allMapData[x][y].Ladder > 0)
                            {
                                allMapData[x][y].Ladder++;
                                allMapData[x][targetY] = new GridData(x, targetY);
                            }
                        }
                    }
                }

                //移动
                for (int x = 0; x <= mapSize; x++)
                {
                    for (int y = 0; y <= mapSize; y++)
                    {
                        for (int tempy = y + 1; tempy <= mapSize; tempy++)
                        {
                            if (allMapData.ContainsKey(x) && allMapData[x].ContainsKey(y)
                            && allMapData[x].ContainsKey(tempy))
                            {
                                if (allMapData[x][y].Ladder <= 0
                                 && allMapData[x][tempy].Ladder > 0)
                                {
                                    allMapData[x][y].FromID = allMapData[x][tempy].ID;
                                    allMapData[x][y].Ladder = allMapData[x][tempy].Ladder;
                                    allMapData[x][tempy] = new GridData(x, tempy);
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case PlayerOperate.None:
                ret = curMapData;
                break;
        }
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
        return ret;
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    /// <returns></returns>
    private MapData Boom()
    {
        MapData mapData = curMapData.Clone();
        int targetCount = 2;
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
        while (deletGridData.Count < targetCount && minGridData.Count > 0)
        {
            if (minGridData[0].Count > 0)
            {
                int index = Random.Range(0, minGridData[0].Count);
                deletGridData.Add(minGridData[0][index].Clone());

                var data = new GridData(minGridData[0][index].Position.x, minGridData[0][index].Position.y);
                minGridData[0][index] = data;
            }
            else
            {
                minGridData.RemoveAt(0);
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
            return mapDatas[mapDatas.Count - 1];
        }
        return curMapData;
    }
    #endregion

    #region 其他

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
        while (mapDatas.Count != 0 && mapDatas.Count > recordCount)
        {
            mapDatas.RemoveAt(0);
        }
        curMapData = mapData;
    }

    #endregion
}


