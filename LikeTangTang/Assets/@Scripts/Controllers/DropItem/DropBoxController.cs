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

        return true;
    }

   
    public override void SetInfo(DropItemData _dropItem)
    {
        base.SetInfo(_dropItem);
        
        if(ItemSprite != null) 
            ItemSprite.sprite = Manager.ResourceM.Load<Sprite>(_dropItem.SpriteName);

        if (anim != null) anim.runtimeAnimatorController = null;
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        SpawnEffect(_dropItem);
    }

    public void SpawnEffect(DropItemData _dropItem)
    {
        effect = Manager.ResourceM.Instantiate(_dropItem.EffectName, transform);
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
        //TODO : UI레벨업 Popup
        base.CompleteGetItem();
        Manager.ResourceM.Destory(effect);
        Manager.ObjectM.DeSpawn(this);
    }
}
