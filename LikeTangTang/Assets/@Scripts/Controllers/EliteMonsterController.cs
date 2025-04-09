using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonsterController : MonsterController
{
    public override bool Init()
    {
        if (!base.Init()) return false;

        return true;
    }
    public override void OnDead()
    {
        
    }
}
