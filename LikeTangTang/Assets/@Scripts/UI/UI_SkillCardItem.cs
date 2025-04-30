using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCardItem : UI_Base
{

    /*[x] : 어떤 스킬?, 몇 레벨?, 데이트 시트, Set, ClickItem
    */

    public int templateID;
    public Data.SkillData skillData;

    private void Start()
    {
        StartFidnBtn();
    }

    //NOTE : 버튼을 찾아서 동적으로 클릭함수를 넣어주는 코드(인자가 있으면 람다로 해줘야함)
    void StartFidnBtn()
    {
        Button btn = transform.GetChild(0).GetComponent<Button>();

        btn.onClick.AddListener(OnClickItem);
    }
    public void Click()
    {

    }
    public void Click(int a)
    {

    }
    public void SetInfo(SkillBase _skill)
    {   
        //TODO : 스킬 아이템 세팅 
    }

    public void OnClickItem()
    {
        //TODO : 아이템 클릭시.
        Debug.Log("Click");
        Manager.UiM.ClosePopup();
    }

}