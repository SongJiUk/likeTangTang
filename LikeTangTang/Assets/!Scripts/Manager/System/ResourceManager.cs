using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;


public class ResourceManager
{
    /* TODO
     * 로드(Instantiate로 필요할때마다 만들고 삭제하는 방식
     * 어차피 처음엔 만들어주긴해야됨(나중가면 pooling할거)
     * 
     * 
     * 
     * 비동기 로
     * 값을 받아서 Dic에 넣어서 보관해줄거임 <stirng , Object>
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


    #region 비동기 코드 로딩
    public void LoadAsync<T>(string _key, Action<T> cb = null) where T : Object
    {


        //있으면 반환
        if(resourceDic.TryGetValue(_key, out Object resource))
        {
            cb?.Invoke(resource as T);
            return;
        }


        //없으면 추가
        var asyncOperationHandle = Addressables.LoadAssetAsync<T>(_key);

        asyncOperationHandle.Completed += (op) =>
        {
            resourceDic.Add(_key, op.Result);
            cb?.Invoke(op.Result);
        };
    }


    public void LoadAllAsync<T>(string _label, Action<string, int, int> cb = null) where T : Object
    {

        var asyncOperationHandle = Addressables.LoadResourceLocationsAsync(_label, typeof(T));

        asyncOperationHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int maxCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, (op) =>
                {
                    loadCount++;
                    cb?.Invoke(result.PrimaryKey, loadCount, maxCount);
                });
            }
        };
        
    }

    #endregion

}
