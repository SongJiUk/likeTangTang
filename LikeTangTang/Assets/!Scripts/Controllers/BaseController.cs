using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Define.ObjectType objType { get; protected set; }

    private void Awake()
    {
        Init();

    }

    bool isInit = false;
    public virtual bool Init()
    {

        if (isInit) return false;

        isInit = true;
        return true;
    }
}
