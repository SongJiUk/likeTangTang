using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//NOTE : 스킬 관련된 모든 코드들 여기서 사용할것임(플레이어에 최소한의 코드만 들어가게.), 스킬매니저임 쉽게 말해서
public class SkillComponent : MonoBehaviour
{
    public List<SkillBase> skillList { get; }= new List<SkillBase>();
    public List<SkillBase> RepeatSkills { get;} = new List<SkillBase>{ };

    public List<SequenceSkill> SequenceSkills { get;} = new List<SequenceSkill>();

    public Dictionary<Define.SkillType, int> SavedBattleSkill = new Dictionary<Define.SkillType, int>();
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

            skillList.Add(egoSword);
            RepeatSkills.Add(egoSword);

            return egoSword as T;

        }
        else if(type == typeof(FireBall))
        {
            var fireBall = Manager.ObjectM.Spawn<FireBall>(_pos, 2);
            fireBall.transform.SetParent(_parent);
            //fireBall.coolTime = 2f;
            fireBall.ActivateSkill();

            skillList.Add(fireBall);
            RepeatSkills.Add(fireBall);

            return fireBall as T;
        }
        else
        {

        }

        return null;
    }

    public void AddSkill(Define.SkillType _type,  int _skillID = 0)
    {
        string name = _type.ToString();
        if(_type == Define.SkillType.EnergyRing || _type == Define.SkillType.ElectronicField || _type == Define.SkillType.SpectralSlash)
        {
            GameObject go = Manager.ResourceM.Instantiate(name, gameObject.transform);
            if(go != null)
            {
                SkillBase skill = go.GetOrAddComponent<SkillBase>();
                skillList.Add(skill);
                
            }
        }
        else if(_type == Define.SkillType.PlasmaSpinner)
        {   
            SequenceSkill skill = gameObject.AddComponent(Type.GetType(name)) as SequenceSkill;
            if(skill != null)
            {

            }
            else
            {
                RepeatSkill skillbase = gameObject.GetComponent(Type.GetType(name)) as RepeatSkill;
                skillList.Add(skillbase);
            }
        }
        else
        {
            SequenceSkill skill = gameObject.AddComponent(Type.GetType(name)) as SequenceSkill;
            if(skill != null)
            {

            }
            else
            {
                RepeatSkill skillbase = gameObject.GetComponent(Type.GetType(name)) as RepeatSkill;
                
                if(skillbase != null) skillList.Add(skillbase);
            }
        }
    }

    public void LevelUpSkill(Define.SkillType _type)
    {   
        for(int i =0; i< skillList.Count; i++)
        {
            if(skillList[i].Skilltype == _type)
            {
                if(skillList[i].SkillLevel > 6) continue;
                skillList[i].OnSkillLevelup();
            }
        }
    }
}
