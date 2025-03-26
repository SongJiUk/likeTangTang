using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ProjectileController : SkillController
{
    CreatureController owner;
    Vector3 dir;
    float speed;
    float lifeTime;


    //초기화, 세팅
    public override bool Init()
    {
        base.Init();
        speed = 10f;
        lifeTime = 5f;
        StartDestory(lifeTime);
        return true;
    }

    public void SetInfo(int _templateID, CreatureController _owner, Vector3 _dir)
    {
        if(Manager.DataM.SkillDic.TryGetValue(_templateID, out Data.SkillData skillData) == false)
        {
            Debug.LogError("Skill Data is Unknown");
            return;
        }

        SkillDatas = skillData;
        owner = _owner;
        dir = _dir;
    }


    public override void UpdateController()
    {
        base.UpdateController();

        transform.position += dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.gameObject.GetComponent<MonsterController>();
        if(mc.IsVaild() == false) return;
        if(this.IsVaild()==false) return;

        mc.OnDamaged(owner, SkillDatas.damage);
        StopDestroy();

        Manager.ObjectM.DeSpawn(this);
    }

}
