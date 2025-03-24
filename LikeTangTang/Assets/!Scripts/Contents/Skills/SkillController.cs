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
    public Data.SkillData SkillData { get; protected set; }

    
}
