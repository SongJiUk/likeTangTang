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
    void Awake()
    {
        Skilltype = Define.SkillType.EnergyRing;
        gameObject.SetActive(false);
        SetActiveSpinner(false);
        coolTime = 0f;
        
    }

    private void OnEnable() 
    {
        if(Manager.UpdateM != null) Manager.UpdateM.Register(this);
    }

    // void OnDisable()
    // {
    //     if(Manager.UpdateM != null) Manager.UpdateM.Unregister(this);
    // }
    public override void ActivateSkill()
    {
        if(SkillDatas == null) base.ActivateSkill();
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
        if(isPlaying) return;

        coolTime -= _deltaTime;
        if(coolTime <= 0f)
        {
            DoSkill();
            coolTime = SkillDatas.CoolTime;
        }
    }
    public void SetEnergyRing()
    {
        transform.localPosition = Vector3.zero;

        for(int i =0; i<spinner.Length; i++)
        {
            if(i < SkillDatas.ProjectileCount)
            {
                spinner[i].SetActive(true);
                float degree = 360f / SkillDatas.ProjectileCount * i;
                spinner[i].transform.localPosition = Quaternion.Euler(0f,0f,degree) * Vector3.up * SkillDatas.Range;
                spinner[i].transform.localScale = Vector3.one * SkillDatas.ScaleMultiplier;
            }
            else
            {
                spinner[i].SetActive(false);
            }
        }
    }
    public void BackEnergyRing()
    {
        for(int i =0; i<spinner.Length; i++)
        {
            if(i <SkillDatas.ProjectileCount)
            {
                spinner[i].transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.InSine);
                spinner[i].transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InSine);
            }
            else
            {
                spinner[i].SetActive(false);
            }
        }
    }

    public override void DoSkill()
    {
        if(isPlaying) return;
        isPlaying = true;

        SetEnergyRing();

        Sequence enableSequence = DOTween.Sequence();
        transform.rotation = Quaternion.identity;

        float totalRotaion = SkillDatas.RoatateSpeed * SkillDatas.Duration;
        Tween scaleUp = transform.DOScale(1f, 0.2f);
        Tween rotateMain = transform.DORotate(new Vector3(0, 0, totalRotaion), SkillDatas.Duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);

        Tween scaleDown = transform.DOScale(0f, 1f);
        Tween rotateEnd = transform.DORotate(new Vector3(0, 0, totalRotaion), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear);


        enableSequence.Append(scaleUp).Join(rotateMain)
        .Append(scaleDown).Join(rotateEnd)
        .AppendCallback(() => BackEnergyRing())
        .AppendCallback(() => isPlaying = false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.transform.GetComponent<CreatureController>();

        if(cc.IsValid() == false) return;
        if(cc.IsMonster()) cc.OnDamaged(Manager.GameM.player, this);    
    }
}
