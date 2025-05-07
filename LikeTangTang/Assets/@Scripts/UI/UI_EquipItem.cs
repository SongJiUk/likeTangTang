using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_EquipItem : UI_Base
{
    public Equipment Equipment;
    public Action OnClickEquipItem;
    Define.UI_ItemParentType parentType;

    public void SetInfo(Equipment _item, Define.UI_ItemParentType _parentType)
    {
        Equipment = _item;
        parentType = _parentType;

    }
}
