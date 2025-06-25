using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
        GameObject prefab = Load<GameObject>(_key);
        if (prefab == null)
        {
            Debug.LogError("키에 맞는 프리팹이 없음!!, ResourceManager 29Line");
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
    public void LoadAsync<T>(string _key, Action<T> _cb = null) where T : Object
    {
        if (resourceDic.TryGetValue(_key, out var resource))
        {
            _cb?.Invoke(resource as T);
            return;
        }

        var asyncOperationHandle = Addressables.LoadAssetAsync<T>(_key);
        asyncOperationHandle.Completed += (oper) =>
        {
            if (oper.Status == AsyncOperationStatus.Succeeded)
            {
                if (oper.Result != null)
                {
                    if (!resourceDic.ContainsKey(_key))
                    {
                        resourceDic.Add(_key, oper.Result);
                    }
                    _cb?.Invoke(oper.Result);
                }
                else
                {
                    Debug.LogError($"ResourceManager: Loaded asset is null for key '{_key}'.");
                    _cb?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"ResourceManager: Failed to load asset for key '{_key}'.\nException: {oper.OperationException}");
                _cb?.Invoke(null);
            }
        };

        //string loadKey = _key;

        //if (_key.Contains("/"))
        //{
        //    string[] parts = _key.Split('/');
        //    if (parts.Length == 2)
        //    {
        //        string parentTextureName = parts[0];
        //        string spriteName = parts[1];
        //        loadKey = $"{parentTextureName}[{spriteName}]";
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"ResourceManager: Unexpected key format for sub-asset '{_key}'. Expected 'Parent/Child'. Using original key.");
        //    }
        //}
        //else if (_key.EndsWith(".sprite", StringComparison.OrdinalIgnoreCase))
        //{
        //    string baseName = _key.Substring(0, _key.LastIndexOf(".sprite", StringComparison.OrdinalIgnoreCase));
        //    loadKey = $"{baseName}[{baseName}]";
        //}

        //var asyncOperationHandle = Addressables.LoadAssetAsync<T>(loadKey);
        //asyncOperationHandle.Completed += (oper) =>
        //{
        //    if (oper.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        if (oper.Result != null)
        //        {

        //            if (!resourceDic.ContainsKey(_key))
        //            {
        //                resourceDic.Add(_key, oper.Result);
        //            }
        //            _cb?.Invoke(oper.Result);
        //        }
        //        else
        //        {
        //            Debug.LogError($"ResourceManager: Loaded asset is null for key '{_key}' (loadKey: {loadKey}).");
        //            _cb?.Invoke(null); // 콜백 호출, null 전달
        //        }
        //    }
        //    else
        //    {
        //        // 로드 실패 시
        //        Debug.LogError($"ResourceManager: Failed to load asset for key '{_key}' (loadKey: {loadKey}). Exception: {oper.OperationException}");
        //        _cb?.Invoke(null); // 콜백 호출, null 전달
        //    }
        //};
    }
    public void LoadAllAsync<T>(string _label, Action<string, int, int> _cb = null) where T : Object
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
                    _cb?.Invoke(result.PrimaryKey, loadcount, maxCount);
                });
            }
        };
    }
    //public void LoadAllAsync<T>(string _label, Action<string, int, int> _cb = null) where T : Object
    //{
    //    //// 1) 먼저 LoadResourceLocationsAsync로 위치 리스트를 구하고
    //    //Addressables.LoadResourceLocationsAsync(_label, typeof(T))
    //    //    .Completed += locsOp =>
    //    //    {
    //    //        int loadcount = 0;
    //    //        int maxCount = locsOp.Result.Count;
    //    //        var locs = locsOp.Result;
                
    //    //        foreach (var loc in locs)
    //    //        {
    //    //            Addressables.LoadAssetAsync<T>(loc)
    //    //                .Completed += op =>
    //    //                    {
    //    //                        if (op.Status == AsyncOperationStatus.Succeeded)
    //    //                        {
    //    //                            Debug.Log(op.Result.name);
    //    //                            resourceDic[loc.PrimaryKey] = op.Result;
    //    //                            loadcount++;
    //    //                            _cb?.Invoke(loc.PrimaryKey, loadcount, maxCount);
    //    //                        }
    //    //                    };
    //    //        }
    //    //    };
    //}
    #endregion
}
