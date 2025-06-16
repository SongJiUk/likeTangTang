using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{

    Animator anim;
    public override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.LobbyScene;
        Manager.UiM.ShowSceneUI<UI_LobbyScene>();

        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        RenderTexture rt = new RenderTexture(512, 512, 16);
        var cam = GameObject.Find("PreviewCamera").GetComponent<Camera>();
        cam.targetTexture = rt;
        var target = cam.targetTexture;
        anim = GameObject.Find("Character").GetComponent<Animator>();

        int id = Manager.GameM.CurrentCharacter.DataId;
        string anim_name = Manager.DataM.CreatureDic[id].CharacterAnimName;
        anim.runtimeAnimatorController = Manager.ResourceM.Load<RuntimeAnimatorController>(anim_name);
        Manager.SceneM.Setup(cam, target, this);

        Manager.SoundM.Play(Define.Sound.Bgm, "Bgm_Lobby");
        
    }

    public void ChangeCharacter()
    {
        int id = Manager.GameM.CurrentCharacter.DataId;
        string anim_name = Manager.DataM.CreatureDic[id].CharacterAnimName;
        anim.runtimeAnimatorController = Manager.ResourceM.Load<RuntimeAnimatorController>(anim_name);
    }

    public override void Clear()
    {

    }
}
