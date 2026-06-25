using System;
using System.Collections.Generic;

public class PlayerStatDictionary
{
    public static Dictionary<int, float> PlayerMoveSpeed = new Dictionary<int, float>()
    {
        { 0, 8f },
        { 1, 10f },
        { 2, 12f },
    };

    public static Dictionary<int, float> PlayerAttackSpeed = new Dictionary<int, float>()
    {
        { 0, 1f },
        { 1, 1.3f },
        { 2, 1.6f },
    };

    public static Dictionary<int, float> PlayerDamageIncrease = new Dictionary<int, float>()
    {
        { 0, 1f },
        { 1, 1.5f },
        { 2, 2.0f },
    };

    public static Dictionary<int, float> PlayerMagnetRadius = new Dictionary<int, float>()
    {
        { 0, 3f },
        { 1, 5f },
        { 2, 7f },
    };
}

public enum PlayerStatEnum
{
    NormalAttack = 0,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Skill5,
    MoveSpeed = 11,
    AttackSpeed,
    MaxHP,
    DamageIncrease,
    MagnetRadius
}
