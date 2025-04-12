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

        //GenerateProjectile(2, owner, spawnPos, dir);
    }

    protected override void GenerateProjectile(CreatureController _owner, string _prefabName, Vector3 _startPos, Vector3 _dir, Vector3 _targetPos, SkillBase _skill)
    {
        base.GenerateProjectile(_owner, _prefabName, _startPos, _dir, _targetPos, _skill);
    }
}
