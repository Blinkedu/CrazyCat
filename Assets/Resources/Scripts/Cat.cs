using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 猫
/// </summary>
public class Cat
{
    private Transform m_TransCat = null;
    private Animator m_Animator = null;
    private Vector3 m_InitPos = Vector3.zero;
    private Vector2Int m_InitIndex = Vector2Int.zero;
    private Vector2Int m_CurrentIndex;

    public Cat(int rowIndex,int colIndex)
    {
        m_InitIndex = m_CurrentIndex = new Vector2Int(rowIndex, colIndex);
        GameObject prefab = ResManager.Load<GameObject>(ConstDefine.PREFAB_CAT);
        Transform parent = GameObject.Find("Canvas/CatRoot").transform;
        GameObject goCat = GameObject.Instantiate(prefab, parent);
        m_InitPos = GameManager.Instance.CalcPos(rowIndex,colIndex);
        m_TransCat = goCat.transform;
        m_TransCat.localPosition = m_InitPos;
        m_Animator = goCat.GetComponent<Animator>();
    }

    public Vector2Int CurrentIndex
    {
        get { return m_CurrentIndex; }
    }

    public void Move()
    {
        Vector2Int target = GameManager.Instance.GetTargetPotIndex(m_CurrentIndex.x, m_CurrentIndex.y);
        m_TransCat.localPosition = GameManager.Instance.CalcPos(target.x, target.y);
        m_CurrentIndex = target;
    }

    public void Reset()
    {
        m_TransCat.localPosition = m_InitPos;
        m_CurrentIndex = m_InitIndex;
        m_Animator.SetBool("IsWeizhu", false);
    }

    public void Clear()
    {
        GameObject.Destroy(m_TransCat.gameObject);
    }

    public void Closeed()
    {
        m_Animator.SetBool("IsWeizhu", true);
    }
}
