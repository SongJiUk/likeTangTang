using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class UI_Base : MonoBehaviour
{
    #region 초기화
    protected bool isInit = false;

    void Start()
    {
        Init();
    }
    public virtual bool Init()
    {
        if(isInit) return false;

        isInit = true;
        return true;
    }
    #endregion

    protected virtual void SetInfo()
    {
        //RefreshUI();
    }

    protected virtual void RefreshUI()
    {

    }

    #region 바인딩
    protected  Dictionary<Type, UnityEngine.Object[]> objs_Dic = new Dictionary<Type, UnityEngine.Object[]>();
    protected void Bind<T>(Type _type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(_type);
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];

        objs_Dic.Add(_type, objs);

        for(int i =0 ; i<names.Length; i++)
        {
            if(typeof(T) == typeof(GameObject))
                objs[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objs[i] = Utils.FindChild<T>(gameObject, names[i], true);

            if(objs[i] == null) Debug.LogError($"Failed bind {names[i]}, UI_BASE 54Line");
        }
    }

    protected void BindObject(Type _type) {Bind<GameObject>(_type);}
    protected void BindImage(Type _type){Bind<Image>(_type);}
    protected void BindText(Type _type){Bind<Text>(_type);}
    protected void BindButton(Type _type){Bind<Button>(_type);}
    protected void BindToggle(Type _type){Bind<Toggle>(_type);}
    #endregion

    #region  Get
    //[ ] 일단 Type추가해서 수정함.(Bind에서 Type을 가져오면 동적바인딩돼서 호출한 해당 스크립트의 이름도 같이 들어와서 여기서 TryGetValue가 안되는 문제때문) 문제발생하면 튜플로 넣어보자
    protected T Get<T>(Type _type, int _index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs = null;
        if(objs_Dic.TryGetValue(_type, out objs) == false) return null;

        return objs[_index] as T;
    }

    // protected T Get<T>(int _index) where T : UnityEngine.Object
    // {
    //     UnityEngine.Object[] objs = null;
    //     if(objs_Dic.TryGetValue(typeof(T), out objs) == false) return null;

    //     return objs[_index] as T;
    // }


    protected GameObject GetObject(Type _type, int _index) {return Get<GameObject>(_type, _index);}
    protected Image GetImage(Type _type, int _index) {return Get<Image>(_type, _index);}
    protected TextMeshProUGUI GetText(Type _type,int _index) {return Get<TextMeshProUGUI>(_type, _index);}
    //protected TextMeshProUGUI GetText(int _index) {return Get<TextMeshProUGUI>(_index);}
    protected Button GetButton(Type _type, int _index) {return Get<Button>(_type, _index);}
    protected Toggle GetToggle(Type _type, int _index) {return Get<Toggle>(_type,_index);}
    #endregion

    #region 이벤트 바인딩
    public static void BindEvent(GameObject _go, Action _action = null, Action<BaseEventData> _dragAction = null, Define.UIEvent _type = Define.UIEvent.Click)
    {
        UI_EventHandler eh = Utils.GetOrAddComponent<UI_EventHandler>(_go);

        switch(_type)
        {
            case Define.UIEvent.Click :
            eh.OnClickHandler -= _action;
            eh.OnClickHandler += _action;
            break;

            case Define.UIEvent.Pressed :
            eh.OnPressHandler -= _action;
            eh.OnPressHandler += _action;
            break;

            case Define.UIEvent.PointerDown :
            eh.OnPointerDownHandler -= _action;
            eh.OnPointerDownHandler += _action;
            break;
            
            case Define.UIEvent.PointerUp :
            eh.OnPointerUpHandler -= _action;
            eh.OnPointerUpHandler += _action;
            break;

            case Define.UIEvent.Drag :
            eh.OnDragHandler -= _dragAction;
            eh.OnDragHandler += _dragAction;

            break;

            case Define.UIEvent.BeginDrag :
            eh.OnBeginDragHandler -= _dragAction;
            eh.OnBeginDragHandler += _dragAction;
            break;

            case Define.UIEvent.EndDrag:
            eh.OnEndDragHandler -= _dragAction;
            eh.OnEndDragHandler += _dragAction;
            break;

        }

    }

    #endregion

}
