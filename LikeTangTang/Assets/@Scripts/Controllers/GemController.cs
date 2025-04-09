using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class GemInfo
{   

    public enum GemType
    {
        Small,
        Green,
        Blue,
        Yellow
    }

    public GemType gemType;
    public string SpriteName;
    public Vector3 GemScale;
    public int ExpAmount;

    public GemInfo(GemType _gemType, Vector3 _gemScale)
    {
        gemType = _gemType;
        SpriteName = $"{_gemType}Gem.sprite";
        GemScale = _gemScale;
        switch(_gemType)
        {
            case GemType.Small :
                ExpAmount = Define.SMALL_GEM_EXP;
            break;
            case GemType.Green :
                ExpAmount = Define.GREEN_GEM_EXP;
            break;
            case GemType.Blue :
                ExpAmount = Define.BLUE_GEM_EXP;
            break;
            case GemType.Yellow :
                ExpAmount = Define.YELLOW_GEM_EXP;
            break;
        }
    }
}

public class GemController : DropItemController
{
    GemInfo gemInfo;
    

    public override bool Init()
    {  
        itemType = Define.ItemType.Gem;
        if (!base.Init()) return false;

        return true;
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public void SetInfo(GemInfo _gemInfo)
    {
        Init();
        gemInfo = _gemInfo;
        var sr = Manager.ResourceM.Load<Sprite>($"{_gemInfo.SpriteName}");
        GetComponent<SpriteRenderer>().sprite = sr;
        transform.localScale = _gemInfo.GemScale;
    }

    public override void GetItem()
    {
        base.GetItem();
        // TODO : 아이템 획득( 애니메이션 넣을건가?    
    }
}
