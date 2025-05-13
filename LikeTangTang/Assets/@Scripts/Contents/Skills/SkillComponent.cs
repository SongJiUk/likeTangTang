using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//NOTE : 스킬 관련된 모든 코드들 여기서 사용할것임(플레이어에 최소한의 코드만 들어가게.), 스킬매니저임 쉽게 말해서
public class SkillComponent : MonoBehaviour
{
    public List<SkillBase> skillList { get; }= new List<SkillBase>();
    public List<int> evolutionItemList { get; } = new List<int>();
    public List<SkillBase> RepeatSkills { get;} = new List<SkillBase>{ };

    public List<SequenceSkill> SequenceSkills { get;} = new List<SequenceSkill>();


    public Dictionary<Define.SkillType, int> SavedBattleSkill = new Dictionary<Define.SkillType, int>();
    //TODO : 서포트 스킬(진화스킬)

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
        SkillBase skill = null;
        if(_type == Define.SkillType.EnergyRing || _type == Define.SkillType.ElectronicField || _type == Define.SkillType.SpectralSlash)
        {
            GameObject go = Manager.ResourceM.Instantiate(name, Utils.FindChild(gameObject, Define.STANDARDNAME).transform);
            if(go != null)
            {
                skill = go.GetOrAddComponent<SkillBase>();
            }
        }
        else
        {
            Type skillType = Type.GetType(name);
            skill = gameObject.AddComponent(skillType) as SkillBase;
        }

        skill.UpdateSkillData();

        if(skill != null) skillList.Add(skill);
        
        // if(_type == Define.SkillType.EnergyRing || _type == Define.SkillType.ElectronicField || _type == Define.SkillType.SpectralSlash)
        // {

        //     GameObject go = Manager.ResourceM.Instantiate(name, Utils.FindChild(gameObject, Define.STANDARDNAME).transform);
        //     if(go != null)
        //     {
        //         SkillBase skill = go.GetOrAddComponent<SkillBase>();
        //         skillList.Add(skill);
        //     }
        // }
        // else
        // {
        //     Type skillType = Type.GetType(name);
        //     SequenceSkill skill = gameObject.AddComponent(skillType) as SequenceSkill;
        //     if(skill != null)
        //     {
                
        //     }
        //     else
        //     {
        //         RepeatSkill skillbase = gameObject.GetComponent(skillType) as RepeatSkill;
        //         if(skillbase != null) skillList.Add(skillbase);
        //     }
        // }
    }
    public void RemoveSkill()
    {

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

    public List<SkillBase> RecommendSkills()
    {
        //TODO : 배운 스킬 및 맥스 스킬 개수 비교 
        
        List<SkillBase> skillList = Manager.GameM.player.Skills.skillList.ToList();
        List<SkillBase> activeSkills = skillList.FindAll(skill => skill.isLearnSkill);

        List<SkillBase> recommendSkills;
        if(activeSkills.Count == Define.MAX_SKILL_COUNT)
        {
            recommendSkills = activeSkills.FindAll(s => s.SkillLevel < Define.MAX_SKILL_LEVEL);
        }
        else
        {
            recommendSkills = skillList.FindAll(s => s.SkillLevel < Define.MAX_SKILL_LEVEL);
            
        }
        recommendSkills.Shuffle();
        return recommendSkills.Take(3).ToList();
    }

    public List<int> GetAvailableEvolutionItems()
    {
        List<int> evoItems = new List<int>();

        foreach (var skill in skillList)
        {
            if (!skill.isLearnSkill) continue;
            if (skill.SkillLevel != 5) continue;
            if (!skill.isCanEvolve()) continue;
            if (skill.SkillDatas.EvolutionItemID == 0) continue;

            if (skill.SkillDatas.EvolutionItemID != 0)
                evoItems.Add(skill.SkillDatas.EvolutionItemID);
        }

        return evoItems;
    }


    public List<object> GetSkills()
    {
        List<SkillBase> baseSkillCandidates = RecommendSkills();
        List<int> evoItemCandidates = GetAvailableEvolutionItems();

        List<object> finalCandidates = new List<object>();

        if(baseSkillCandidates != null) finalCandidates.AddRange(baseSkillCandidates);
        if(evoItemCandidates.Count > 0) finalCandidates.AddRange(evoItemCandidates);
        
        finalCandidates.Shuffle();

        return finalCandidates.Take(3).ToList();
    }

   
    public void TryEvolveSkill(int _evolutionItemID)
    {
        foreach(var skill in skillList)
        {
            if (!skill.isLearnSkill) continue;
            if (!skill.isCanEvolve()) continue;

            if(skill.SkillDatas.EvolutionItemID == _evolutionItemID)
            {
                evolutionItemList.Add(_evolutionItemID);
                skill.Evolution();
                break;
            }
        }
    }

}
