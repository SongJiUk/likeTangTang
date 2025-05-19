using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GachaResultsPopup : UI_Popup
{
    [SerializeField]
    private GameObject particle;

    List<Equipment> items = new List<Equipment>();
    enum GameObjects
    {
        OpenContentObject,
        ResultsContentObject,
        ResultsContentScrollObject,
        GatchaBoxAni
    }
    enum Texts
    {
        SkipButtonText
    }

    enum Buttons
    {
        SkipButton,
        ConfirmButton
    }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnim(GetObject(gameObjectsType, (int)GameObjects.ResultsContentObject)); 
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        TextsType = typeof(Texts);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindText(TextsType);
        BindButton(ButtonsType);

        GetObject(gameObjectsType, (int)GameObjects.OpenContentObject).gameObject.SetActive(true);
        GetObject(gameObjectsType, (int)GameObjects.ResultsContentObject).gameObject.SetActive(false);

        GetButton(ButtonsType, (int)Buttons.SkipButton).gameObject.BindEvent(OnClickSkipButton);
        GetButton(ButtonsType, (int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        AnimationEventDetector ad = GetObject(gameObjectsType, (int)GameObjects.GatchaBoxAni).GetComponent<AnimationEventDetector>();
        ad.OnEvent -= PlayParticle;
        ad.OnEvent += PlayParticle;
        Refresh();

        var main = particle.GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;

        return true;
    }

    public void SetInfo(List<Equipment> _item)
    {
        items = _item;
        Refresh();
    }

    void Refresh()
    {
        //TODO : 여기서 초기화 해줘야됌.( 아이템을!!!!)
        OnClickSkipButton();
    }

    void OnClickSkipButton()
    {
        Manager.SoundM.PlayButtonClick();

        GetObject(gameObjectsType, (int)GameObjects.OpenContentObject).SetActive(false);
        GetObject(gameObjectsType, (int)GameObjects.ResultsContentObject).SetActive(true);



        GameObject cont = GetObject(gameObjectsType, (int)GameObjects.ResultsContentScrollObject);
        foreach(Transform child in cont.transform)
        {
            child.gameObject.SetActive(false);
        }


        int index = 0;
        foreach(Equipment item in items)
        {
            UI_EquipItem equipItem = null;

            if(index < cont.transform.childCount)
            {
                equipItem = cont.transform.GetChild(index).GetComponent<UI_EquipItem>();
                equipItem.gameObject.SetActive(true);
            }
            else
            {
                string key = typeof(UI_EquipItem).Name;
                equipItem = Manager.ResourceM.Instantiate(key).GetOrAddComponent<UI_EquipItem>();
                equipItem.transform.SetParent(cont.transform, false);
            }
            
            equipItem.SetInfo(item, Define.UI_ItemParentType.GachaResultPopup);
            index++;
        }
    }

    void OnClickConfirmButton()
    {
        Manager.SoundM.PlayPopupClose();
        Manager.ResourceM.Destory(gameObject);
        Manager.GameM.SaveGame();
    }


    public void PlayParticle()
    {
        particle.SetActive(true);
        StartCoroutine(CoSkip());
    }
    IEnumerator CoSkip()
    {
        yield return new WaitForSeconds(2.5f);
        OnClickSkipButton();
    }
}
