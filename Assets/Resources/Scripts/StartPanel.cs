using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public void Init()
    {
        transform.Find("btnStart").GetComponent<Button>().onClick.AddListener(OnStartBtn);
    }

    private void OnStartBtn()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GameState.GamePlaying);
    }
}
