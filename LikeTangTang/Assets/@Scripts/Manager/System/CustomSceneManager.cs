using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager
{

    public void LoadScene(Define.SceneType _type, Transform _tr = null) //씬 이동 애니메이션()
    {
        switch(_type)
        {
            case Define.SceneType.TitleScene :
                
            break;

            case Define.SceneType.LobbyScene :
                SceneManager.LoadScene(GetScene(_type));
            break;

            case Define.SceneType.GameScene : 
                SceneManager.LoadScene(GetScene(_type));
            break;
        }
    }


    public string GetScene(Define.SceneType _type)
    {
        string sceneName = Enum.GetName(typeof(Define.SceneType), _type);
        return sceneName;
    }

}
