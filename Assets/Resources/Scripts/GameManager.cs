using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager m_Instance = null;
    public GameManager Instance { get { return m_Instance; } }

    private Cat m_Cat = null;
    private MapController m_MapCtrl = null;

    private void Start()
    {
        m_Instance = this;
        m_MapCtrl = new MapController();
        m_Cat  = new Cat(m_MapCtrl.CalcPos(4, 4));
    }

    // 获取移动的目标点索引
    private void ResetMap()
    {
        m_MapCtrl.Reset();
        m_Cat.Reset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ResetMap();
    }

    public void CatMove(Vector2 pos)
    {
        m_Cat.Move(pos);
    }
}
