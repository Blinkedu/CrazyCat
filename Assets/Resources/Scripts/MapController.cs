using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图控制器
/// </summary>
public class MapController
{
    private Pot[,] m_Maps = null;

    public const int ROW_COUNT = 9;
    public const int COL_COUNT = 9;
    
    // 重置
    public void Reset()
    {
        InitPots();
        InitHinder();
    }

    // 清理
    public void Clear()
    {
        for (int i = 0; i < ROW_COUNT; i++)
        {
            for (int j = 0; j < COL_COUNT; j++)
            {
                m_Maps[i, j].Clear();
            }
        }
        m_Maps = null;
    }

    // 根据索引计算位置
    public Vector2 CalcPos(int rowIndex, int colIndex)
    {
        float space = 70f;
        float startX = -280f;
        float startY = -485f;

        if (rowIndex % 2 == 0)
        {
            // 向左偏移
            return new Vector3(colIndex * space + startX + space / 4f, rowIndex * space + startY, 0);
        }
        else
        {
            // 向右偏移
            return new Vector3(colIndex * space + startX - space / 4f, rowIndex * space + startY, 0);
        }
    }

    // 获取目标点索引
    public Vector2Int GetTargetPotIndex(int rowIndex, int colIndex)
    {
        Vector2Int index = FindMinPotIndex(rowIndex, colIndex);
        if (index.x == rowIndex && index.y == colIndex)
        {
            List<Vector2Int> tempList = GetCanMovePotIndexs(rowIndex, colIndex);
            if (tempList.Count > 0)
            {
                Vector2Int temp = tempList[0];
                float sum = ROW_COUNT + COL_COUNT;
                for (int i = 0; i < tempList.Count; i++)
                {
                    int x = tempList[i].x;
                    int y = tempList[i].y;
                    int tempSum = 0;
                    if (Mathf.Abs(0 - x) > Mathf.Abs(ROW_COUNT - 1 - x))
                    {
                        tempSum += Mathf.Abs(ROW_COUNT - 1 - x);
                    }
                    else
                    {
                        tempSum += Mathf.Abs(0 - x);
                    }

                    if (Mathf.Abs(0 - y) > Mathf.Abs(COL_COUNT - 1 - y))
                    {
                        tempSum += Mathf.Abs(COL_COUNT - 1 - y);
                    }
                    else
                    {

                        tempSum += Mathf.Abs(0 - y);
                    }
                    if (sum > tempSum)
                    {
                        sum = tempSum;
                        index.x = x;
                        index.y = y;
                    }
                }
            }
        }
        return index;
    }

    // 获取最短路径目标点索引
    private Vector2Int FindMinPotIndex(int rowIndex, int colIndex)
    {
        /* 获取目标点思路：
         *  1.向周围六方向遍历，可通行到边缘的方向
         *  2.比较各可通行到边缘的方向的距离，取最短距离方向的点
         */
        int minCount = int.MaxValue;
        int potCount = 0;
        Vector2Int target = new Vector2Int(rowIndex, colIndex);
        bool canMove = true;

        //左
        for (int i = colIndex - 1; i >= 0; i--)
        {
            if (!m_Maps[rowIndex, i].CanMove)
            {
                canMove = false;
                break;
            }
            potCount++;
        }
        if (canMove && potCount < minCount)
        {
            minCount = potCount;
            target.x = rowIndex;
            target.y = colIndex - 1;
            //return v;
        }

        //右
        canMove = true;
        potCount = 0;
        for (int i = colIndex + 1; i < COL_COUNT; i++)
        {
            if (!m_Maps[rowIndex, i].CanMove)
            {
                canMove = false;
                break;
            }
            potCount++;
        }
        if (canMove && potCount < minCount)
        {
            minCount = potCount;
            target.x = rowIndex;
            target.y = colIndex + 1;
            //return v;
        }
        //左上
        canMove = true;
        potCount = 0;
        for (int i = rowIndex + 1, j = (rowIndex % 2 == 0 ? colIndex : colIndex - 1); i < ROW_COUNT && j >= 0; i++)
        {
            if (!m_Maps[i, j].CanMove)
            {
                canMove = false;
                break;
            }
            if (i % 2 == 1)
            {
                j--;
            }
            potCount++;
        }
        if (canMove && potCount < minCount)
        {
            minCount = potCount;
            if (rowIndex % 2 == 0)
            {
                target.x = rowIndex + 1;
                target.y = colIndex;
            }
            else
            {
                target.x = rowIndex + 1;
                target.y = colIndex - 1;
            }
            //return v;
        }

        //右下
        canMove = true;
        potCount = 0;
        for (int i = rowIndex - 1, j = (rowIndex % 2 == 0 ? colIndex + 1 : colIndex); i >= 0 && j < COL_COUNT; i--)
        {
            if (!m_Maps[i, j].CanMove)
            {
                canMove = false;
                break;
            }
            if (i % 2 == 0)
            {
                j++;
            }
            potCount++;
        }
        if (canMove && potCount < minCount)
        {
            minCount = potCount;
            if (rowIndex % 2 == 0)
            {
                target.x = rowIndex - 1;
                target.y = colIndex + 1;
            }
            else
            {
                target.x = rowIndex - 1;
                target.y = colIndex;
            }
            //return v;
        }

        //右上
        canMove = true;
        potCount = 0;
        for (int i = rowIndex + 1, j = (rowIndex % 2 == 0 ? colIndex + 1 : colIndex); i < ROW_COUNT && j < COL_COUNT; i++)
        {
            if (!m_Maps[i, j].CanMove)
            {
                canMove = false;
                break;
            }
            //偶数行+1，奇数行不加
            if (i % 2 == 0)
            {
                j++;
            }
            potCount++;
        }
        if (canMove && potCount < minCount)
        {
            minCount = potCount;
            if (rowIndex % 2 == 0)
            {
                target.x = rowIndex + 1;
                target.y = colIndex + 1;
            }
            else
            {
                target.x = rowIndex + 1;
                target.y = colIndex;
            }
            //return v;
        }

        //左下
        canMove = true;
        potCount = 0;
        for (int i = rowIndex - 1, j = (rowIndex % 2 == 0 ? colIndex : colIndex - 1); i >= 0 && j >= 0; i--)
        {
            if (!m_Maps[i, j].CanMove)
            {
                canMove = false;
                break;
            }
            if (i % 2 == 1)
            {
                j--;
            }
            potCount++;
        }
        if (canMove && potCount < minCount)
        {
            minCount = potCount;
            if (rowIndex % 2 == 0)
            {
                target.x = rowIndex - 1;
                target.y = colIndex;
            }
            else
            {
                target.x = rowIndex - 1;
                target.y = colIndex - 1;
            }
            //return v;
        }

        return target;
    }

