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

    public List<Data.SpecialSkillData> SpecialSkills {get; } = new List<Data.SpecialSkillData>();


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
        if (SavedBattleSkill.ContainsKey(_type))
            SavedBattleSkill[_type] = skill.SkillLevel;
        else
            SavedBattleSkill.Add(_type, skill.SkillLevel);
    }

    public void AddSpecialSkill(Data.SpecialSkillData _skill, bool _isLockSkill = false)
    {
        _skill.IsLearned = true;

        if (_skill.SpecialSkillName == Define.SpecialSkillName.Healing)
        {
            //TODO : 스킬타입이 힐링이면 바로 치료(이건 빼야될듯 아니면 인게임에 회복기능 하나 만들어주던가.)
        }

        SpecialSkills.Add(_skill);

        //TODO : LoadSkill이면 return;
        if (_isLockSkill) return;


        if(_skill.SkillType == Define.SpecialSkillType.General)
        {
            GeneralSpecialSkill(_skill);
        }
        else if(_skill.SkillType == Define.SpecialSkillType.Special)
        {
            foreach(SkillBase playerSkill in skillList)
            {
                if(_skill.SpecialSkillName.ToString() == playerSkill.Skilltype.ToString())
                {
                    playerSkill.UpdateSkillData();
                }
            }
        }
        
    }

    public void LoadSkill(Define.SkillType _skillType, int _level)
    {
        AddSkill(_skillType);
        for(int i = 0; i<_level; i++)
        {
            LevelUpSkill(_skillType);
        }
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
                if(SavedBattleSkill.ContainsKey(_type))
                {
                    SavedBattleSkill[_type] = skillList[i].SkillLevel;
                    Manager.GameM.SaveGame();
                }
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


    public void GeneralSpecialSkill(Data.SpecialSkillData _skill)
    {
        List<Data.SpecialSkillData> skillList = SpecialSkills.Where(_skill => _skill.SkillType == Define.SpecialSkillType.General).ToList();

        PlayerController player = Manager.GameM.player;
        player.CriticalRate += _skill.CriticalBouns;
        player.MaxHpRate += _skill.MaxHpBonus;
        player.ExpBounsRate += _skill.ExpBonus;
        player.DamageReduction += _skill.DamageReductionBonus;
        player.AttackRate += _skill.AttackBonus;
        player.SpeedRate += _skill.MoveSpeedBonus;
        player.HealBounsRate += _skill.HealingBouns;
        player.HpRegen += _skill.HpRegenBonus;
        player.CriticalDamage += _skill.CriticalDamageBouns;
        player.CollectDistBonus += _skill.CollectRangeBouns;

        player.UpdatePlayerStat();
    }

    public void PlayerLevelUpBonus()
    {
        List<Data.SpecialSkillData> skills = SpecialSkills.Where(skill => skill.SkillType == Define.SpecialSkillType.LevelUp).ToList();

        float MoveSpeedBonus = 0;
        float DamageReductionBonus = 0;
        float AttackBonus = 0;
        float CriticalBonus = 0;
        float CriticalDamageBonus = 0;

        foreach(Data.SpecialSkillData skill in skills)
        {
            MoveSpeedBonus += skill.LevelUpMoveSpeedBonus;
            DamageReductionBonus += skill.LevelUpDamageReductionBonus;
            AttackBonus += skill.LevelUpAttackBonus;
            CriticalBonus += skill.LevelUpCriticalBonus;
            CriticalDamageBonus += skill.CriticalDamageBouns;
        }

        PlayerController player = Manager.GameM.player;
        player.SpeedRate += MoveSpeedBonus;
        player.DamageReduction += DamageReductionBonus;
        player.AttackRate += AttackBonus;
        player.CriticalRate += CriticalBonus;
        player.CriticalDamage += CriticalDamageBonus;

        player.UpdatePlayerStat();
    }


}
