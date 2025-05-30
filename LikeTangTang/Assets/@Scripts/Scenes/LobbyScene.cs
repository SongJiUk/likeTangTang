using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{

    public override void Init()
    {
        base.Init();
        
        SceneType = Define.SceneType.LobbyScene;

        RenderTexture rt = new RenderTexture(512, 512, 16);
        var cam = GameObject.Find("PreviewCamera").GetComponent<Camera>();
        cam.targetTexture = rt;
        var target = cam.targetTexture;
        var anim = GameObject.Find("Character").GetComponent<Animator>();

        //int id = Manager.GameM.CurrentCharacter.DataId;
        int id = 2;
        string anim_name = Manager.DataM.CreatureDic[id].CharacterAnimName;
        anim.runtimeAnimatorController = Manager.ResourceM.Load<RuntimeAnimatorController>(anim_name);
        Manager.SceneM.Setup(cam, target);


        Manager.UiM.ShowSceneUI<UI_LobbyScene>();
    }

    public override void Clear()
    {

    }
}
