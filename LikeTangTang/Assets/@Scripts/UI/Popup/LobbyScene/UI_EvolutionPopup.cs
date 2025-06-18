using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

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
        Manager.SoundM.PlayButtonClick();
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
}
