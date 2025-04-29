using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class PotionController : DropItemController
{
    
    

    public override bool Init()
    {
        base.Init();
        itemType = Define.ItemType.Potion;

        return true;
    }

    public override void GetItem()
    {
        base.GetItem();
        if(coGetItem == null && this.IsValid())
        {
            coGetItem = StartCoroutine(CoCheckDist());
        }

    }

    public override void SetInfo(DropItemData _dropItem)
    {
        dropItem = _dropItem;
        transform.localScale = new Vector3(2f, 2f, 2f);
        if(ItemSprite != null) 
            ItemSprite.sprite = Manager.ResourceM.Load<Sprite>(dropItem.SpriteName);
        if( anim != null) 
            anim.runtimeAnimatorController = Manager.ResourceM.Load<RuntimeAnimatorController>($"{_dropItem.AnimName}");
    }

    public override void CompleteGetItem()
    {
        base.CompleteGetItem();
        float healAmount;

        if(Define.POTION_AMOUNT.TryGetValue(dropItem.DataID , out healAmount))
        {
            Manager.GameM.player.Healing(healAmount);
        }

        Manager.ObjectM.DeSpawn(this);
    }
}
