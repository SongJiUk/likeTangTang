using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


class Pool
{
    GameObject prefab;
    IObjectPool<GameObject> pool;


    public Pool(GameObject _prefab)
    {
        prefab = _prefab;
        pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestory);
    }



    //ToDO : Pop, Push 생성 

    GameObject OnCreate()
    {
        return null;
    }

    void OnGet(GameObject _go)
    {

    }

    void OnRelease(GameObject _go)
    {

    }

    void OnDestory(GameObject _go)
    {

    }

}


public class PoolManager
{
    /* ToDo : 예전과는 다르게 유니티에서 공식적으로 지원해줌
     *  Pop, Create, Push
     */


}
