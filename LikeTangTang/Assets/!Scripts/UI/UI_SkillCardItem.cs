using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillCardItem : UI_Base
{

    /*
    [ ] : 어던 스킬?, 몇 레벨?, 데이트 시트, Set, ClickItem
    */


    //NOTE : 버튼을 찾아서 동적으로 클릭함수를 넣어주는 코드(인자가 있으면 람다로 해줘야함)
    void StartFidnBtn()
    {
        Button btn = transform.GetChild(0).GetComponent<Button>();

        btn.onClick.AddListener(Click);

        btn.onClick.AddListener(() => Click(1));
    }
    public void Click()
    {

    }
    public void Click(int a)
    {

    }
}
