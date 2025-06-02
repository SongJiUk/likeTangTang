using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UserInfoItem : UI_Base
{
    enum Buttons
    {
        StaminaButton,
        DiaButton,
        GoldButton
    }

    enum Texts
    {
        UserLevelText,
        StaminaValueText,
        DiaValueText,
        GoldValueText
    }

    enum Images
    {
        UserIconImage,
    }

    enum Sliders
    {
        //UserExpSliderObject
    }


    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        Manager.GameM.OnResourcesChanged -= Refresh;
    }
    public override bool Init()
    {
        if (!base.Init()) return false;
        ButtonsType = typeof(Buttons);
        TextsType = typeof(Texts);
        ImagesType = typeof(Images);
        //SlidersType = typeof(Sliders);

        BindButton(ButtonsType);
        BindText(TextsType);
        BindImage(ImagesType);
        //BindSlider(SlidersType);

        GetButton(ButtonsType, (int)Buttons.StaminaButton).gameObject.BindEvent(OnClickStaminaButton);
        GetButton(ButtonsType, (int)Buttons.DiaButton).gameObject.BindEvent(OnClickDiaButton);
        GetButton(ButtonsType, (int)Buttons.GoldButton).gameObject.BindEvent(OnClickGoldButton);

        Manager.GameM.OnResourcesChanged += Refresh;
        Refresh();

        return true;
    }

    public void Refresh()
    {
        transform.localScale = Vector3.one;
        //TODO : 이거 위에 바꾸는거임 + Level, sliders추가 해줘야함.
        GetImage(ImagesType, (int)Images.UserIconImage).sprite = Manager.ResourceM.Load<Sprite>($"{Manager.DataM.CreatureDic[Manager.GameM.CurrentCharacter.DataId].Image_Name}");
        //if(Manager.DataM.CharacterLevelDataDic.TryGetValue(Manager.GameM.CurrentCharacter.Level, out var Coupon))
        //{
        //    GetSlider(SlidersType, (int)Sliders.UserExpSliderObject).value = Manager.GameM.CurrentCharacter.UseCoupon / Coupon.NeedCouponCount;
        //}
        
        GetText(TextsType, (int)Texts.UserLevelText).text = $"Lv. {Manager.GameM.CurrentCharacter.Level}";
        GetText(TextsType, (int)Texts.StaminaValueText).text = $"{Manager.GameM.Stamina} / {Define.MAX_STAMINA}";
        GetText(TextsType, (int)Texts.DiaValueText).text = $"{Manager.GameM.Dia}";
        GetText(TextsType, (int)Texts.GoldValueText).text = $"{Manager.GameM.Gold}";
    }

    void OnClickStaminaButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_StaminaChargePopup>();
    }

    void OnClickDiaButton()
    {
        Manager.SoundM.PlayButtonClick();
        Manager.UiM.ShowPopup<UI_DiaChargePopup>();
    }

    void OnClickGoldButton()
    {

    }
}
