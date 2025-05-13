using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }


    public void LoadScene(Define.SceneType _type, Transform _tr = null) //씬 이동 애니메이션()
    {
        switch(CurrentScene.SceneType)
        {
            case Define.SceneType.TitleScene :
                Manager.Clear();
                SceneManager.LoadScene(GetScene(_type));
                break;

            case Define.SceneType.LobbyScene :
                Time.timeScale = 1;
                Manager.ResourceM.Destory(Manager.UiM.SceneUI.gameObject);
                Manager.Clear();
                SceneManager.LoadScene(GetScene(_type));
            break;

            case Define.SceneType.GameScene :
                Time.timeScale = 1;
                Manager.ResourceM.Destory(Manager.UiM.SceneUI.gameObject);
                Manager.Clear();
                SceneManager.LoadScene(GetScene(_type));
            break;
        }
    }


    public string GetScene(Define.SceneType _type)
    {
        string sceneName = Enum.GetName(typeof(Define.SceneType), _type);
        return sceneName;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
