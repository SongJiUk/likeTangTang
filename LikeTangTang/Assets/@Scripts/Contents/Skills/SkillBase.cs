using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : BaseController
{
    public CreatureController owner {get; set;}
    public Define.SkillType Skilltype { get; set; } = Define.SkillType.None;
    public Data.SkillData SkillDatas { get; protected set; }
    public int SkillLevel {get; set;} = 0;
    public bool isLearnSkill
    {
        get { return SkillLevel > 0;}
    }

    public SkillBase(Define.SkillType _skillType)
    {
        Skilltype = _skillType;
    }

    public virtual void ActivateSkill() {}
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

    
}
