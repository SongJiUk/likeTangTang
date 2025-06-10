using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpectralSlash : RepeatSkill
{  
    [SerializeField]
    ParticleSystem[] swingParticle;
    [SerializeField]
    ParticleSystem[] EvolutionSwingParticle;

    ParticleSystem[] currentSwingParticle;

    Coroutine coSwing;

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
        if(coSwing != null) StopCoroutine(coSwing);
        coSwing = StartCoroutine(CoStartSpectralSlash());
    }

    public override void OnChangedSkillData()
    {
        projectileCount = SkillDatas.ProjectileCount;
        UpdateCurrentSwingParticle();

    }

    void UpdateCurrentSwingParticle()
    {

        currentSwingParticle = SkillLevel >=6 ? EvolutionSwingParticle : swingParticle;
    }

    void SetParticleRotationAndPosition(ParticleSystem _particle)
    {
        if(Manager.GameM.player == null || transform.parent == null) return;

        //MEMO : 플레이어의 회전각도에 따라, 반지름을 구하고, 파티클의 시작 로테이션을 지정해준다.
        float z = -transform.parent.eulerAngles.z;
        float rad = Mathf.Deg2Rad * z;

        var main = _particle.main;
        main.startRotation = rad;
        _particle.transform.position = Manager.GameM.player.Standard.position;
    }
    IEnumerator CoStartSpectralSlash()
    {
        var waitTime = new WaitForSeconds(SkillDatas.CoolTime * (1 - Manager.GameM.CurrentCharacter.Evol_CoolTimeBouns));
        while(true)
        {
            int swingCount = Mathf.Min(projectileCount, currentSwingParticle.Length);
            for(int i = 0; i< swingCount; i++)
            {   
                var particle = currentSwingParticle[i];
                if(particle == null) continue;

                SetParticleRotationAndPosition(particle);
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
}
