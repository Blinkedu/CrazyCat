using UnityEngine;
using UnityEngine.UI;

public class Pot
{
    private Image m_imgPot = null;

    // 该位置是否可以到达
    private bool m_CanMove = true;

    public Pot(Transform potRoot, Vector2 pos)
    {
        GameObject prefab = ResManager.Load<GameObject>(ConstDefine.PREFAB_POT);
        GameObject goPot = GameObject.Instantiate(prefab, potRoot);
        goPot.transform.localPosition = pos;
        m_imgPot = goPot.GetComponent<Image>();
        Button btn = m_imgPot.gameObject.AddComponent<Button>();
        btn.onClick.AddListener(OnClick); 
    }

    public bool CanMove
    {
        get { return m_CanMove; }
    }

    // 设置为障碍
    public void SetHinder()
    {
        if (m_imgPot != null)
            m_imgPot.sprite = ResManager.Load<Sprite>(ConstDefine.SPRITE_POT2);
        m_CanMove = false;
        m_imgPot.raycastTarget = false;
    }

    // 点击事件函数
    private void OnClick()
    {
        if (GameManager.Instance.GameState != GameState.GamePlaying || !m_CanMove)
            return;
        AudioManager.PlaySound(ConstDefine.AUDIO_POT);
        SetHinder();
        // 移动猫
        GameManager.Instance.CatMove();
    }

    // 重置
    public void Reset()
    {
        if (m_imgPot != null)
            m_imgPot.sprite = ResManager.Load<Sprite>(ConstDefine.SPRITE_POT1);
        m_CanMove = true;
        m_imgPot.raycastTarget = true;
    }

    // 清理
    public void Clear()
    {
        GameObject.Destroy(m_imgPot.gameObject);
    }
}
