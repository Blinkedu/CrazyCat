using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager
{
    private static Dictionary<string, Object> m_ResDic = new Dictionary<string, Object>();

    public static T Load<T>(string path) where T : Object
    {
        Object res = null;
        if(m_ResDic.TryGetValue(path,out res))
            return res as T;
        T tmp = Resources.Load<T>(path);
        if(tmp == null)
        {
            Debug.LogError("加载资源异常:Path=" + path);
            return null;
        }
        else
        {
            m_ResDic.Add(path, tmp);
            return tmp;
        }
    }
}