using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;


public class ResourceManager
{
    /* 
     * 로드(Instantiate로 필요할때마다 만들고 삭제하는 방식
     * 어차피 처음엔 만들어주긴해야됨(나중가면 pooling할거)
     * 
     * 
     * 
     * 비동기 로드
     * //TODO
     * 키값을 받아서 Dic에 넣어서 보관해줄거임 <stirng , Object>
     * 
     * 키값을 넣어서 부르는 코드를 만들건데 제네릭 코드로 생성(비동기)
     * Dic을 체크해주고 없으면 Addressables의 함수 사용
     * Callback 함수로 처리(키값으로 람다식을 받아서 처리할거임)
     * 
     * 코드 흐름 난잡해지지만 효율적이긴할거
     * 
     * 로드하는 함수 한번에 하기 vs 하나씩하기
     */

    Dictionary<string, Object> resourceDic = new Dictionary<string, Object>();
    public Dictionary<string, Object> ResourceDic { get; }

   
    public T Load<T>(string _key) where T : Object
    { 
        if(resourceDic.TryGetValue(_key, out Object resource))
        {
            return resource as T;
        }
        return null;
    }

    public GameObject Instantiate(string _key, Transform _parent = null, bool _pooing = false)
    {

        //if(resourceDic.TryGetValue(_key, out Object resource))
        //{
        //    var obj = GameObject.Instantiate(resource);
        //    return obj as GameObject;
        //}
        //return null;

        GameObject prefab = Load<GameObject>(_key);

        if (prefab == null)
        {
            Debug.LogError("키에 맞는 프리팹이 없음!!");
            return null;
        }

        if (_pooing)
            return Manager.PoolM.Pop(prefab);

        GameObject go = GameObject.Instantiate(prefab, _parent);
        go.name = prefab.name;

        return go;
    }

    public void Destory(GameObject _go)
    {
        if (_go == null) return;

        if (Manager.PoolM.Push(_go))
            return;

        Object.Destroy(_go);
    }

    #region 비동기 코드 로딩(Addressable)
    public void LoadAsync<T>(string _key, Action<T> cb = null) where T : Object
    {

        if (resourceDic.TryGetValue(_key, out Object resource))
        {
            cb?.Invoke(resource as T);
            return;
        }

        /*
         * 이걸 해주는 이유는 AddressAble에서 이미지를 불러올때
         * 위에는 texture2D이고 밑에있는게 Sprite임
         */
        string loadKey = _key;
        if(_key.Contains(".sprite"))
        {
            loadKey = $"{_key}[{_key.Replace(".sprite", "")}]";
        }

        var asyncOperationHandle = Addressables.LoadAssetAsync<T>(loadKey);
        asyncOperationHandle.Completed += (oper) =>
        {
            resourceDic.Add(_key, oper.Result);
            cb?.Invoke(oper.Result);
        };
    }

    public void LoadAllAsync<T>(string _label, Action<string, int, int> cb = null) where T : Object
    {
        var asyncOperationHandle = Addressables.LoadResourceLocationsAsync(_label, typeof(T));
        asyncOperationHandle.Completed += (oper) =>
        {
            int loadcount = 0;
            int maxCount = oper.Result.Count;

            foreach (var result in oper.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (oper) =>
                {
                    loadcount++;
                    cb?.Invoke(result.PrimaryKey, loadcount, maxCount);
                });
            }
        };
    }
    #endregion
}
