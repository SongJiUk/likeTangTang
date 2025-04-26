using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DropItemController : BaseController
{
    public Define.ItemType itemType = Define.ItemType.None;
    public float GetEnvDist {get; set;} = 1f;


    public override bool Init()
    {
        base.Init();
        return true;
    }

    public void OnEnable()
    {
        
    }
    public virtual void OnDisable()
    {
        
    }
    public virtual void GetItem()
    {

    }
        void GetGem()
        {
            List<DropItemController> dropItems = Manager.ObjectM.gemSet.ToList();
            var FindGem = Manager.ObjectM.Grid.GetObjects(transform.position, GetEnvDist);
        
            var sqrtDist = GetEnvDist * GetEnvDist;
            foreach (var gem in gems)
            {
                Vector3 dir = gem.transform.position - transform.position;

                if(dir.sqrMagnitude <= sqrtDist)
                {
                    Manager.ObjectM.DeSpawn(gem);
                }
            }

        
            // Debug.Log($"{FindGem.Count}  /  {gems.Count}");

        }
}
