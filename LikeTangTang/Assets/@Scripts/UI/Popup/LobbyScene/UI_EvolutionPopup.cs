using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EvolutionPopup : UI_Popup
{
    enum GameObjects
    {
        Level_1_BGIN_Object,
        Level_2_BGIN_Object,
        Level_3_BGIN_Object,
        Level_4_BGIN_Object,
        Level_5_BGIN_Object,
        Level_6_BGIN_Object,
        Level_7_BGIN_Object,
        Level_8_BGIN_Object,
        Level_9_BGIN_Object,
        Level_10_BGIN_Object,
        Level_11_BGIN_Object,
        Level_12_BGIN_Object,
        Level_13_BGIN_Object,
        Level_14_BGIN_Object,
        Level_15_BGIN_Object,
        Level_16_BGIN_Object,
        Level_17_BGIN_Object,
        Level_18_BGIN_Object,
        Level_19_BGIN_Object,
        Level_20_BGIN_Object,
        Level_21_BGIN_Object,
        Level_22_BGIN_Object,
        Level_23_BGIN_Object,
        Level_24_BGIN_Object,
        Level_25_BGIN_Object,
        Level_26_BGIN_Object,
        Level_27_BGIN_Object,
        Level_28_BGIN_Object,
        Level_29_BGIN_Object,
        Level_30_BGIN_Object,
        Level_1_LevelCheck,
        Level_2_LevelCheck,
        Level_3_LevelCheck,
        Level_4_LevelCheck,
        Level_5_LevelCheck,
        Level_6_LevelCheck,
        Level_7_LevelCheck,
        Level_8_LevelCheck,
        Level_9_LevelCheck,
        Level_10_LevelCheck,
        Level_11_LevelCheck,
        Level_12_LevelCheck,
        Level_13_LevelCheck,
        Level_14_LevelCheck,
        Level_15_LevelCheck,
        Level_16_LevelCheck,
        Level_17_LevelCheck,
        Level_18_LevelCheck,
        Level_19_LevelCheck,
        Level_20_LevelCheck,
        Level_21_LevelCheck,
        Level_22_LevelCheck,
        Level_23_LevelCheck,
        Level_24_LevelCheck,
        Level_25_LevelCheck,
        Level_26_LevelCheck,
        Level_27_LevelCheck,
        Level_28_LevelCheck,
        Level_29_LevelCheck,
        Level_30_LevelCheck,


    }

    enum Images
    {
        Level_3_Button,
        Level_6_Button,
        Level_9_Button,
        Level_12_Button,
        Level_15_Button,
        Level_18_Button,
        Level_21_Button,
        Level_24_Button,
        Level_27_Button,
        Level_30_Button,
        Level_3_Img,
        Level_6_Img,
        Level_9_Img,
        Level_12_Img,
        Level_15_Img,
        Level_18_Img,
        Level_21_Img,
        Level_24_Img,
        Level_27_Img,
        Level_30_Img,
    }
    

    enum Buttons
    {
        Level_3_Button,
        Level_6_Button,
        Level_9_Button,
        Level_12_Button,
        Level_15_Button,
        Level_18_Button,
        Level_21_Button,
        Level_24_Button,
        Level_27_Button,
        Level_30_Button,
    }


    public bool isOpen = false;

    
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (!base.Init()) return false;
        gameObjectsType = typeof(GameObjects);
        ImagesType = typeof(Images);
        ButtonsType = typeof(Buttons);

        BindObject(gameObjectsType);
        BindImage(ImagesType);
        BindButton(ButtonsType);

        GetButton(ButtonsType, (int)Buttons.Level_3_Button).gameObject.BindEvent(OnClickLevel3Button);
        GetButton(ButtonsType, (int)Buttons.Level_6_Button).gameObject.BindEvent(OnClickLevel6Button);
        GetButton(ButtonsType, (int)Buttons.Level_9_Button).gameObject.BindEvent(OnClickLevel9Button);
        GetButton(ButtonsType, (int)Buttons.Level_12_Button).gameObject.BindEvent(OnClickLevel12Button);
        GetButton(ButtonsType, (int)Buttons.Level_15_Button).gameObject.BindEvent(OnClickLevel15Button);
        GetButton(ButtonsType, (int)Buttons.Level_18_Button).gameObject.BindEvent(OnClickLevel18Button);
        GetButton(ButtonsType, (int)Buttons.Level_21_Button).gameObject.BindEvent(OnClickLevel21Button);
        GetButton(ButtonsType, (int)Buttons.Level_24_Button).gameObject.BindEvent(OnClickLevel24Button);
        GetButton(ButtonsType, (int)Buttons.Level_27_Button).gameObject.BindEvent(OnClickLevel27Button);
        GetButton(ButtonsType, (int)Buttons.Level_30_Button).gameObject.BindEvent(OnClickLevel30Button);

        for (int i = 0; i < 30; i++)
        {
            GetObject(gameObjectsType, (int)GameObjects.Level_1_BGIN_Object + i).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.Level_1_LevelCheck + i).SetActive(false);
            if (i + 1 % 3 == 0)
            {
                GetImage(ImagesType, (int)Images.Level_3_Button + i).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.Off];
                GetImage(ImagesType, (int)Images.Level_3_Img + i).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.Off];
                GetButton(ButtonsType, (int)Buttons.Level_3_Button + i).enabled = false;
            }
        }

        Refresh();

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }
    
    void Refresh()
    {
        GetObject(gameObjectsType, (int)GameObjects.Level_1_LevelCheck + Manager.GameM.CurrentCharacter.Level -1).SetActive(true);

        for (int i = 0; i < Manager.GameM.CurrentCharacter.Level; i++)
        {
            GetObject(gameObjectsType, (int)GameObjects.Level_1_BGIN_Object + i).SetActive(true);
            //Manager.GameM.CurrentCharacter.isEvolutionDic
            if (i + 1 % 3 == 0)
            {
                //GetImage(ImagesType, (int)Images.Level_3_Button + i).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                //GetImage(ImagesType, (int)Images.Level_3_Img + i).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetButton(ButtonsType, (int)Buttons.Level_3_Button + i).enabled = true;
            }
        }
    }

    void OnClickLevel3Button()
    {
        //TODO : 진화 하시겠습니까? + 필요한 재화 
        GetImage(ImagesType, (int)Images.Level_3_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_3_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];

    }
    void OnClickLevel6Button()
    {
        GetImage(ImagesType, (int)Images.Level_6_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_6_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel9Button()
    {
        GetImage(ImagesType, (int)Images.Level_9_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_9_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel12Button()
    {
        GetImage(ImagesType, (int)Images.Level_12_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_12_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel15Button()
    {
        GetImage(ImagesType, (int)Images.Level_15_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_15_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel18Button()
    {
        GetImage(ImagesType, (int)Images.Level_18_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_18_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel21Button()
    {
        GetImage(ImagesType, (int)Images.Level_21_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_21_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel24Button()
    {
        GetImage(ImagesType, (int)Images.Level_24_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_24_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel27Button()
    {
        GetImage(ImagesType, (int)Images.Level_27_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_27_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
    void OnClickLevel30Button()
    {
        GetImage(ImagesType, (int)Images.Level_30_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        GetImage(ImagesType, (int)Images.Level_30_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
    }
}
