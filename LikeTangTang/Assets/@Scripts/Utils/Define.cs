using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
  
    public enum Scene
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

    public enum SkillType
    {
        None,
        Sequence,
        Repeat
    }

    public enum StageType
    {
        Normal,
        Boss
    }

    public enum CreatureState
    {
        Idle,
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
    public static string GEMNAME = "Gem.prefab";
}
