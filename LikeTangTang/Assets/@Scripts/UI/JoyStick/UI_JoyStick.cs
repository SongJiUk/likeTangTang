using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_JoyStick : UI_Scene
{

    enum GameObjects
    {
        TouchBG,
        BG,
        Handler
    }

    GameObject touchBG;
    GameObject handler;
    GameObject handlerBG;


    float circleRadius;
    Vector2 touchPos;
    Vector2 moveDir;

    public override bool Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        
        touchBG = GetObject(typeof(GameObjects), (int)GameObjects.TouchBG);
        handlerBG = GetObject(typeof(GameObjects), (int)GameObjects.BG);
        handler = GetObject(typeof(GameObjects), (int)GameObjects.Handler);

        RectTransform bgrect = handlerBG.gameObject.GetComponent<RectTransform>();
        circleRadius = (bgrect.rect.width * handlerBG.transform.lossyScale.x) / 2f;
        
        BindEvent(touchBG, OnPointerDown, _type: Define.UIEvent.PointerDown);
        BindEvent(touchBG, OnPointerUp, _type: Define.UIEvent.PointerUp);
        BindEvent(touchBG, _dragAction: OnDrag, _type: Define.UIEvent.Drag);
        
        TurnOnAndOff();

        return true;
    }

    //[x] 수정하기(UI_BASE사용해서 )
    public void OnPointerDown()
    {
        TurnOnAndOff(true);
        Vector2 mousePos = Input.mousePosition;
        handlerBG.transform.position = mousePos;
        handler.transform.position = mousePos;
        touchPos = mousePos;
    }

    public void OnPointerUp()
    {
        handler.transform.position = touchPos;
        moveDir = Vector2.zero;

        Manager.GameM.PlayerMoveDir = moveDir;

        TurnOnAndOff();
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData pe = eventData as PointerEventData;
        if(pe == null) return;

        Vector2 touchDir = pe.position - touchPos;
        
        float movedist = Mathf.Min(touchDir.magnitude, circleRadius);
        moveDir = touchDir.normalized;

        Vector2 newPos = touchPos + moveDir * movedist;
        handler.transform.position = newPos;

        Manager.GameM.PlayerMoveDir = moveDir;
    }


    public void TurnOnAndOff(bool isOn = false)
    {
        handlerBG.gameObject.SetActive(isOn);
        handler.gameObject.SetActive(isOn);
    }
}
