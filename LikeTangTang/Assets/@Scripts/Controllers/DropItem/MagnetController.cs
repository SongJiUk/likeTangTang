using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : DropItemController
{
    public override bool Init()
    {
        base.Init();
        itemType = Define.ItemType.Magnet;
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
    }

    public override void CompleteGetItem()
    {
        
    }
}
