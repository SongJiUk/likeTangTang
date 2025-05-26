
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
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
    
    public static bool IsValid(this GameObject _go)
    {
        return _go != null && _go.activeSelf;
    }

    public static bool IsValid(this BaseController _bc)
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

    public static Vector2 CreateObjectAroundPlayer(Vector3 _pos, float _minDist = 6f, float _maxDist = 10f)
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        float radius = angle * Mathf.Deg2Rad;
        
        float dist = UnityEngine.Random.Range(_minDist, _maxDist);


        Vector2 spawnPos = new Vector2(Mathf.Cos(radius), Mathf.Sin(radius)) * dist;
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

    public static Define.EvoloutionType GetEvolutionSkillTypeFromInt(int _value)
    {
        foreach (Define.EvoloutionType type in Enum.GetValues(typeof(Define.EvoloutionType)))
        {
            return type;
        }

        return Define.EvoloutionType.None;
    }

    public static MonsterController FindClosestMonster(Vector3 _origin, HashSet<MonsterController> _prevTargets = null)
    {
        float closestDist = Mathf.Infinity;
        MonsterController closestMC = null;

        foreach(MonsterController monster in Manager.ObjectM.mcSet)
        {
            if(!monster.IsValid()) continue;
            if(_prevTargets!= null && _prevTargets.Contains(monster)) continue;


            float dist = Vector3.Distance(_origin, monster.transform.position);
            if(dist < closestDist)
            {
                closestDist = dist;
                closestMC = monster;
            }
        }

        return closestMC;
    }

    public static void DestroyChilds(this GameObject _go)
    {
        Transform[] children = new Transform[_go.transform.childCount];

        for(int i =0; i<_go.transform.childCount; i++)
        {
            children[i] = _go.transform.GetChild(i);
        }

        foreach(Transform child in children)
        {
            Manager.ResourceM.Destory(child.gameObject);
        }
    }



    //List를 섞어준다.
    public static void Shuffle<T>(this List<T> _list)
    {
        int count = _list.Count;

        while(count > 1)
        {
            count--;
            int randNum = UnityEngine.Random.Range(0, count+1);
            T value = _list[randNum];
            _list[randNum] = _list[count];
            _list[count] = value;
        }
    }


    public static Color HexToColor(string _color)
    {
        Color parsedColor;
        ColorUtility.TryParseHtmlString("#" + _color, out parsedColor);

        return parsedColor;
    }

    public static int GetUpgradeNumber(Enum _grade)
    {
        var match = Regex.Match(_grade.ToString(), @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }

}
