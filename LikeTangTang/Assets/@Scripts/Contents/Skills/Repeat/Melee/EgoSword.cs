using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class EgoSword : RepeatSkill
{
    [SerializeField]
    ParticleSystem[] swingParticle;

    protected enum SwingType
    {
        First,
        Second,
        Third,
        Fourth
    }
    public override bool Init()
    {
        base.Init();
        SetInfo(Manager.GameM.player);
        FindChild();
        return true;
    }
    public void SetInfo(CreatureController _owner, SkillData _skillData = null)
    {
        owner = _owner;
        coolTime = 2f;
        if(_skillData != null) SkillDatas = _skillData;
    }

    public void FindChild()
    {
        int childCount = transform.childCount;

        if (swingParticle.Length == childCount) return;

        swingParticle = new ParticleSystem[childCount];

        for (int i =0; i< childCount; i++)
            swingParticle[i] =  transform.GetChild(i).GetComponent<ParticleSystem>();
    }
    public override void ActivateSkill()
    {
        StartCoroutine(coStartSkill());
    }
    void SetParticles(SwingType _swingType)
    {
        if(Manager.GameM.player == null) return;

        //MEMO : 플레이어의 회전각도에 따라, 반지름을 구하고, 파티클의 시작 로테이션을 지정해준다.
        float z = transform.parent.transform.eulerAngles.z;
        float rad = (Mathf.PI / 180) * z * -1;

        var main = swingParticle[(int)_swingType].main;
        main.startRotation = rad;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();

        if (mc.IsVaild() == false) return;
        // [ ] 데이터값으로 변경
        //mc.OnDamaged(owner, 1000);//SkillDatas.damage);

    }


    public override void DoSkill()
    {
        
    }
    protected override IEnumerator coStartSkill()
    {
        WaitForSeconds waitTime = new WaitForSeconds(coolTime);
        while(true)
        {
            // [ ] 레벨별로 나눠서 1레벨이면 한번만 스윙하게끔( continue 사용하면 될듯)
            SetParticles(SwingType.First);
            swingParticle[(int)SwingType.First].gameObject.SetActive(true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.First].main.duration);

            //if (SkillLevel <= 1) continue;

            SetParticles(SwingType.Second);
            swingParticle[(int)SwingType.Second].gameObject.SetActive(true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.Second].main.duration);

            //if (SkillLevel <= 2) continue;

            SetParticles(SwingType.Third);
            swingParticle[(int)SwingType.Third].gameObject.SetActive(true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.Third].main.duration);

            //if (SkillLevel <= 3) continue;

            SetParticles(SwingType.Fourth);
            swingParticle[(int)SwingType.Fourth].gameObject.SetActive(true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.Fourth].main.duration);

            yield return waitTime;
        }
    }

}
