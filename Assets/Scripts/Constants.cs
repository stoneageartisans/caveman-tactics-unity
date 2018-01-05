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

    public enum HexOverlay
    {
        Blank,
        Highlighted,
        Obstructed
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

    public const int LEFT_MOUSE = 0;
    public const int RIGHT_MOUSE = 1;

    public const int STARTING_ROUND = 1;

    public static readonly Quaternion[] FACING =
    {
        Quaternion.Euler( 0, 0,   0 ),
        Quaternion.Euler( 0, 0, 300 ),
        Quaternion.Euler( 0, 0, 240 ),
        Quaternion.Euler( 0, 0, 180 ),
        Quaternion.Euler( 0, 0, 120 ),
        Quaternion.Euler( 0, 0,  60 )
    };

    public const float HEX_SPACING = 1.45F; // True spacing is 1.425, but it was rounded to avoid misses
    public const float SPLASH_DELAY = 2.5F;

    public static readonly string[] CENTER_HEXES =
    {
        "5,4",
        "5,3",
        "4,2",
        "4,3"
    };

    public static readonly string[] CORNER_HEXES =
    {
        "9,6",
        "6,0",
        "0,0",
        "3,6"
    };
}
