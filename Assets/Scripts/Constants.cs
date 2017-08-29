using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public enum AppState
    {
        TitleScreen,
        MainMenu,
        Settings,
        Character,
        PreRound,
        PlayerTurn,
        AiTurn,
        PostRound,
        Exiting
    };

    public enum TacticalStance
    {
        Defensive,
        Balanced,
        Aggressive
    };

    public enum Weapon
    {
        Hands,
        Knife,
        Club,
        Spear
    };

    public const int EAST           = 0;
    public const int SOUTHEAST      = 1;
    public const int SOUTHWEST      = 2;
    public const int WEST           = 3;
    public const int NORTHWEST      = 4;
    public const int NORTHEAST      = 5;
    public const int MAX_DIRECTIONS = 6;

    public static Quaternion[] FACING =
    {
        Quaternion.Euler( 0, 0,   0 ),
        Quaternion.Euler( 0, 0, 300 ),
        Quaternion.Euler( 0, 0, 240 ),
        Quaternion.Euler( 0, 0, 180 ),
        Quaternion.Euler( 0, 0, 120 ),
        Quaternion.Euler( 0, 0,  60 )
    };
}
