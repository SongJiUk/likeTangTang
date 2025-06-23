using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CustomSceneManager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public Camera cam;
    public RenderTexture cam_target;
    public LobbyScene lobbyScene;


    public void Setup(Camera _cam, RenderTexture _cam_target, LobbyScene _lobbyScene)
    {
        cam = _cam;
        cam_target = _cam_target;
        lobbyScene = _lobbyScene;

    }

    public void LoadScene(Define.SceneType _type, Transform _tr = null) //씬 이동 애니메이션()
    {

        if(CurrentScene.SceneType == Define.SceneType.TitleScene)
        {
            Manager.Clear();
            SceneManager.LoadScene(GetScene(_type));
            return;
        }

        PlaySceneChangeAnimation(_type, _tr);
    }

    private void PlaySceneChangeAnimation(Define.SceneType _type, Transform _parent)
    {
        var animGo = Manager.ResourceM.Instantiate("SceneChangeAnimation_In");
        var anim = animGo.GetOrAddComponent<SceneChangeAnimation_In>();
        animGo.transform.SetParent(_parent, worldPositionStays: false);

        Time.timeScale = 1f;
       


        anim.SetInfo(_type, () =>
        {
            DOTween.KillAll();
            DOTween.Clear();
            Manager.UpdateM.Clear();
            Manager.ResourceM.Destory(Manager.UiM.SceneUI.gameObject);
            Manager.Clear();
            switch (_type)
            {
                case Define.SceneType.LobbyScene:
                    Manager.AdM.ShowBanner();
                    break;

                case Define.SceneType.GameScene:
                    Manager.AdM.HideBanner();
                    break;
            }
            SceneManager.LoadSceneAsync(GetScene(_type));

            //var op = SceneManager.LoadSceneAsync(GetScene(_type));
            //op.allowSceneActivation = true;

            

        });
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
