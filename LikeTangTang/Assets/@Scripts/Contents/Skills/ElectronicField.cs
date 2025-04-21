using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public interface IDotTickable
{
    float TickInterval { get; }
    void OnDotTick(CreatureController _cc);
}

public class DotTargetTracker
{ 
    class DotTarget
    {
        public CreatureController target;
        public float LastTick;

        public DotTarget(CreatureController _target, float _currentTime)
        {
            target = _target;
            LastTick = _currentTime;
        }
    }

    private readonly MonoBehaviour owner;
    private readonly IDotTickable tickSource;
    private readonly List<DotTarget> targets = new();
    private Coroutine coroutine;

    public DotTargetTracker(MonoBehaviour _owner, IDotTickable _tickSource)
    {
        owner = _owner;
        tickSource = _tickSource;
    }

    public void Add(CreatureController _target)
    {
        if (targets.Exists(t => t.target == _target)) return;

        targets.Add(new DotTarget(_target, Time.time - tickSource.TickInterval));

        if (coroutine == null)
            coroutine = owner.StartCoroutine(CoTick());
    }

    public void Remove(CreatureController _target)
    {
        targets.RemoveAll(t => t.target == _target);

        if (targets.Count == 0 && coroutine != null)
        {
            owner.StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator CoTick()
    {
        while (true)
        {
            float now = Time.time;

            foreach (var t in targets)
            {
                if (!t.target.IsValid())
                    continue;

                if (now - t.LastTick >= tickSource.TickInterval)
                {
                    t.LastTick = now;
                    tickSource.OnDotTick(t.target);
                }
            }

            yield return null;
        }
    }
}


public class ElectronicField : RepeatSkill, IDotTickable
{
    [SerializeField]
    GameObject normalEffect;
    [SerializeField]
    GameObject evolutionEffect;


    HashSet<MonsterController> targets = new HashSet<MonsterController>();

    DotTargetTracker dotTracker;
    public float TickInterval => SkillDatas.AttackInterval;

    public void OnDotTick(CreatureController _cc)
    {
        _cc.OnDamaged(Manager.GameM.player, this);
    }

    void Awake()
    {
        Skilltype = Define.SkillType.ElectronicField;
        gameObject.SetActive(false);

    }
    public override void DoSkill()
    {
        
    }

    public override void ActivateSkill()
    {
        gameObject.SetActive(true);
        if(SkillDatas == null ) base.ActivateSkill();

        dotTracker = new DotTargetTracker(this, this);
    }
    

    public void OnEvolutaion()
    {
        normalEffect.SetActive(false);
        evolutionEffect.SetActive(true);

        transform.localScale = Vector3.one * SkillDatas.ScaleMultiplier;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController cc = collision.GetComponent<MonsterController>();
        if(!cc.IsValid()) return;
        if(!cc.IsMonster()) return;

        dotTracker?.Add(cc);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController cc = collision.GetComponent<MonsterController>();
        if(!cc.IsValid()) return;
        if(!cc.IsMonster()) return;

        dotTracker?.Remove(cc);
    }
}
