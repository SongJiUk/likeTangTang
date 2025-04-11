using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Data;
public class EnergyRing : RepeatSkill
{
    public GameObject[] spinner;
    void Awake()
    {
        Skilltype = Define.SkillType.EnergyRing;
        gameObject.SetActive(false);
        SetActiveSpinner(false);
    }

    public override void ActivateSkill()
    {
        gameObject.SetActive(true);
        if(SkillDatas == null) base.ActivateSkill();
        SetActiveSpinner(true);
        ActiveSpinner();
    }


    public override void DoSkill()
    {

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

    public void ActiveSpinner()
    {
        SetEnergyRing();

        Sequence enableSequence = DOTween.Sequence();
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;

        float totalRotaion = SkillDatas.RoatateSpeed * SkillDatas.Duration;
        Tween scaleUp = transform.DOScale(1f, 0.2f);
        Tween rotateMain = transform.DORotate(new Vector3(0, 0, totalRotaion), SkillDatas.Duration, RotateMode.FastBeyond360).SetEase(Ease.Linear);

        Tween scaleDown = transform.DOScale(0f, 1f);
        Tween rotateEnd = transform.DORotate(new Vector3(0, 0, totalRotaion), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

        // enableSequence.Append(scaleUp).Join(rotateMain)
        //           .Append(scaleDown).Join(rotateEnd)
        //           .InsertCallback(SkillDatas.Duration, () => 
        //           {  
        //               BackEnergyRing();
        //               gameObject.SetActive(false);
        //           })
        //           .AppendInterval(SkillDatas.CoolTime- 1f)
        //           .AppendCallback(() => ActiveSpinner());

             enableSequence.Append(scaleUp).Join(rotateMain)
            .Append(scaleDown).Join(rotateEnd)
            .AppendCallback(() => BackEnergyRing())
            .InsertCallback(0.5f, () => gameObject.SetActive(false))
            .AppendInterval(SkillDatas.CoolTime - 0.5f)
            .AppendCallback(() => ActiveSpinner());

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        CreatureController cc = collision.transform.GetComponent<CreatureController>();

        if(cc.IsVaild() == false) return;
        if(cc.IsMonster()) cc.OnDamaged(Manager.GameM.player, this);    
    }
}
