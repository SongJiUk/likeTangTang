using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        ItemSprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

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

    public virtual void GetItem()
    {
        Manager.GameM.CurrentMap.Grid.RemoveCell(this);
    }

    public virtual void SetInfo(Data.DropItemData _dropItem)
    {
        
    }

    public virtual void CompleteGetItem()
    {

    }
    public IEnumerator CoCheckDist()
    {
        while(this.IsValid())
        {
            float dist = Vector3.Distance(transform.position, Manager.GameM.player.transform.position);

            transform.position = Vector3.MoveTowards(transform.position, Manager.GameM.player.transform.position, Time.deltaTime);

            if(dist < 1f)
            {
                CompleteGetItem();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
