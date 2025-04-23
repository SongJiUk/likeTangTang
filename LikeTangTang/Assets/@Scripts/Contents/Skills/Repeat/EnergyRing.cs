using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Data;
using UnityEngine.UIElements;
public class EnergyRing : RepeatSkill, ITickable
{
    public GameObject[] spinner;
    bool isPlaying = false;
    float rotationAngle = 0f;
    float durationTimer = 0f;
    void Awake()
    {
        Skilltype = Define.SkillType.EnergyRing;
        gameObject.SetActive(false);
        SetActiveSpinner(false);
        coolTime = 0f;
    }

  

    public override void ActivateSkill()
    {
        if(SkillDatas == null) base.ActivateSkill();
        Manager.UpdateM.Register(this);

        gameObject.SetActive(true);
        SetActiveSpinner(true);
        DoSkill();
    }


   

    public override void OnChangedSkillData()
    {
        SetActiveSpinner(true);
        SetEnergyRing();
    }

    public void SetActiveSpinner(bool _isActive)
    {
        if(SkillLevel == 6)
        {

        }
        else
        {
            foreach(GameObject go in spinner)
            {
                go.SetActive(_isActive);
            }
        }
    }
    public void Tick(float _deltaTime)
    {
        if (!gameObject.activeSelf || spinner == null || spinner.Length == 0) return;

        if (isPlaying)
        {
            Vector3 playerPos = Manager.GameM.player.transform.position;

            rotationAngle += SkillDatas.RoatateSpeed * _deltaTime;
            rotationAngle %= 360f;

            int count = Mathf.Min(SkillDatas.ProjectileCount, spinner.Length);

            for (int i = 0; i < count; i++)
            {
                float angle = rotationAngle + (360 / count) * i;
                float rad = angle * Mathf.Deg2Rad;

                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * SkillDatas.Range;
                spinner[i].transform.position = playerPos + offset;
                spinner[i].transform.rotation = Quaternion.identity;
                spinner[i].transform.localScale = Vector3.one * SkillDatas.ScaleMultiplier;
            }



            durationTimer -= _deltaTime;
            if (durationTimer <= 0f)
            {
                BackEnergyRing();
                isPlaying = false;
            }

            return;
        }

        coolTime -= _deltaTime;
        if(coolTime <= 0f)
        {
            DoSkill();
            coolTime = SkillDatas.CoolTime;
        }
    }

    public void SetEnergyRing()
    {
        rotationAngle = 0f;

        int count = Mathf.Min(SkillDatas.ProjectileCount, spinner.Length);
        for (int i = 0; i < spinner.Length; i++)
        {
            spinner[i].SetActive(i < count);
            spinner[i].transform.localScale = (i < count) ? Vector3.one * SkillDatas.ScaleMultiplier : Vector3.zero;
        }
    }
    public void BackEnergyRing()
    {
        for(int i =0; i<spinner.Length; i++)
        {
            if(i <SkillDatas.ProjectileCount)
            {
                spinner[i].transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InSine)
                    .OnComplete(() => spinner[i].SetActive(false));
            }
            else
            {
                spinner[i].SetActive(false);
            }
        }

        transform.rotation = Quaternion.identity;
    }

    public override void DoSkill()
    {
        if(isPlaying) return;
        isPlaying = true;

        durationTimer = SkillDatas.Duration;
        SetEnergyRing();

    }

    private void OnDisable()
    {
        isPlaying = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.transform.GetComponent<CreatureController>();

        if(cc.IsValid() == false) return;
        if(cc.IsMonster()) cc.OnDamaged(Manager.GameM.player, this);    
    }
}
