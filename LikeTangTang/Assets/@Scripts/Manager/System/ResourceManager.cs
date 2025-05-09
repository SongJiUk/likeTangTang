using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;


public class ResourceManager
{
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

    public GameObject Instantiate(string _key, Transform _parent = null, bool _pooling = false)
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
            Debug.LogError("키에 맞는 프리팹이 없음!!, ResourceManager 38Line");
            return null;
        }

        if (_pooling)
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

        // NOTE : 이걸 해주는 이유는 AddressAble에서 이미지를 불러올때 위에는 texture2D이고 밑에있는게 Sprite임

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
