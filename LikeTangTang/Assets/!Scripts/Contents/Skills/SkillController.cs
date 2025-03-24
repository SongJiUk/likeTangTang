using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * TODO : Melee : 평타
 * Projectile : 투사체
 *  투사체는 끝없이 날아가는것이 아님, 시간 지나면 사라져야함.
 * Field : 바닥
 */
public class SkillController : BaseController
{
    public Define.SkillType Skilltype { get; set; }
    public Data.SkillData SkillDatas { get; protected set; }

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
