using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpectralSlash : RepeatSkill
{  
    [SerializeField]
    ParticleSystem[] swingParticle;
    enum SwingType
    {
        First,
        Second,
        Third,
        Fourth
    }
    void Awake()
    {
        Skilltype = Define.SkillType.SpectralSlash;
        gameObject.SetActive(false);

    }
    public override void DoSkill()
    {
        

    }
    public override void ActivateSkill()
    {
        gameObject.SetActive(true);
        base.ActivateSkill();
        OnChangedSkillData();
        StartCoroutine(CoStartSpectralSlash());
    }

    public override void OnChangedSkillData()
    {
        SetSpectralSlash();
    }
    public void SetSpectralSlash()
    {
        projectileCount = SkillDatas.ProjectileCount;

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
    IEnumerator CoStartSpectralSlash()
    {
        var waitTime = new WaitForSeconds(SkillDatas.CoolTime);
        while(true)
        {
            for(int i = 0; i< projectileCount; i++)
            {
                SetParticles((SwingType)i);
                swingParticle[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(swingParticle[i].main.duration);
            }
            yield return waitTime;    
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        if(!mc.IsValid()) return;
        mc.OnDamaged(Manager.GameM.player, this);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        MonsterController mc = collision.transform.GetComponent<MonsterController>();
        if(!mc.IsValid()) return;

        
    }
}
