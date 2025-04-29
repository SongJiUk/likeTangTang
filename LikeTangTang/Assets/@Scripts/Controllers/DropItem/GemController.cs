using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils;
using DG.Tweening;
public class GemInfo
{   

    public enum GemType
    {
        Red = 1,
        Green = 2,
        Blue = 5,
        Gold = 10
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
            case GemType.Red :
                ExpAmount = Define.SMALL_GEM_EXP;
            break;
            case GemType.Green :
                ExpAmount = Define.GREEN_GEM_EXP;
            break;
            case GemType.Blue :
                ExpAmount = Define.BLUE_GEM_EXP;
            break;
            case GemType.Gold :
                ExpAmount = Define.YELLOW_GEM_EXP;
            break;
        }
    }
}

public class GemController : DropItemController
{
    GemInfo gemInfo;
    Coroutine coMoveToPlayer;

    public override bool Init()
    {  
        itemType = Define.ItemType.Gem;
        if (!base.Init()) return false;

        return true;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if(coMoveToPlayer != null)
        {
            StopCoroutine(coMoveToPlayer);
            coMoveToPlayer = null;
        }
    }

    public void SetInfo(GemInfo _gemInfo)
    {
        Init();
        gemInfo = _gemInfo;
        var sr = Manager.ResourceM.Load<Sprite>($"{_gemInfo.SpriteName}");
        GetComponent<SpriteRenderer>().sprite = sr;
        if (anim != null) anim.runtimeAnimatorController = null;

        transform.localScale = _gemInfo.GemScale;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (coMoveToPlayer == null & this.IsValid())
        {
            Vector3 dir = (transform.position - Manager.GameM.player.transform.position).normalized;
            Vector3 target = transform.position + dir;
            DOTween.Sequence().Append(transform.DOMove(target, 0.3f)
                .SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    coMoveToPlayer = StartCoroutine(CoMoveToPlayer());
                });
         }
    }

    IEnumerator CoMoveToPlayer()
    {
        while(this.IsValid())
        {
            float dist = Vector3.Distance(transform.position, Manager.GameM.player.transform.position);

            transform.position = Vector3.MoveTowards(transform.position, Manager.GameM.player.transform.position, Time.deltaTime);

            if(dist < 0.4f)
            {
                //TODO : 사운드
                Manager.GameM.player.Exp += gemInfo.ExpAmount * Manager.GameM.player.ExpBounsRate;
                Manager.ObjectM.DeSpawn(this);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
