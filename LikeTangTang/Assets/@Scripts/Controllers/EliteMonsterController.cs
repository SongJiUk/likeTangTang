using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterController : MonsterController
{
    public override bool Init()
    {
        if (!base.Init()) return false;
        CreatureState = Define.CreatureState.Moving;

        Rigid.simulated = true;
        transform.localScale = new Vector3(2f, 2f, 2f);

        objType = Define.ObjectType.Monster;

        InvokeMonsterData();
        return true;
    }
    public override void OnDead()
    {
        base.OnDead();
        //TODO : 엘리트보스 잡고 난 후의 보상
        //Manager.GameM.player.Skills


        DropItem();
    }

    void DropItem()
    {

    }
}
