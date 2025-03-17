using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public PlayerController Player { get; private set; }


    /*TODO 오브젝트 생성, 스폰등 관리
     *  
     * 스폰(ID를 받아서 리턴해줌) - Generic
     * 디스폰
    */

    //public T Spawn<T>(int _templateID) where T : BaseController
    //{
    //    System.Type type = typeof(T);

    //    if(type == typeof(PlayerController))
    //    {
             //TODO : Data에서 값을 가져와서 생성.
    //    }

    //}
    
    public void DeSpawn(GameObject _go)
    {

    }
}
