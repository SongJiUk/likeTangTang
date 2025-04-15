using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : RepeatSkill
{
    public FireBall() {}
    
    public override void DoSkill()
    {

        if(Manager.GameM.player == null) return;
        Vector3 spawnPos = Manager.GameM.player.FirePos;
        Vector3 dir = Manager.GameM.player.ShootDir;

    }

}
