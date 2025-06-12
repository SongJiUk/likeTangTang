using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_EvolutionPopup : UI_Popup
{
    //TODO : 주석된거 다 지우기 
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

    private readonly int[] evolutionLevels = { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30 };
    private Dictionary<int, Button> levelButtons;
    private Dictionary<int, Image> levelButtonImages;
    private Dictionary<int, Image> levelIcons;

    public bool isOpen;
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


        SetupEvolutionUI();
        //GetButton(ButtonsType, (int)Buttons.Level_3_Button).gameObject.BindEvent(OnClickLevel3Button);
        //GetButton(ButtonsType, (int)Buttons.Level_6_Button).gameObject.BindEvent(OnClickLevel6Button);
        //GetButton(ButtonsType, (int)Buttons.Level_9_Button).gameObject.BindEvent(OnClickLevel9Button);
        //GetButton(ButtonsType, (int)Buttons.Level_12_Button).gameObject.BindEvent(OnClickLevel12Button);
        //GetButton(ButtonsType, (int)Buttons.Level_15_Button).gameObject.BindEvent(OnClickLevel15Button);
        //GetButton(ButtonsType, (int)Buttons.Level_18_Button).gameObject.BindEvent(OnClickLevel18Button);
        //GetButton(ButtonsType, (int)Buttons.Level_21_Button).gameObject.BindEvent(OnClickLevel21Button);
        //GetButton(ButtonsType, (int)Buttons.Level_24_Button).gameObject.BindEvent(OnClickLevel24Button);
        //GetButton(ButtonsType, (int)Buttons.Level_27_Button).gameObject.BindEvent(OnClickLevel27Button);
        //GetButton(ButtonsType, (int)Buttons.Level_30_Button).gameObject.BindEvent(OnClickLevel30Button);

        //int count = 0;
        //for (int i = 0; i < Define.CHARACTER_MAX_LEVEL; i++)
        //{
        //    GetObject(gameObjectsType, (int)GameObjects.Level_1_BGIN_Object + i).SetActive(false);
        //    GetObject(gameObjectsType, (int)GameObjects.Level_1_LevelCheck + i).SetActive(false);
        //    if ((i + 1) % 3 == 0)
        //    {
        //        GetImage(ImagesType, (int)Images.Level_3_Button + count).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.Off];
        //        GetImage(ImagesType, (int)Images.Level_3_Img + count).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.Off];
        //        GetButton(ButtonsType, (int)Buttons.Level_3_Button + count).interactable = false;
        //        count++;
        //    }
        //}

        Refresh();

        return true;
    }

    void SetupEvolutionUI()
    {
        levelButtons = new Dictionary<int, Button>();
        levelButtonImages = new Dictionary<int, Image>();
        levelIcons = new Dictionary<int, Image>();

        for(int i =0; i<evolutionLevels.Length; i++)
        {
            int level = evolutionLevels[i];
            var btn = GetButton(ButtonsType, i);
            var btnImg = GetImage(ImagesType, (int)Images.Level_3_Button + i);
            var iconImg = GetImage(ImagesType, (int)Images.Level_3_Img + i);

            levelButtons[level] = btn;
            levelButtonImages[level] = btnImg;
            levelIcons[level] = iconImg;

            btn.interactable = false;
            btnImg.color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.Off];
            iconImg.color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.Off];

            int eventLevel = level;
            btn.gameObject.BindEvent(() => OnClickEvolutionButton(eventLevel));
        }
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {

        var Character = Manager.GameM.CurrentCharacter;
        int characterLevel = Character.Level;

        for (int i = 0; i < Define.CHARACTER_MAX_LEVEL; i++)
        {
            GetObject(gameObjectsType, (int)GameObjects.Level_1_BGIN_Object + i).SetActive(false);
            GetObject(gameObjectsType, (int)GameObjects.Level_1_LevelCheck + i).SetActive(false);
        }

        int showIndex = GetIndexToShow(characterLevel, Character.isLearnEvloution);

        for (int i = 0; i <= showIndex && i < Define.CHARACTER_MAX_LEVEL; i++)
        {
            GetObject(gameObjectsType, (int)GameObjects.Level_1_BGIN_Object + i).SetActive(true);
        }

        if (showIndex >= 0 && showIndex < Define.CHARACTER_MAX_LEVEL)
        {
            GetObject(gameObjectsType, (int)GameObjects.Level_1_LevelCheck + showIndex).SetActive(true);
        }

        foreach(var level in evolutionLevels)
        {
            var btn = levelButtons[level];
            var btnImg = levelButtonImages[level];
            var iconImg = levelIcons[level];

            bool available = level <= characterLevel;
            btn.interactable = available;

            bool learned = Character.isLearnEvloution.TryGetValue(level, out bool islearned) && islearned;

            var style = Define.EquipmentUIColors.EvolutionStyles[learned ? Define.EvolutionOnOff.On : Define.EvolutionOnOff.Off];
            btnImg.color = style;
            iconImg.color = style;

        }

        //int count = 0;
        //for (int i = 3; i <= Define.CHARACTER_MAX_LEVEL; i += 3)
        //{
        //    if (i > characterLevel) break;
        //    GetButton(ButtonsType, (int)Buttons.Level_3_Button + count).interactable = true;

        //    bool learned = Character.isLearnEvloution.ContainsKey(i) && Character.isLearnEvloution[i];
        //    var evolOn = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
        //    var evolOff = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.Off];

        //    GetImage(ImagesType, (int)Images.Level_3_Button + count).color = GetImage(ImagesType, (int)Images.Level_3_Img + count).color = learned ? evolOn : evolOff;


        //    count++;
        //}
    }

    int GetIndexToShow(int _characterLevel, Dictionary<int, bool> _learnDic)
    {
        for (int i = 3; i <= Define.CHARACTER_MAX_LEVEL; i += 3)
        {
            if (i <= _characterLevel)
            {
                bool learned = _learnDic.ContainsKey(i) && _learnDic[i];
                if (!learned)
                {
                    return i - 2;
                }
            }
            else
            {
                return _characterLevel - 1;
            }
        }
        return _characterLevel - 1;
    }

    void OnClickEvolutionButton(int _level)
    {
        var Character = Manager.GameM.CurrentCharacter;
        const int interval = 3;
        int prevLevel = _level - interval;
        if(prevLevel >= 1)
        {
            if(!Character.isLearnEvloution.TryGetValue(prevLevel, out bool prevLearn) || !prevLearn)
            {
                Manager.UiM.ShowToast("이전 단계 진화 스킬을 먼저 배워야 합니다!");
                return;
            }
        }

        if (!levelButtons[_level].interactable || (Character.isLearnEvloution.TryGetValue(_level, out bool learned) && learned))
            return;

        UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
        popup.SetInfo(Character.evolutionData[_level], _level);
        popup.OnLearnCallBack = () =>
        {
            levelButtonImages[_level].color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
            levelIcons[_level].color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
            Refresh();
        };

        popup.gameObject.SetActive(true);
    }

    void OnClickLevel3Button()
    {
        
        if (GetButton(ButtonsType, (int)Buttons.Level_3_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[3])
        {
            
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[3], 3);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_3_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_3_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
    }

    void OnClickLevel6Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_6_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[6])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[6], 6);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_6_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_6_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
            
    }
    void OnClickLevel9Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_9_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[9])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[9], 9);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_9_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_9_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
    void OnClickLevel12Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_12_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[12])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[12], 12);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_12_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_12_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
    void OnClickLevel15Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_15_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[15])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[15], 15);

            popup.OnLearnCallBack = () =>
            {
                 
                GetImage(ImagesType, (int)Images.Level_15_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_15_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
    void OnClickLevel18Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_18_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[18])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[18], 18);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_18_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_18_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
    void OnClickLevel21Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_21_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[21])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[21], 21);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_21_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_21_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
    void OnClickLevel24Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_24_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[24])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[24], 24);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_24_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_24_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
    void OnClickLevel27Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_27_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[27])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[27], 27);

            popup.OnLearnCallBack = () =>
            {
                
                GetImage(ImagesType, (int)Images.Level_27_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_27_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
    void OnClickLevel30Button()
    {
        if (GetButton(ButtonsType, (int)Buttons.Level_30_Button).interactable && !Manager.GameM.CurrentCharacter.isLearnEvloution[30])
        {
            UI_EvolutioninfoPopup popup = Manager.UiM.ShowPopup<UI_EvolutioninfoPopup>();
            popup.SetInfo(Manager.GameM.CurrentCharacter.evolutionData[30], 30);

            popup.OnLearnCallBack = () =>
            {
                GetImage(ImagesType, (int)Images.Level_30_Button).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                GetImage(ImagesType, (int)Images.Level_30_Img).color = Define.EquipmentUIColors.EvolutionStyles[Define.EvolutionOnOff.On];
                Refresh();
            };

            popup.gameObject.SetActive(true);
        }
        
    }
}
