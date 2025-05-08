using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentInfoPopup : UI_Popup
{
    Equipment equipment;
    private void Awake()
    {
        Init();
    }


    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObject.SetActive(false);

        return true;
    }
    public void SetInfo(Equipment _equipment)
    {
        equipment = _equipment;
        
    }
}
