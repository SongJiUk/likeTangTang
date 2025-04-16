using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class SkillBase : BaseController
{
    public CreatureController owner {get; set;}
    public Define.SkillType Skilltype { get; set; } = Define.SkillType.None;
    public Data.SkillData SkillDatas { get; protected set; }
    public int SkillLevel {get; set;} = 0;
    public float TotalDamage { get; set; } = 0;
    public string prefabName;
    public float duration;
    public float coolTime;
    public int projectileCount; //투사체 개수 
    public int boundDist;
    public int numBounce;
    public float bounceSpeed;
    public float speed;
    public float numPenerations; //관통 개수
    public float range;
    public float effectRange;

    public bool isLearnSkill
    {
        get { return SkillLevel > 0;}
    }

    public SkillBase(Define.SkillType _skillType)
    {
        Skilltype = _skillType;
    }

    public virtual void ActivateSkill() { UpdateSkillData(); }
    protected virtual void GenerateProjectile(
    CreatureController _owner, 
    string _prefabName,
    Vector3 _startPos = default, 
    Vector3 _dir= default, 
    Vector3 _targetPos = default, 
    SkillBase _skill = null, 
    HashSet<MonsterController> _sharedTarget = null)
    {
        ProjectileController pc = Manager.ObjectM.Spawn<ProjectileController>(_startPos, _prefabName: _prefabName);
        pc.SetInfo(_owner,_startPos, _dir, _targetPos, _skill, _sharedTarget);
    }


    
    #region Destory
    Coroutine coDestroyInfo;

    public void StartDestory(float _time = 0)
    {
        StopDestroy();
        coDestroyInfo = StartCoroutine(CoDestroy(_time));
    }

    public void StopDestroy()
    {
        if(coDestroyInfo != null) 
        {
            StopCoroutine(coDestroyInfo);
            coDestroyInfo = null;
        }

    }

    public IEnumerator CoDestroy(float _time)
    {
        yield return new WaitForSeconds(_time);

        if(this.IsValid())
        {
            Manager.ObjectM.DeSpawn(this);
        }
    }
    #endregion
    public virtual void OnChangedSkillData() {}

    public virtual void OnSkillLevelup()
    {
        if(SkillLevel == 0) ActivateSkill();

        SkillLevel++;
        UpdateSkillData();
    }

    public Data.SkillData UpdateSkillData(int _skillID = 0)
    {
        int id = 0;
        if(_skillID ==0)
            id = SkillLevel < 2 ? (int)Skilltype : (int)Skilltype + SkillLevel - 1;
        else
            id = _skillID;

        SkillData skillData = new SkillData();

        if(Manager.DataM.SkillDic.TryGetValue(id, out skillData) == false) return SkillDatas;
        
        SkillDatas = skillData;
        OnChangedSkillData();
        return SkillDatas;

    }
    
}
