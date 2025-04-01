using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager
{

    public void LoadScene(Define.Scene _type, Transform _tr = null) //씬 이동 애니메이션()
    {
        switch(_type)
        {
            case Define.Scene.TitleScene :
                
            break;

            case Define.Scene.LobbyScene :
                SceneManager.LoadScene(GetScene(_type));
            break;

            case Define.Scene.GameScene : 
            
            break;
        }
    }


    public string GetScene(Define.Scene _type)
    {
        string sceneName = Enum.GetName(typeof(Define.Scene), _type);
        return sceneName;
    }

}
