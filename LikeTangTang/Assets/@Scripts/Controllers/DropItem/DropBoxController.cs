using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : DropItemController
{
    GameObject effect;
    DropItemData dropItem;
    public override bool Init()
    {
        base.Init();
        itemType = Define.ItemType.DropBox;

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

       //TODO : 여기서 아이템 세팅
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
        //TODO : 상자마다, 다르게 해줘야함(뭘 할지 생각해봐야됌)
        switch (dropItem.Grade)
        {
            case Define.ItemGrade.Normal:
                
                break;
            case Define.ItemGrade.Rare:
                break;

            case Define.ItemGrade.Unique:
                break;

        }


        base.CompleteGetItem();
        Manager.ResourceM.Destory(effect);
        Manager.ObjectM.DeSpawn(this);
    }
}
