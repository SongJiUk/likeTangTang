using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : DropItemController
{
    GameObject effect;
    public override bool Init()
    {
        base.Init();
        itemType = Define.ItemType.DropBox;
        Manager.SoundM.Play(Define.Sound.Effect, "Drop_Box");

        return true;
    }

   
    public override void SetInfo(DropItemData _dropItem)
    {
        dropItem = _dropItem;
        base.SetInfo(_dropItem);
        
        if(ItemSprite != null) 
            ItemSprite.sprite = Manager.ResourceM.Load<Sprite>(dropItem.SpriteName);

        if (anim != null) anim.runtimeAnimatorController = null;
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        SpawnEffect();
    }

    public void SpawnEffect()
    {
        effect = Manager.ResourceM.Instantiate(dropItem.EffectName, transform);
    }


    public override void GetItem()
    {
        base.GetItem();
        if(coGetItem == null && this.IsValid())
        {
            coGetItem = StartCoroutine(CoCheckDist());
        }
    }

    public override void CompleteGetItem()
    {
        PlayerController player = Manager.GameM.player;
        float needExp = 0;
        switch (dropItem.Grade)
        {
            case Define.ItemGrade.Normal:
                //Heal
                player.SpecialSkillHealCount++;
                if (player != null) player.Skills.RefreshSkillUI();

                break;
            case Define.ItemGrade.Rare:
                // LEvelUP
                needExp = player.TotalExp - player.Exp;
                player.Exp += needExp;
                break;

            case Define.ItemGrade.Unique:
                //HEAL + LEvelUP
                Manager.GameM.player.SpecialSkillHealCount++;
                if (Manager.GameM.player != null) Manager.GameM.player.Skills.RefreshSkillUI();

                needExp = player.TotalExp - player.Exp;
                player.Exp += needExp;

                break;

        }


        base.CompleteGetItem();
        Manager.ResourceM.Destory(effect);
        Manager.ObjectM.DeSpawn(this);
    }
}
