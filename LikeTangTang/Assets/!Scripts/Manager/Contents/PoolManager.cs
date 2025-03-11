using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


class Pool
{
    IObjectPool<GameObject> pool;


    public Pool(GameObject _prefab)
    {
        //pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestory);
    }

}


public class PoolManager
{



}
