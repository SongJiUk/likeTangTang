
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public static class Utils
{
    //Get을 해본다음 없으면 추가, 있으면 리턴
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
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
    
    public static bool IsVaild(this GameObject _go)
    {
        return _go != null && _go.activeSelf;
    }

    public static bool IsVaild(this BaseController _bc)
    {
        return _bc != null && _bc.isActiveAndEnabled;
    }

    public static Vector2 CreateMonsterSpawnPoint(Vector2 _CharacterPos, float _minDist = 10.0f, float _maxDist = 20.0f)
    {
        //NOTE : 몬스터 스폰 포인트 지정해주는거 각도, 거리 계산해서 스폰포인트 랜덤으로 지정.
        float angle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        float dist = UnityEngine.Random.Range(_minDist, _maxDist);

        float xDist = Mathf.Cos(angle) * dist;
        float yDist = Mathf.Sin(angle) * dist;

        Vector2 spawnPos = _CharacterPos + new Vector2(xDist, yDist);

        return spawnPos;
    }

    public static void BindEvent(this GameObject _go, Action _action = null, Action<BaseEventData> _dragAction = null, Define.UIEvent _type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(_go, _action, _dragAction, _type);
    }

    public static Vector2 CreateObjectAroundPlayer(Vector3 _pos)
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        float radius = angle * Mathf.Deg2Rad;

        Vector2 spawnPos = new Vector2(Mathf.Cos(radius), Mathf.Sin(radius)) * UnityEngine.Random.Range(0, radius);
        Vector3 pos = _pos + new Vector3(spawnPos.x, spawnPos.y, 0f);

        return pos;
    }

    //NOTE : SKillType 통일 시키려고
    public static Define.SkillType GetSkillTypeFromInt(int _value)
    {
        foreach(Define.SkillType type in Enum.GetValues(typeof(Define.SkillType)))
        {
            int minValue = (int)type;
            int maxValue = minValue+5;

            if(_value >= minValue && _value <= maxValue)
            {
                return type;
            }
        }

        return Define.SkillType.None;
    }
}
