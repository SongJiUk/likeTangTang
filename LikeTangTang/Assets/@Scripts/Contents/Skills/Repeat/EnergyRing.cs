using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Data;
using UnityEngine.UIElements;
public class EnergyRing : RepeatSkill, ITickable
{
    public GameObject[] spinner;
    public GameObject[] evolutionSpinner;

    GameObject[] currentSpinner;
    bool isPlaying = false;
    float rotationAngle = 0f;
    float durationTimer = 0f;
    void Awake()
    {
        Skilltype = Define.SkillType.EnergyRing;
        gameObject.SetActive(false);
        coolTime = 0f;
    }

    private void OnDestroy()
    {
        Manager.UpdateM.Unregister(this);
    }

    public override void ActivateSkill()
    {
        if(SkillDatas == null) base.ActivateSkill();
        Manager.UpdateM.Register(this);

        gameObject.SetActive(true);
        UpdateCurrentSpinner();  
        SetActiveSpinner(true);
        DoSkill();
    }


   

    public override void OnChangedSkillData()
    {
        UpdateCurrentSpinner();
        SetActiveSpinner(true);
        SetEnergyRing();
    }

    public void UpdateCurrentSpinner()
    {
        currentSpinner = (SkillLevel == 6) ? evolutionSpinner : spinner;
    }

    public void SetActiveSpinner(bool _isActive)
    {
        foreach(GameObject go in spinner) go.SetActive(false);
        foreach(GameObject go in evolutionSpinner) go.SetActive(false);

        foreach(GameObject go in currentSpinner) go.SetActive(_isActive);
        
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
                currentSpinner[i].transform.position = playerPos + offset;
                currentSpinner[i].transform.rotation = Quaternion.identity;
                currentSpinner[i].transform.localScale = Vector3.one * SkillDatas.ScaleMultiplier;
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
            coolTime = SkillDatas.CoolTime * (1 - Manager.GameM.CurrentCharacter.Evol_CoolTimeBouns);
        }
    }

    public void SetEnergyRing()
    {
        rotationAngle = 0f;

        int count = Mathf.Min(SkillDatas.ProjectileCount, currentSpinner.Length);
        for (int i = 0; i < currentSpinner.Length; i++)
        {
            currentSpinner[i].SetActive(i < count);
            currentSpinner[i].transform.localScale = (i < count) ? Vector3.one * SkillDatas.ScaleMultiplier : Vector3.zero;
        }
    }
    public void BackEnergyRing()
    {
        for(int i =0; i<currentSpinner.Length; i++)
        {
            if(i <SkillDatas.ProjectileCount)
            {
                currentSpinner[i].transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InSine)
                    .OnComplete(() => currentSpinner[i].SetActive(false));
            }
            else
            {
                currentSpinner[i].SetActive(false);
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
