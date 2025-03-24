using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
  
    public enum Scene
    {
        UnKnown,
        DevScene,
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
        Projectile,
        Melee
    }

    public static string GRIDNAME = "!Grid";
    public static string GEMNAME = "Gem.prefab";
}
