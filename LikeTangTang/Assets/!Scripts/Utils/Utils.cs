using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utils
{
    //Get을 해본다음 없으면 추가, 있으면 리턴
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    //자식찾기.
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform tr = FindChild<Transform>(go, name, recursive);
        if (tr == null) return null;

        return tr.gameObject;
    }

    //계층구조 산하에서 물체를 찾고싶을때.
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null) return null;

        if(recursive == false)
        {
            for(int i =0; i<go.transform.childCount; i++)
            {
                Transform tr = go.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || tr.name == name)
                {
                    T component = tr.GetComponent<T>();
                    if (component != null) return component;
                }
            }
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if(string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }
}
