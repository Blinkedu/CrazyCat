using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance = null;
    public static GameManager Instance { get { return m_Instance; } }

    [SerializeField]
    private StartPanel m_StartPanel = null;
    [SerializeField]
    private GamePanel m_GamePanel = null;

    private Cat m_Cat = null;
    private MapController m_MapCtrl = null;
    public GameState GameState { get; private set; }

    // 最好成绩（围住猫用的最小步数）
    private int m_BastScore = 0;
    public int BastScore
    {
        get
        {
            return m_BastScore;
        }
        private set
        {
            if(m_BastScore == 0 || value < m_BastScore)
            {
                m_BastScore = value;
                PlayerPrefs.SetInt("BastScore", value);
            }
        }
    }

    private int m_CurrentStep = 0;

    private void Start()
    {
        m_Instance = this;
        BastScore = PlayerPrefs.GetInt("BastScore", 0);
        m_StartPanel.Init();
        m_GamePanel.Init();
        ChangeGameState(GameState.StartMenu);
    }

    private void InitGame()
    {
        if (m_MapCtrl == null)
            m_MapCtrl = new MapController();
        if (m_Cat == null)
            m_Cat = new Cat(4, 4);
        ResetMap();
    }

    // 获取移动的目标点索引
    private void ResetMap()
    {
        m_CurrentStep = 0;
        m_MapCtrl.Reset();
        m_Cat.Reset();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            switch (GameState)
            {
                case GameState.StartMenu:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                    break;
                case GameState.GamePlaying:
                    ChangeGameState(GameState.StartMenu);
                    break;
                case GameState.GameOver:
                    ChangeGameState(GameState.StartMenu);
                    break;
            }
        }
    }

    public void CatMove()
    {
        m_GamePanel.RefreshStep(++m_CurrentStep);
        m_Cat.Move();
        // 检测是否结束游戏
        if (CheckLose())
        {
            // 失败
            ChangeGameState(GameState.GameOver, false);
        }
        else if (CheckVictory())
        {
            // 胜利
            m_Cat.Closeed();
            ChangeGameState(GameState.GameOver, true);
        }
    }

    public Vector2 CalcPos(int rowIndex, int colIndex)
    {
        return m_MapCtrl.CalcPos(rowIndex, colIndex);
    }

    public Vector2Int GetTargetPotIndex(int rowIndex, int colIndex)
    {
        return m_MapCtrl.GetTargetPotIndex(rowIndex, colIndex);
    }

    // 切换游戏状态
    public void ChangeGameState(GameState state, params object[] args)
    {
        if (state == GameState) return;
        GameState = state;
        switch (state)
        {
            case GameState.StartMenu:
                ClearGame();
                m_StartPanel.gameObject.SetActive(true);
                m_GamePanel.ResetRefresh();
                m_GamePanel.gameObject.SetActive(false);
                break;
            case GameState.GamePlaying:
                if (m_GamePanel.gameObject.activeSelf == false)
                    m_GamePanel.gameObject.SetActive(true);
                InitGame();
                break;
            case GameState.GameOver:
                m_GamePanel.RefreshResult((bool)args[0], m_CurrentStep, BastScore);
                BastScore = m_CurrentStep;
                break;
        }
    }

    // 检测是否成功
    private bool CheckVictory()
    {
        // 如果猫没有可移动的点就成功
        var list = m_MapCtrl.GetCanMovePotIndexs(m_Cat.CurrentIndex.x, m_Cat.CurrentIndex.y);
        if (list != null && list.Count > 0)
            return false;
        return true;
    }

    // 检测失败
    private bool CheckLose()
    {
        int rowIndex = m_Cat.CurrentIndex.x;
        int colIndex = m_Cat.CurrentIndex.y;
        // 如果猫到达边界就失败
        return (rowIndex == 0 || rowIndex == MapController.ROW_COUNT - 1 || colIndex == 0 || colIndex == MapController.COL_COUNT - 1);
    }

    // 清理游戏
    private void ClearGame()
    {
        if (m_Cat != null)
        {
            m_Cat.Clear();
            m_Cat = null;
        }
        if (m_MapCtrl != null)
        {
            m_MapCtrl.Clear();
            m_MapCtrl = null;
        }
    }
}

public enum GameState
{
    None = 0,
    StartMenu,
    GamePlaying,
    GameOver
}