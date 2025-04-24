using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Define.ObjectType objType { get; protected set; }


    bool isInit = false;
    public virtual bool Init()
    {

        if (isInit) return false;


        isInit = true;
        return true;
    }


    public virtual void UpdateController() { }
}
