using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{

    public override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.TitleScene;
        // TODO : 여기서 게임매니저 init 해줘야함(성취미션도 있기떄문.)
    }

    public override void Clear()
    {

    }
}
