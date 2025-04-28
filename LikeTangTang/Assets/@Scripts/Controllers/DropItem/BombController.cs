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
    }

    public void SetInfo(Data.DropItemData _data)
    {

    }
}
