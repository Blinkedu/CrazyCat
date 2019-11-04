using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat
{
    private Transform m_TransCat = null;
    private Animator m_Animator = null;
    private Vector3 m_InitPos = Vector3.zero;

    public Cat(Vector2 pos)
    {
        GameObject prefab = ResManager.Load<GameObject>(ConstDefine.PREFAB_CAT);
        Transform parent = GameObject.Find("Canvas/CatRoot").transform;
        GameObject goCat = GameObject.Instantiate(prefab, parent);
        m_InitPos = pos;
        m_TransCat = goCat.transform;
        m_TransCat.localPosition = m_InitPos;
        m_Animator = goCat.GetComponent<Animator>();
    }

    public void Move(Vector2 pos)
    {
        m_TransCat.position = pos;
    }

    public void Reset()
    {
        m_TransCat.localPosition = m_InitPos;
    }
}
