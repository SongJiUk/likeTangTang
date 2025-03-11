using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_JoyStick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    [SerializeField]
    Image backGround;
    [SerializeField]
    Image handler;

    float circleRadius;
    Vector2 touchPos;
    Vector2 moveDir;
   
    private void Start()
    {
        circleRadius = backGround.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        TurnOnAndOff();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TurnOnAndOff(true);
        backGround.transform.position = eventData.position;
        handler.transform.position = eventData.position;
        touchPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handler.transform.position = touchPos;
        moveDir = Vector2.zero;

        Manager.GameM.PlayerMoveDir = moveDir;

        TurnOnAndOff();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDir = (eventData.position - touchPos);

        float movedist = Mathf.Min(touchDir.magnitude, circleRadius);
        moveDir = touchDir.normalized;

        Vector2 newPos = touchPos + moveDir * movedist;
        handler.transform.position = newPos;

        Manager.GameM.PlayerMoveDir = moveDir;
    }

    public void TurnOnAndOff(bool isOn = false)
    {
        backGround.gameObject.SetActive(isOn);
        handler.gameObject.SetActive(isOn);
    }
}
