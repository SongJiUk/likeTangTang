using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterInfoItem : UI_Base
{

    enum Texts
    {
        MonsterLevelValueText
    }

    enum Buttons
    {
        MonsterInfoButton
    }

    enum Images
    {
        MonsterImage
    }
    Data.CreatureData creatureData;
    int stageLevel;
    Transform parent;
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);
        ImagesType = typeof(Images);

        BindText(TextsType);
        BindButton(ButtonsType);
        BindImage(ImagesType);

        GetButton(ButtonsType, (int)Buttons.MonsterInfoButton).gameObject.BindEvent(OnClickMonsterInfo);

        return true;

    }

    public void SetInfo(int _monsterID, int _stageLevel, Transform _parent)
    {
        transform.localScale = Vector3.one;
        parent = _parent;

        if (Manager.DataM.CreatureDic.TryGetValue(_monsterID, out creatureData))
        {
            creatureData = Manager.DataM.CreatureDic[_monsterID];
            stageLevel = _stageLevel;
        }

        Refresh();
    }

    void Refresh()
    {
        GetImage(ImagesType, (int)Images.MonsterImage).sprite = Manager.ResourceM.Load<Sprite>(creatureData.Image_Name);
        GetText(TextsType, (int)Texts.MonsterLevelValueText).text = $"Lv. {stageLevel}";
    }

    void OnClickMonsterInfo()
    {

        UI_ToolTipItem item = Manager.UiM.MakeSubItem<UI_ToolTipItem>(parent);
        item.transform.localScale = Vector3.one;
        RectTransform targetPos = this.gameObject.GetComponent<RectTransform>();
        RectTransform parentsCanvas = parent.gameObject.GetComponent<RectTransform>();
        item.SetInfo(creatureData, targetPos, parentsCanvas);
        item.transform.SetAsLastSibling();
    }
}
    