using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//NOTE : 스킬 관련된 모든 코드들 여기서 사용할것임(플레이어에 최소한의 코드만 들어가게.), 스킬매니저임 쉽게 말해서
public class SkillComponent : MonoBehaviour
{
    public List<SkillBase> skills { get; }= new List<SkillBase>();
    public List<SkillBase> RepeatSkills { get;} = new List<SkillBase>{ };

    public List<SequenceSkill> SequenceSkills { get;} = new List<SequenceSkill>();

    public T AddSkill<T>(Vector3 _pos, Transform _parent = null ) where T : SkillBase
    {
        //[ ] 나중에 templateID로 바꾸기.
        System.Type type = typeof(T);
        //Debug.Log("AddSkill");
        if(type == typeof(EgoSword))
        {  
            var egoSword = Manager.ObjectM.Spawn<EgoSword>(_pos, 1);
            egoSword.transform.SetParent(_parent);
            egoSword.ActivateSkill(); // [ ] 레벨이 0이라면 따로 함수 처리.

            skills.Add(egoSword);
            RepeatSkills.Add(egoSword);

            return egoSword as T;

        }
        else if(type == typeof(FireBall))
        {
            var fireBall = Manager.ObjectM.Spawn<FireBall>(_pos, 2);
            fireBall.transform.SetParent(_parent);
            fireBall.coolTime = 2f;
            fireBall.ActivateSkill();

            skills.Add(fireBall);
            RepeatSkills.Add(fireBall);

            return fireBall as T;
        }
        else
        {

        }

        return null;

    }
}
