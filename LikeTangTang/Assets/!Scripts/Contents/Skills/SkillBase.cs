using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : BaseController
{
    public BaseController owner {get; set;}
    public Define.SkillType Skilltype { get; set; }
    public Data.SkillData SkillDatas { get; protected set; }
    public float coolTime {get; set;}
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
