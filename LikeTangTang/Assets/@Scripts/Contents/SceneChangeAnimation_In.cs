using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeAnimation_In : UI_Popup
{
    Action action;
    Define.SceneType nextScene;

 

    public void SetInfo(Define.SceneType _nextScene, Action _callback)
    {
        transform.localScale = Vector3.one;
        action = _callback;
        nextScene = _nextScene;
        StartCoroutine(OnAnimationComplete());
    }

    IEnumerator OnAnimationComplete()
    {
        yield return new WaitForSeconds(1f);
        action.Invoke();
    }
}
