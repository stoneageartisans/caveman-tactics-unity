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

    public enum Weapon
    {
        Hands,
        Knife,
        Club,
        Spear
    };

    public enum TacticalStance
    {
        Defensive,
        Balanced,
        Aggressive
    };
}
