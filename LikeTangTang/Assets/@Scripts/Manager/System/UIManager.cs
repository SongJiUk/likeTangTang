using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

//TODO : 마무리 후 스프라이트 이쁜걸로 찾아서 수정하기.


//NOTE : 씬에 놓고 껏다 키는것이 아닌 코드로 관리(어떻게 해도 상관없긴한데, 껏다 키면 UI가 많아질수록 관리하기 힘들다)
/*[ ] 코드에서 자동화해서 사용할 수 있음(방법 알아보기)) - 이름 없이 완전 자동화를 하는 법은 인터페이스를 사용하거나 , ScriptableObject로 사용

UI는 Set, Refresh두 함수를 이용하는게 관리하기 용이함.
*/
public class UIManager
{
    UI_Base ui_Base;
    Stack<UI_Base> uiStack = new Stack<UI_Base>();

    //NOTE : 해당 씬의 스크립트를 호출해서 사용할 수 있게 해줌. ex) Manager.UiM.GetSceneUI<UI_GameScene>().RefreshUI();
    public T GetSceneUI<T>() where T : UI_Base
    {
        return ui_Base as T;
    }


    public T ShowSceneUI<T>() where T : UI_Base
    {
        if(ui_Base != null) return GetSceneUI<T>();

        string key = typeof(T).Name + ".prefab";
        T ui = Manager.ResourceM.Instantiate(key, _pooling:true).GetOrAddComponent<T>();
        ui_Base = ui;

        return ui;
    }

    //NOTE : 설계적인 규칙임(UI는 겹쳐서 사용되는 경우가 많기 때문에, Stack으로 관리하면 편함)
    public T ShowPopup<T>() where T : UI_Base
    {
        string key = typeof(T).Name + ".prefab";
        T ui = Manager.ResourceM.Instantiate(key, _pooling:true).GetOrAddComponent<T>();
        uiStack.Push(ui);
        return ui;
    }

    public void ClosePopup()
    {
        if(uiStack.Count == 0) return;

        UI_Base ui = uiStack.Pop();
        Manager.ResourceM.Destory(ui.gameObject);
        Time.timeScale = 1f;

    }



}
