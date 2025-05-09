using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{

    public override void Init()
    {
        base.Init();
        
        SceneType = Define.SceneType.LobbyScene;

        Manager.UiM.ShowSceneUI<UI_LobbyScene>();
    }

    public override void Clear()
    {

    }
}
