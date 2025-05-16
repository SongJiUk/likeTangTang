using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO : 마무리 후 스프라이트 이쁜걸로 찾아서 수정하기.


//NOTE : 씬에 놓고 껏다 키는것이 아닌 코드로 관리(어떻게 해도 상관없긴한데, 껏다 키면 UI가 많아질수록 관리하기 힘들다)
/*[ ] 코드에서 자동화해서 사용할 수 있음(방법 알아보기)) - 이름 없이 완전 자동화를 하는 법은 인터페이스를 사용하거나 , ScriptableObject로 사용

UI는 Set, Refresh두 함수를 이용하는게 관리하기 용이함.
*/
public class UIManager
{
    UI_Base ui_Base;
    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
 
    UI_Scene sceneUI = null;
    public UI_Scene SceneUI { get { return sceneUI; } }

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public T MakeSubItem<T>(Transform _parent = null, string _name = null, bool _pooling = true) where T : UI_Base
    {
        if(string.IsNullOrEmpty(_name)) _name = typeof(T).Name;

        GameObject go = Manager.ResourceM.Instantiate($"{_name}", _parent, _pooling);
        if (_parent != null)
            go.transform.SetParent(_parent);
        
        return Utils.GetOrAddComponent<T>(go);
    }


    //NOTE : 해당 씬의 스크립트를 호출해서 사용할 수 있게 해줌. ex) Manager.UiM.GetSceneUI<UI_GameScene>().RefreshUI();
    //public T GetSceneUI<T>() where T : UI_Base
    //{
    //    return ui_Base as T;
    //}



    public T ShowSceneUI<T>(string _name = null) where T : UI_Scene
    {
        //if(ui_Base != null) return GetSceneUI<T>();

        if(string.IsNullOrEmpty(_name))
            _name = typeof(T).Name;

        GameObject go = Manager.ResourceM.Instantiate(_name);

        T ui = go.GetOrAddComponent<T>();
        sceneUI = ui;

        go.transform.SetParent(Root.transform);

        return ui;
        
    }

    //NOTE : 설계적인 규칙임(UI는 겹쳐서 사용되는 경우가 많기 때문에, Stack으로 관리하면 편함)
    public T ShowPopup<T>(string _name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(_name)) 
            _name = typeof(T).Name;

        GameObject go = Manager.ResourceM.Instantiate($"{_name}");
        T popup = go.GetOrAddComponent<T>();
        popupStack.Push(popup);
        go.transform.SetParent(Root.transform);

        RefreshTimeScale();
        return popup;
    }

    
    public UI_Toast ShowToast(string _detail)
    {
        string name = typeof(UI_Toast).Name;
        GameObject go = Manager.ResourceM.Instantiate(name, _pooling : true);
        UI_Toast toast = go.GetOrAddComponent<UI_Toast>();
        toast.SetInfo(_detail);
        go.transform.SetParent(Root.transform);

        return toast;
    }

    public void CloseToast(UI_Toast _toast)
    {
        Manager.ResourceM.Destory(_toast.gameObject);
    }


    public void ClosePopup(UI_Popup _popup)
    {
        if(popupStack.Count == 0) return;

        if(popupStack.Peek() != _popup)
        {
            Debug.Log("Failed Close Popup");
            return;
        }

        Manager.SoundM.PlayPopupClose();
        ClosePopup();
    }

    public void ClosePopup()
    {
        if (popupStack.Count == 0) return;

        UI_Popup popup = popupStack.Pop();
        Manager.ResourceM.Destory(popup.gameObject);
        popup = null;

        RefreshTimeScale();

    }
    public void CloseAllPopup()
    {
        while (popupStack.Count > 0) ClosePopup();
    }

    public void RefreshTimeScale()
    {
        if(SceneManager.GetActiveScene().name != Define.SceneType.GameScene.ToString())
        {
            Time.timeScale = 1f;
            return;
        }

        if (popupStack.Count > 0)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void Clear()
    {
        CloseAllPopup();
        Time.timeScale = 1;
        sceneUI = null;
    }
}
