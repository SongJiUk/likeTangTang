using Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
         if(ItemSprite != null) 
            ItemSprite.sprite = Manager.ResourceM.Load<Sprite>(dropItem.SpriteName);

        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public override void CompleteGetItem()
    {
        Manager.ObjectM.ColletAllItem();
        Manager.ObjectM.DeSpawn(this);
    }
}
