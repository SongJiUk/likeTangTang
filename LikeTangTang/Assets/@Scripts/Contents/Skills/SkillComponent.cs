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

    private GameManager gameM;
    private ResourceManager resourceM;
    public List<SkillBase> skillList { get; }= new List<SkillBase>();
    public List<int> evolutionItemList { get; } = new List<int>();
    public List<SkillBase> RepeatSkills { get;} = new List<SkillBase>{ };

    public List<SequenceSkill> SequenceSkills { get;} = new List<SequenceSkill>();

    public List<Data.SpecialSkillData> SpecialSkills {get; } = new List<Data.SpecialSkillData>();


    public Dictionary<Define.SkillType, int> SavedBattleSkill = new Dictionary<Define.SkillType, int>();
    public Dictionary<Define.SkillType, int> SavedEvolutionSkill = new Dictionary<Define.SkillType, int>();
    public event Action UpdateSkillUI;

    bool stopped = false;
    // public T AddSkill<T>(Vector3 _pos, Transform _parent = null ) where T : SkillBase
    // {
    //     System.Type type = typeof(T);
    //     //Debug.Log("AddSkill");
    //     if(type == typeof(EgoSword))
    //     {  
    //         var egoSword = Manager.ObjectM.Spawn<EgoSword>(_pos, 1);
    //         egoSword.transform.SetParent(_parent);
    //         egoSword.ActivateSkill();

    //         skillList.Add(egoSword);
    //         RepeatSkills.Add(egoSword);

    //         return egoSword as T;

    //     }
    //     else if(type == typeof(FireBall))
    //     {
    //         var fireBall = Manager.ObjectM.Spawn<FireBall>(_pos, 2);
    //         fireBall.transform.SetParent(_parent);
    //         //fireBall.coolTime = 2f;
    //         fireBall.ActivateSkill();

    //         skillList.Add(fireBall);
    //         RepeatSkills.Add(fireBall);

    //         return fireBall as T;
    //     }
    //     else
    //     {

    //     }

    //     return null;
    // }

    private void Awake()
    {
        gameM = Manager.GameM;
        resourceM = Manager.ResourceM;
    }

    public void AddSkill(Define.SkillType _type, int _skillID = 0)
    {
        SkillBase skill = null;
        Transform parent = transform;

        switch(_type)
        {
            case Define.SkillType.BossMove:
            case Define.SkillType.BossDash:
            case Define.SkillType.BossSkill:
                skill = gameObject.AddComponent(Type.GetType(_type.ToString())) as SkillBase;
                if (skill is SequenceSkill seq) SequenceSkills.Add(seq);
                break;

            case Define.SkillType.EnergyRing:
            case Define.SkillType.ElectronicField:
            case Define.SkillType.SpectralSlash:
                var go = resourceM.Instantiate(_type.ToString(), parent);
                if (go != null) skill = go.GetOrAddComponent<SkillBase>();
                break;

            default:
                skill = gameObject.AddComponent(Type.GetType(_type.ToString())) as SkillBase;
                break;
        }

        if (skill == null) return;
        skill.UpdateSkillData();
        skillList.Add(skill);

        SavedBattleSkill[_type] = skill.SkillLevel;
        gameM.ContinueDatas.SavedBattleSkill = SavedBattleSkill;
    }

    public void AddSpecialSkill(Data.SpecialSkillData _skill, bool _isLoadSkill = false)
    {
        _skill.IsLearned = true;
        SpecialSkills.Add(_skill);

        if (_skill.SpecialSkillName == Define.SpecialSkillName.Healing)
        {
            Manager.GameM.player.SpecialSkillHealCount++;
        }

       

        if (_isLoadSkill)
        {
            foreach (SkillBase playerSkill in skillList)
            {
                if (_skill.SpecialSkillName.ToString() == playerSkill.Skilltype.ToString())
                    playerSkill.UpdateSkillData();
                
            }
        }
        else if (_skill.SkillType == Define.SpecialSkillType.General)
        {
            GeneralSpecialSkill(_skill);
        }
        else
        {
            foreach (SkillBase playerSkill in skillList)
            {
                if (_skill.SpecialSkillName.ToString() == playerSkill.Skilltype.ToString())
                    playerSkill.UpdateSkillData();
            }
        }
        Manager.GameM.ContinueDatas.SavedSpecialSkill.Add(_skill);
        UpdateSkillUI?.Invoke();
        gameM.SaveGame();
    }

    public void LoadSkill(Define.SkillType _skillType, int _level)
    {
        AddSkill(_skillType);
        for(int i = 0; i<_level; i++)
        {
            LevelUpSkill(_skillType);
        }
    }

    public void LevelUpSkill(Define.SkillType _type)
    {   
        for(int i =0; i< skillList.Count; i++)
        {
            var sb = skillList[i];

            if (sb.Skilltype != _type) continue;
            if (sb.SkillLevel > 6) continue;
            sb.OnSkillLevelup();

            SavedBattleSkill[_type] = sb.SkillLevel;
            gameM.ContinueDatas.SavedBattleSkill = SavedBattleSkill;

            UpdateSkillUI?.Invoke();
            gameM.SaveGame();

            break;
        }
    }

    public List<SkillBase> RecommendSkills()
    {
        var all = skillList;
        var active = new List<SkillBase>(Define.MAX_SKILL_COUNT);
        for (int i = 0; i < all.Count; i++)
            if (all[i].isLearnSkill) active.Add(all[i]);

        var candidates = (active.Count == Define.MAX_SKILL_COUNT) 
            ? active.FindAll(s => s.SkillLevel < Define.MAX_SKILL_LEVEL) 
            : all.FindAll(s => s.SkillLevel < Define.MAX_SKILL_LEVEL);

        for(int i =0; i<candidates.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, candidates.Count);
            (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
        }

        return candidates.Count <= 3 ? candidates : candidates.GetRange(0, 3);

        //List<SkillBase> skillList = Manager.GameM.player.Skills.skillList.ToList();
        //List<SkillBase> activeSkills = skillList.FindAll(skill => skill.isLearnSkill);

        //List<SkillBase> recommendSkills;
        //if(activeSkills.Count == Define.MAX_SKILL_COUNT)
        //{
        //    recommendSkills = activeSkills.FindAll(s => s.SkillLevel < Define.MAX_SKILL_LEVEL);
        //}
        //else
        //{
        //    recommendSkills = skillList.FindAll(s => s.SkillLevel < Define.MAX_SKILL_LEVEL);

        //}
        //recommendSkills.Shuffle();
        //return recommendSkills.Take(3).ToList();
    }

    public List<int> GetAvailableEvolutionItems()
    {
        List<int> evoItems = new List<int>();

        for(int i =0; i< skillList.Count; i++)
        {
            var sb = skillList[i];
            if (!sb.isLearnSkill || sb.SkillLevel != 5) continue;

            int id = sb.SkillDatas.EvolutionItemID;
            if (id > 0) evoItems.Add(id);
        }

        return evoItems;
    }


    public List<object> GetSkills()
    {
        List<SkillBase> baseSkillCandidates = RecommendSkills();
        List<int> evoItemCandidates = GetAvailableEvolutionItems();
        List<object> finalCandidates = new List<object>(baseSkillCandidates.Count + evoItemCandidates.Count);

        finalCandidates.AddRange(baseSkillCandidates);
        finalCandidates.AddRange(evoItemCandidates);
        
        for(int i =0; i<finalCandidates.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, finalCandidates.Count);
            (finalCandidates[i], finalCandidates[j]) = (finalCandidates[j], finalCandidates[i]);
        }

        return finalCandidates.Count <= 3 ? finalCandidates : finalCandidates.GetRange(0, 3);
    }

   
    public void TryEvolveSkill(int _evolutionItemID)
    {
        for(int i =0; i<skillList.Count; i++)
        {
            var sb = skillList[i];
            if (!sb.isLearnSkill || !sb.isCanEvolve()) continue;
            if (sb.SkillDatas.EvolutionItemID != _evolutionItemID) continue;

            evolutionItemList.Add(_evolutionItemID);
            sb.Evolution();

            SavedEvolutionSkill[sb.Skilltype] = _evolutionItemID;
            gameM.ContinueDatas.SavedEvolutionSkill = SavedEvolutionSkill;

            UpdateSkillUI?.Invoke();
            gameM.SaveGame();
            break;
        }
    }


    public void GeneralSpecialSkill(Data.SpecialSkillData _skill)
    {
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

    public void RefreshSkillUI()
    {
        UpdateSkillUI?.Invoke();
    }


    #region Boss Skill
    int sequenceIndex = 0;
    public void StartNextSequenceSkill()
    {
        if (stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;

        SequenceSkills[sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }
    void OnFinishedSequenceSkill()
    {
        sequenceIndex = (sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    public void StopSkills() => stopped = true;
    #endregion
}
