using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    #region 초기화
    public virtual void Init()
    {

    }
    #endregion


    #region ClickEvents
    public virtual void OnDrag(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        
    }
    #endregion
}
