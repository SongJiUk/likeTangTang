using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MaterialItem : UI_Base
{
    Data.MaterialData materialData;
    public void SetInfo(Data.MaterialData _data, Transform _parent, int _count)
    {
        transform.localScale = Vector3.one;
        materialData = _data;

        //TODO : Color

    }
}
