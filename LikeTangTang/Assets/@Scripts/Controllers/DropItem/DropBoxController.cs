using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxController : DropItemController
{

    enum DropItemBoxGrade
    { 
        Normal,
        Rare,
        Unique
    }

    DropItemBoxGrade boxGrade;

    public override bool Init()
    {
        base.Init();
        
        return true;
    }
    public override void SetInfo(DropItemData _dropItem)
    {
        base.SetInfo(_dropItem);
        
        if(ItemSprite != null) 
            ItemSprite.sprite = Manager.ResourceM.Load<Sprite>(_dropItem.SpriteName);

        if (!Enum.TryParse(_dropItem.Grade, out boxGrade))
            Debug.LogError($"[DropBoxController] 잘못된 Grade : {_dropItem.Grade}");
        
            


        SpawnEffect(_dropItem);
    }

    public void SpawnEffect(DropItemData _dropItem)
    {
        Manager.ResourceM.Instantiate(_dropItem.EffectName, transform);
    }

    // TODO : 획득하는 코드 
}
