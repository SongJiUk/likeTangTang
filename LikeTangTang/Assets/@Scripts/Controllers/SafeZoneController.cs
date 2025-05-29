using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class SafeZoneController : BaseController
{
    private Coroutine coDotDamage;
    public override bool Init()
    {
        base.Init();
        return true;
    }


    private void OnTriggerEnter2D(Collider2D collision) 
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (!pc.IsValid()) return;

        pc.OnSafeZoneEnter();
        if (coDotDamage != null)
        {
            StopCoroutine(coDotDamage);
            coDotDamage = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        if (!pc.IsValid()) return;

        
        pc.OnSafeZoneExit(this);
        if (coDotDamage == null)
        {
            coDotDamage = StartCoroutine(CoStartDotDamage(pc));
        }
    }

    IEnumerator CoStartDotDamage(PlayerController _pc)
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            _pc.OnSafeZoneExit(this);
        }
    }
}
