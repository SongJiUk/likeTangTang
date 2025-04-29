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
        if( anim != null) anim.runtimeAnimatorController = Manager.ResourceM.Load<RuntimeAnimatorController>($"{_dropItem.AnimName}");
    }

    public override void CompleteGetItem()
    {

        float healAmount;

        if(Define.POTION_AMOUNT.TryGetValue(dropItem.DataID , out healAmount))
        {
            Manager.GameM.player.Healing(healAmount);
        }

        Manager.ObjectM.DeSpawn(this);
    }
}
