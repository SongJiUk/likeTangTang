using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : DropItemController
{  

    public override bool Init()
    {
        base.Init();
        itemType = Define.ItemType.Bomb;
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

    public override void SetInfo(Data.DropItemData _dropItem)
    {
        dropItem = _dropItem;
         if(ItemSprite != null) ItemSprite.sprite = Manager.ResourceM.Load<Sprite>(dropItem.SpriteName);
         transform.localScale = Vector3.one;
    }

    public override void CompleteGetItem()
    {
        Manager.ObjectM.KillAllMonsters();
        Manager.ObjectM.DeSpawn(this);
    }
}
