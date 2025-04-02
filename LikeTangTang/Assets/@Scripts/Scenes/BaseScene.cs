using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.SceneType SceneType {get; protected set;} = Define.SceneType.UnKnownScene;


    void Awake()
    {
        Init();
    }
    
    public virtual void Init()
    {
        var obj  = GameObject.FindObjectOfType(typeof(EventSystem));
        if(obj == null)
            Manager.ResourceM.Instantiate("UI/EventSystem").name = "@EventSystem";
    }
    public abstract void Clear();
}
