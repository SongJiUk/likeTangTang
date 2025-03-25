using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSwordController : SkillController
{
    [SerializeField]
    ParticleSystem[] swingParticle;

    public int SkillLevel = 1;
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

        FindChild();

        for(int i= 0; i< swingParticle.Length; i++)
            swingParticle[i].GetComponent<Rigidbody2D>().simulated = false;

        for (int i = 0; i < swingParticle.Length; i++)
            swingParticle[i].gameObject.GetOrAddComponent<EgoSwordChild>().SetInfo(Manager.ObjectM.Player, 1000);

        return true;
    }

    public void FindChild()
    {
        if (swingParticle.Length != 0) return;

        int childCount = transform.childCount;

        swingParticle = new ParticleSystem[childCount];

        for (int i =0; i< childCount; i++)
            swingParticle[i] =  transform.GetChild(i).GetComponent<ParticleSystem>();
    }
    public void ActivateSkill()
    {
        StartCoroutine(coSwingSword());
    }

    float coolTime = 2f;
    IEnumerator coSwingSword()
    {
        while(true)
        {
            // [ ] 레벨별로 나눠서 1레벨이면 한번만 스윙하게끔( continue 사용하면 될듯)
            yield return new WaitForSeconds(coolTime);


            SetParticles(SwingType.First);
            swingParticle[(int)SwingType.First].Play();
            TurnOnOffPhysics(SwingType.First, true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.First].main.duration);
            TurnOnOffPhysics(SwingType.First, false);

            //if (SkillLevel <= 1) continue;

            SetParticles(SwingType.Second);
            swingParticle[(int)SwingType.Second].Play();
            TurnOnOffPhysics(SwingType.Second, true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.Second].main.duration);
            TurnOnOffPhysics(SwingType.Second, false);

            //if (SkillLevel <= 2) continue;

            SetParticles(SwingType.Third);
            swingParticle[(int)SwingType.Third].Play();
            TurnOnOffPhysics(SwingType.Third, true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.Third].main.duration);
            TurnOnOffPhysics(SwingType.Third, false);

            //if (SkillLevel <= 3) continue;

            SetParticles(SwingType.Fourth);
            swingParticle[(int)SwingType.Fourth].Play();
            TurnOnOffPhysics(SwingType.Fourth, true);
            yield return new WaitForSeconds(swingParticle[(int)SwingType.Fourth].main.duration);
            TurnOnOffPhysics(SwingType.Fourth, false);
        }
    }

    void SetParticles(SwingType _swingType)
    {
        //MEMO : 플레이어의 회전각도에 따라, 반지름을 구하고, 파티클의 시작 로테이션을 지정해준다.
        float z = transform.parent.transform.eulerAngles.z;
        float rad = (Mathf.PI / 180) * z * -1;

        var main = swingParticle[(int)_swingType].main;
        main.startRotation = rad;
    }

    void TurnOnOffPhysics(SwingType _swingType ,bool _isOn)
    {
        for(int i =0; i<swingParticle.Length; i++)
            swingParticle[i].GetComponent<Rigidbody2D>().simulated = false;

        swingParticle[(int)_swingType].GetComponent<Rigidbody2D>().simulated = _isOn;
    }

}
