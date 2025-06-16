using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class DropItemController : BaseController
{
    public Define.ItemType itemType = Define.ItemType.None;
    public Data.DropItemData dropItem;
    public Animator anim;
    public float CollectDist { get; set; } = 2f;
    public SpriteRenderer ItemSprite;
    public Coroutine coGetItem;
    public override bool Init()
    {
        base.Init();
        if(ItemSprite == null) ItemSprite = GetComponent<SpriteRenderer>();
        if(anim == null) anim = GetComponent<Animator>();

        return true;
    }

    public void OnEnable()
    {
        
    }
    
    public virtual void OnDisable()
    {
        if(coGetItem != null)
        {
            StopCoroutine(coGetItem);
            coGetItem = null;
        }
    }

    public void Clear()
    {
        DOTween.Kill(this);
    }
    public virtual void GetItem()
    {
        Manager.GameM.CurrentMap.Grid.RemoveCell(this);
    }

    public virtual void SetInfo(Data.DropItemData _dropItem)
    {
        
    }

    public virtual void CompleteGetItem()
    {
        if(coGetItem != null)
        {
            StopCoroutine(coGetItem);
            coGetItem = null;
        }

        DOTween.Kill(gameObject);
        
        transform.localScale = Vector3.one;
    }
    public IEnumerator CoCheckDist()
    {
        while(this.IsValid())
        {
            float dist = Vector3.Distance(transform.position, Manager.GameM.player.transform.position);

            transform.position = Vector3.MoveTowards(transform.position, Manager.GameM.player.transform.position, Time.deltaTime * 30.0f);

            if(dist < 1f)
            {
                CompleteGetItem();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }   
    }
}
