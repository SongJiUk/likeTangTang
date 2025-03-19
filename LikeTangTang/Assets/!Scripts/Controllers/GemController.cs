using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : BaseController
{


    /* TODO : Grid를 활용하여 Gem 최적화하기.
     
    연산을 사용할때 루트를 씌우면 부하가 심하다.기존의 값을 제곱해서 sqrMagnitude를 사용하면 됨.


     */
    public override bool Init()
    {
        base.Init();
        
        return true;
    }


    private void Start()
    {

    }

}
