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
    public override void DoSkill(){ }
    public override void ActivateSkill()
    {
        gameObject.SetActive(true);
        base.ActivateSkill();
        OnChangedSkillData();
        StartCoroutine(CoStartSpectralSlash());
    }

    public override void OnChangedSkillData()
    {
        projectileCount = SkillDatas.ProjectileCount;
    }


    void SetParticleRotation(ParticleSystem _particle)
    {
        if(Manager.GameM.player == null || transform.parent == null) return;

        //MEMO : 플레이어의 회전각도에 따라, 반지름을 구하고, 파티클의 시작 로테이션을 지정해준다.
        float z = transform.parent.transform.eulerAngles.z;
        float rad = Mathf.Deg2Rad * z * -1;

        var main = _particle.main;
        main.startRotation = rad;   
    }
    IEnumerator CoStartSpectralSlash()
    {
        var waitTime = new WaitForSeconds(SkillDatas.CoolTime);
        while(true)
        {
            int swingCount = Mathf.Min(projectileCount, swingParticle.Length);
            for(int i = 0; i< swingCount; i++)
            {   
                var particle = swingParticle[i];
                if(particle == null) continue;

                SetParticleRotation(particle);
                particle.gameObject.SetActive(true);

                yield return new WaitForSeconds(particle.main.duration);
            }

            yield return waitTime;    
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        

        if(collision.TryGetComponent<MonsterController>(out MonsterController mc) && mc.IsValid())
        {
            mc.OnDamaged(Manager.GameM.player, this);
        }

    }

    // void OnTriggerExit2D(Collider2D collision)
    // {
    //     MonsterController mc = collision.transform.GetComponent<MonsterController>();
    //     if(!mc.IsValid()) return;

        
    // }
}
