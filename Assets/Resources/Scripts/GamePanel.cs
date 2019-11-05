using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Text m_TxtStep;
    private GameObject m_Cover;
    private GameObject m_Feiled;
    private GameObject m_Victory;
    private Text m_TxtVictory;
    private Button m_BtnReplay;

    public void Init()
    {
        m_TxtStep = transform.Find("txtStep").GetComponent<Text>();
        m_Cover = transform.Find("Cover").gameObject;
        m_Feiled = transform.Find("Failed").gameObject;
        m_Victory = transform.Find("Victory").gameObject;
        m_TxtVictory = transform.Find("Victory/Text").GetComponent<Text>();
        m_BtnReplay = transform.Find("btnReplay").GetComponent<Button>();
        m_BtnReplay.onClick.AddListener(OnReplayBtn);

        ResetRefresh();
    }

    public void RefreshStep(int step)
    {
        m_TxtStep.text = "步数:" + step;
    }

    public void RefreshResult(bool isVictory, int step = 0, int bastScore = 0)
    {
        m_Cover.gameObject.SetActive(true);
        m_BtnReplay.gameObject.SetActive(true);
        if (isVictory)
        {
            m_Victory.gameObject.SetActive(true);
            if(bastScore == 0)
            {
                m_TxtVictory.text = string.Format("你用 {0} 步围住了神经猫", step);
            }
            else if(step < bastScore)
            {
                m_TxtVictory.text = string.Format("你用 {0} 步围住了神经猫, 相对最佳分数进步了 {1} 步", step, bastScore - step);
            }
            else
            {
                m_TxtVictory.text = string.Format("你用 {0} 步围住了神经猫, 最佳分数是 {1} 步", step, bastScore);
            }
        }
        else
        {
            m_Feiled.gameObject.SetActive(true);
        }
    }

    public void ResetRefresh()
    {
        RefreshStep(0);
        m_Cover.gameObject.SetActive(false);
        m_Feiled.gameObject.SetActive(false);
        m_Victory.gameObject.SetActive(false);
        m_BtnReplay.gameObject.SetActive(false);
    }

    private void OnReplayBtn()
    {
        ResetRefresh();
        GameManager.Instance.ChangeGameState(GameState.GamePlaying);
    }
}
