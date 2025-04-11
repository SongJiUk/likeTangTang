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
    public bool isLearnSkill
    {
        get { return SkillLevel > 0;}
    }

    public SkillBase(Define.SkillType _skillType)
    {
        Skilltype = _skillType;
    }

    public virtual void ActivateSkill() { UpdateSkillData(); }
    protected virtual void GenerateProjectile(int _templateID, CreatureController _owner, Vector3 _startPos, Vector3 _dir)
    {
        
    }


    
    #region Destory
    Coroutine coDestroyInfo;

    public void StartDestory(float _time)
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

    IEnumerator CoDestroy(float _time)
    {
        yield return new WaitForSeconds(_time);

        if(this.IsVaild())
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