    // 获取能移动点列表
    public List<Vector2Int> GetCanMovePotIndexs(int rowIndex, int colIndex)
    {
        List<Vector2Int> potMoveList = new List<Vector2Int>();
        Vector2Int index = Vector2Int.zero;
        //左
        index.x = rowIndex;
        index.y = colIndex - 1;
        if (IsCanMove(index)) potMoveList.Add(index);

        //右
        index.x = rowIndex;
        index.y = colIndex + 1;
        if (IsCanMove(index)) potMoveList.Add(index);

        //上
        index.x = rowIndex + 1;
        index.y = colIndex;
        if (IsCanMove(index)) potMoveList.Add(index);

        //下
        index.x = rowIndex - 1;
        index.y = colIndex;
        if (IsCanMove(index)) potMoveList.Add(index);

        //奇数行   左上  、偶数行    右上
        index.x = rowIndex + 1;
        if (rowIndex % 2 == 0)
            index.y = colIndex + 1;
        else
            index.y = colIndex - 1;
        if (IsCanMove(index)) potMoveList.Add(index);

        //奇数行   左下  、偶数行    右下
        index.x = rowIndex - 1;
        if (rowIndex % 2 == 0)
            index.y = colIndex + 1;
        else
            index.y = colIndex - 1;
        if (IsCanMove(index)) potMoveList.Add(index);

        return potMoveList;
    }

    // 判断索引位置是否能够到达
    private bool IsCanMove(Vector2Int index)
    {
        if (index.x < 0 || index.x > ROW_COUNT - 1 || index.y < 0 || index.y > COL_COUNT - 1)
            return false;
        return m_Maps[index.x, index.y].CanMove;
    }

    // 初始化障碍
    private void InitHinder()
    {
        // 随机障碍个数
        int hiderCount = Random.Range(10, 16);
        while (hiderCount > 0)
        {
            int i = Random.Range(0, 9);
            int j = Random.Range(0, 9);
            if (!m_Maps[i, j].CanMove || (i == 4 && j == 4))
                continue;
            m_Maps[i, j].SetHinder();
            hiderCount -= 1;
        }
    }

    // 初始化地图点
    private void InitPots()
    {
        if (m_Maps == null)
        {
            // 创建地图点
            m_Maps = new Pot[9, 9];
            Transform m_PotRoot = GameObject.Find("Canvas/PotRoot").transform;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_Maps[i, j] = new Pot(m_PotRoot, CalcPos(i, j));
                }
            }
        }
        else
        {
            // 重置地图点
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    m_Maps[i, j].Reset();
                }
            }
        }
    }
}