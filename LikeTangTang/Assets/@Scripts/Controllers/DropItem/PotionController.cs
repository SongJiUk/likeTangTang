using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class PotionController : DropItemController
{
    Animator anim;

    public override bool Init()
    {
        base.Init();
        itemType = Define.ItemType.Potion;
        anim = GetComponent<Animator>();

        return true;
    }

    public override void GetItem()
    {
        base.GetItem();

    }

    public override void SetInfo(DropItemData _dropItem)
    {
        anim.runtimeAnimatorController = Manager.ResourceM.Load<RuntimeAnimatorController>($"{_dropItem.AnimName}");
    }
}
