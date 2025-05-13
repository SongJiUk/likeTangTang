using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
  
    #region 보석 경험치
    public const int SMALL_GEM_EXP = 1;
    public const int GREEN_GEM_EXP = 2;
    public const int BLUE_GEM_EXP = 5;
    public const int YELLOW_GEM_EXP = 10;
    #endregion
    public const int DEFAULT_PLAYER_ID = 1;

    public const float KNOCKBACK_POWER = 10f;
    public const float KNOCKBACK_COOLTIME = 0.5f;
    public const float KNOCKBACK_TIME = 0.1f;
    public const int MAX_SKILL_COUNT = 6;
    public const int MAX_SKILL_LEVEL = 5;
    public static readonly Dictionary<int, float> POTION_AMOUNT = new Dictionary<int, float>
    {
        { 30000, 0.3f},
        { 30001, 0.6f},
        { 30002, 1f}
    };

    public static int STAGE_GOLD_UP = 1000;
    public static int ID_GOLD = 60000;
    public static int ID_DIA = 60001;
    public static int ID_STAMINA = 60002;
    public static int ID_WeaponScroll = 61000;
    public static int ID_GloveScroll = 61001;
    public static int ID_RingScroll = 61002;
    public static int ID_HelmetScroll = 61003;
    public static int ID_ArmorScroll = 61004;
    public static int ID_BootsScroll = 61005;
    public static int ID_BORONZE_KEY = 62000;
    public static int ID_SILVER_KEY = 62001;
    public static int ID_GOLD_KEY = 62002;
    public static int ID_CLOVER = 62003;

    public static readonly float[] COMMON_GACHA_GRADE_PROB = new float[]
    {
        0.6f, //Common
        0.3f, //Uncommon
        0.09f, // rare
        0.01f // epic
    };

    public static readonly float[] ADVENCED_GACHA_GRADE_PROB = new float[]
    {
        0.35f,
        0.4f,
        0.15f,
        0.1f
    };


    public enum ItemType
    {
        None,
        Gem,
        Bomb,
        Magnet,
        HP,
        Gold,
        Potion,
        DropBox
    }
    public enum SkillType
    {
        None,
        PlasmaShot = 20000, //일정 시간마다 전방에 플라즈마 탄환 발사 (짧은 관통 효과 있음)
        EnergyRing = 20100, // 플레이어 주변에 에너지 고리 생성, 닿으면 몬스터 데미지
        PlasmaSpinner = 20200, //주변의 적에게 튕기는 부메랑을 발사합니다.
        SuicideDrone = 20300, //일정 시간 후 폭발하는 자폭 드론을 발사
        ElectricShock = 20400, //	랜덤 주변 몬스터들에게 전기 번개 튕기면서 연쇄 데미지
        GravityBomb = 20500, // 	중력 자기장 폭탄(몬스터들을 끌어들이는 폭탄)
        TimeStopBomb = 20600, // 주변 몬스터 이동속도 50% 감소하는 필드 생성
        OrbitalBlades = 20700,// 캐릭터 주변을 회전한다.
        ElectronicField = 20800,
        SpectralSlash = 20900
    }

    public enum DropItemType
    {
        None,
        Potion,
        Magnet,
        Bomb,
        DropBox
    }

    public enum EvoloutionType
    {
        None,
        PlasmaCannon = 23000, //플라즈마샷 진화
        Thundering = 23100,
        OmegaBoomerang = 23200,
        ClusterDrone = 23300,
        SuddenStorm = 23400,
        GravitationalCollapse = 23500,
        timeFixedField = 23600,
        OrbitBarrage = 23700,
        HighVoltageField = 23800,
        SpectralPhantom = 23900
    }

    public enum SceneType
    {
        UnKnownScene,
        DevScene,
        TitleScene,
        LobbyScene,
        GameScene

    }

    public enum Sound
    {
        Bgm,
        Effect
    }


    public enum ObjectType
    {
        None,
        Player,
        Monster,
        EliteMonster,
        Boss,
        Projectile,
        Gem,
        Potion,
        DropBox,
        Magnet,
        Bomb,
        Env
    }


    public enum StageType
    {
        Normal,
        Boss
    }

    public enum CreatureState
    {
        Idle,
        OnDamaged,
        Moving,
        Attack,
        Dead
    }

    public enum UIEvent
    {
        None,
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        Drag,
        BeginDrag,
        EndDrag
    }

    public enum SpecialSkillType
    {
        None,
        ItemSkill,
        LevelUp,
        Passive
    }
    
    public enum SpecialSkillGrade
    {
        None,
        Common,
        UnCommon,
        Rare,
        Epic,
        Unique
    }
    
    public enum GachaGrade
    {
        None,
        Normal,
        Common,
        UnCommon,
        Rare,
        Epic,
        Unique

    }

    public enum GachaType
    {
        None = -1,
        CommonGacha = 0,
        AdvancedGacha
    }

    public enum EquipmentGrade
    {
        None = -1,
        Common = 0,
        UnCommon,
        Rare,
        Epic,
        Epic1,
        Epic2,
        Unique,
        Unique1,
        Unique2,
        Unique3
    }

    public static EquipmentGrade GetEquipmnetGrade(EquipmentGrade _grade)
    { 
        return _grade switch
        {
            EquipmentGrade.UnCommon => EquipmentGrade.UnCommon,
            EquipmentGrade.Rare => EquipmentGrade.Rare,
            EquipmentGrade.Epic => EquipmentGrade.Epic,
            EquipmentGrade.Epic1 => EquipmentGrade.Epic,
            EquipmentGrade.Epic2 => EquipmentGrade.Epic,
            EquipmentGrade.Unique => EquipmentGrade.Unique,
            EquipmentGrade.Unique1 => EquipmentGrade.Unique,
            EquipmentGrade.Unique2 => EquipmentGrade.Unique,
            EquipmentGrade.Unique3 => EquipmentGrade.Unique,
            _ => EquipmentGrade.None
        };
    }


    public static int GetGradeNum(EquipmentGrade _grade)
    {
        return _grade switch
        {
            EquipmentGrade.UnCommon => 0,
            EquipmentGrade.Rare => 1,
            EquipmentGrade.Epic or EquipmentGrade.Epic1 or EquipmentGrade.Epic2 => 2,
            EquipmentGrade.Unique or EquipmentGrade.Unique1 or EquipmentGrade.Unique2 or EquipmentGrade.Unique3 => 3,
            _ => -1
        };
    }

    public static int GetEvolutionofEvolutionNum(EquipmentGrade _grade)
    {
        return _grade switch
        {
            EquipmentGrade.Epic1 or EquipmentGrade.Unique1 => 1,
            EquipmentGrade.Epic2 or EquipmentGrade.Unique2 => 2,
            EquipmentGrade.Unique3 => 3,
            _ => -1
        };
    }


    public enum MaterialType
    {
        None,
        Gold,
        Dia,
        Stamina,
        Exp,
        WeaponScroll,
        GloveScroll,
        RingScroll,
        HelmetScroll,
        ArmorScroll,
        BootsScroll,
        BronzeKey,
        SilverKey,
        GoldKey,
        Clover,
        RandomScroll,
        AllRandomEquipmentBox,
        RandomEquipmentBox,
        CommonEquipmentBox,
        UnCommonEquipmentBox,
        RareEquipmentBox,
        EpicEquipmentBox,
        LegendaryEquipmentBox,
        WeaponEnchantStone,
        GloveEnchantStone,
        RingEnchantStone,
        HelmetEnchantStone,
        ArmorEnchantStone,
        BootsEnchantStone,
    }

    public enum MaterialGrade
    {
        None,
        Common,
        UnCommon,
        Rare,
        Epic,
        Unique,
        Legendary
    }

    public enum EquipmentType
    {
        None,
        Weapon,
        Glove,
        Armor,
        Helmet,
        Ring,
        Boots,
    }

    public enum EquipmentSortType
    {
        Level,
        Grade
    }

    public enum MergeEquipmentType
    {
        None,
        ItemCode,
        Grade
    }

    public enum UI_ItemParentType
    {
        CharacterEquipment,
        EquipInventory,
        GachaResultPopup
    }

    public static string GRIDNAME = "@Grid";
    public static string DROPITEMNAME = "DropItem";
    public static string STANDARDNAME = "Standard";

    public static int WAVE_REWARD_TIME = 30;
    public static int POTION_ID = 30000;
    public static int MAGNET_ID = 30003;
    public static int BOMB_ID = 30004;

    
    public class EquipmentUIStyle
    {
        public Color NameColor;
        public Color BorderColor;
        public Color BgColor;

        public EquipmentUIStyle(Color _nameColor, Color _borderColor, Color _bgColor)
        {
            NameColor = _nameColor; //이름
            BorderColor = _borderColor; //테두리
            BgColor = _bgColor; // Enforce, BG
        }
    }

    public static class EquipmentUIColors
    {
        public static readonly Dictionary<MaterialGrade, EquipmentUIStyle> MaterialGradeStyles;
        public static readonly Dictionary<EquipmentGrade, EquipmentUIStyle> EquipGradeStyles;

        static EquipmentUIColors()
        {
            MaterialGradeStyles = new Dictionary<MaterialGrade, EquipmentUIStyle>()
            {
                {MaterialGrade.Common, new EquipmentUIStyle(Utils.HexToColor("A2A2A2"), Utils.HexToColor("AC9B83"), Color.clear)},
                {MaterialGrade.UnCommon, new EquipmentUIStyle(Utils.HexToColor("57FF0B"), Utils.HexToColor("73EC4E"), Color.clear)},
                {MaterialGrade.Rare, new EquipmentUIStyle(Utils.HexToColor("2471E0"), Utils.HexToColor("0F84FF"), Color.clear)},
                {MaterialGrade.Epic, new EquipmentUIStyle(Utils.HexToColor("9F37F2"), Utils.HexToColor("B740EA"), Utils.HexToColor("D094FF"))},
                {MaterialGrade.Unique, new EquipmentUIStyle(Utils.HexToColor("F67B09"), Utils.HexToColor("F19B02"), Utils.HexToColor("F8BE56"))}
            };

            EquipGradeStyles = new Dictionary<EquipmentGrade, EquipmentUIStyle>()
            {
                {EquipmentGrade.Common, new EquipmentUIStyle(Utils.HexToColor("A2A2A2"), Utils.HexToColor("AC9B83"), Utils.HexToColor("AC9B83"))},
                {EquipmentGrade.UnCommon, new EquipmentUIStyle(Utils.HexToColor("57FF0B"), Utils.HexToColor("73EC4E"), Utils.HexToColor("73EC4E"))},
                {EquipmentGrade.Rare, new EquipmentUIStyle(Utils.HexToColor("2471E0"), Utils.HexToColor("0F84FF"), Utils.HexToColor("0F84FF"))},
                {EquipmentGrade.Epic, new EquipmentUIStyle(Utils.HexToColor("9F37F2"), Utils.HexToColor("B740EA"), Utils.HexToColor("D094FF"))},
                {EquipmentGrade.Epic1, new EquipmentUIStyle(Utils.HexToColor("9F37F2"), Utils.HexToColor("B740EA"), Utils.HexToColor("D094FF"))},
                {EquipmentGrade.Epic2, new EquipmentUIStyle(Utils.HexToColor("9F37F2"), Utils.HexToColor("B740EA"), Utils.HexToColor("D094FF"))},
                {EquipmentGrade.Unique, new EquipmentUIStyle(Utils.HexToColor("F67B09"), Utils.HexToColor("F19B02"), Utils.HexToColor("F8BE56"))},
                {EquipmentGrade.Unique1, new EquipmentUIStyle(Utils.HexToColor("F67B09"), Utils.HexToColor("F19B02"), Utils.HexToColor("F8BE56"))},
                {EquipmentGrade.Unique2, new EquipmentUIStyle(Utils.HexToColor("F67B09"), Utils.HexToColor("F19B02"), Utils.HexToColor("F8BE56"))},
                {EquipmentGrade.Unique3, new EquipmentUIStyle(Utils.HexToColor("F67B09"), Utils.HexToColor("F19B02"), Utils.HexToColor("F8BE56"))}
            };
        }
    }

}


