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
    public enum ItemType
    {
        None,
        Gem,
        Bomb,
        Magnet,
        HP,
        Gold
    }
    public enum SkillType
    {
        None,
        PlasmaShot, //일정 시간마다 전방에 플라즈마 탄환 발사 (짧은 관통 효과 있음)
        EnergyRing, // 플레이어 주변에 에너지 고리 생성, 닿으면 몬스터 데미지
        AutoTurretDrone, // 	일정 시간동안 드론을 소환해서 자동으로 주변 공격
        SelfDestructDrone, //일정 시간 후 폭발하는 자폭 드론을 발사
        ElectricShock, //	랜덤 주변 몬스터들에게 전기 번개 튕기면서 연쇄 데미지
        GravityBomb, // 	중력 자기장 폭탄(몬스터들을 끌어들이는 폭탄)
        TimeSlowField, // 주변 몬스터 이동속도 50% 감소하는 필드 생성
        OrbitalSatellites // 캐릭터 주변을 회전한다.
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
        Player,
        Monster,
        Projectile,
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

    public static string GRIDNAME = "@Grid";
    public static string GEMNAME = "Gem";
}
