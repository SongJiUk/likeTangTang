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

        GenerateProjectile(2, owner, spawnPos, dir);
    }

    protected override void GenerateProjectile(int _templateID, CreatureController _owner, Vector3 _startPos, Vector3 _dir)
    {
        ProjectileController pc = Manager.ObjectM.Spawn<ProjectileController>(_startPos, _templateID);
        pc.SetInfo(_templateID, _owner, _dir);
    }
}
