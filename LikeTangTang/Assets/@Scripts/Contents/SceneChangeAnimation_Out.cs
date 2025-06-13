using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeAnimation_Out : UI_Popup
{
    Animator anim;
    Action action;
    Define.SceneType prevScene;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetInfo(Define.SceneType _prevScene, Action _callback)
    {
        transform.localScale = Vector3.one;
        action = _callback;
        prevScene = _prevScene;

    }

    public void OnAnimationComplete()
    {
        action.Invoke();
    }
}
