using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int agility;
    public int agilityMin;
    public int brains;
    public int brainsMin;
    public int brawn;
    public int brawnMin;
    public int stamina;
    public int staminaMin;
    public int unspentPoints;
    public int statMax;
    public Constants.Weapon weapon;
    public Renderer token;
    public Transform transform;

    [HideInInspector]
    public int id;

    float initiative;
    int action;
    int actionPoints;
    Dictionary<Constants.Weapon, int> damageMax;
    Dictionary<Constants.Weapon, int> damageMin;
    int defense;
    int hitPoints;
    int wounds;

    public Player( int statDefaultValue, int statMinValue, int statMaxValue, int startingPoints, int id )
    {
        this.id = id;
        agility = statDefaultValue;
        agilityMin = statMinValue;
        brains = statDefaultValue;
        brainsMin = statMinValue;
        brawn = statDefaultValue;
        brawnMin = statMinValue;
        stamina = statDefaultValue;
        staminaMin = statMinValue;
        unspentPoints = startingPoints;
        statMax = statMaxValue;
        weapon = Constants.Weapon.Hands;
        wounds = 0;
        calculateStats();
    }

    public void calculateStats()
    {
        hitPoints = Mathf.RoundToInt( (float) ( brawn + stamina ) / 2.0F );

        if( ( ( float ) agility / 2.0F ) > ( ( float ) ( brawn + agility + brains + stamina ) / 8.0F ) )
        {
            defense = Mathf.RoundToInt( ( float ) agility / 2.0F );
        }
        else
        {
            defense = Mathf.RoundToInt( ( float ) ( brawn + agility + brains + stamina ) / 8.0F );
        }

        initiative = ( float ) ( agility + brains + stamina ) / 6.0F;

        action = Mathf.RoundToInt( ( float ) ( agility + brains + stamina ) / 6.0F );
        actionPoints = action;

        damageMin = new Dictionary<Constants.Weapon, int>();

        if( brawn > 9 )
        {
            damageMin.Add( Constants.Weapon.Hands, Mathf.RoundToInt( ( float ) ( brawn - 7 ) / 3.0F ) );
        }
        else
        {
            damageMin.Add( Constants.Weapon.Hands, 0 );
        }

        damageMin.Add( Constants.Weapon.Knife, damageMin[Constants.Weapon.Hands] + 1 );
        damageMin.Add( Constants.Weapon.Club, damageMin[Constants.Weapon.Hands] + 2 );
        damageMin.Add( Constants.Weapon.Spear, damageMin[Constants.Weapon.Hands] + 1 );

        damageMax = new Dictionary<Constants.Weapon, int>();

        if( brawn > 7 )
        {
            damageMax.Add( Constants.Weapon.Hands, Mathf.RoundToInt( ( float ) ( brawn - 5 ) / 2.0F ) );
        }
        else
        {
            if( brawn < 6 )
            {
                damageMax.Add( Constants.Weapon.Hands, 0 );
            }
            else
            {
                damageMax.Add( Constants.Weapon.Hands, 1 );
            }
        }

        damageMax.Add( Constants.Weapon.Knife, damageMax[Constants.Weapon.Hands] + 2 );
        damageMax.Add( Constants.Weapon.Club, damageMax[Constants.Weapon.Hands] + 3 );
        damageMax.Add( Constants.Weapon.Spear, damageMax[Constants.Weapon.Hands] + 4 );
    }

    public void resetActionPoints()
    {
        actionPoints = action;
    }

    public int getAction()
    {
        return action;
    }

    public int getActionPoints()
    {
        return actionPoints;
    }

    public float getInitiative()
    {
        return initiative;
    }

    public int getDamageMax()
    {
        return damageMax[weapon];
    }

    public int getDamageMin()
    {
        return damageMin[weapon];
    }

    public int getDefense()
    {
        return defense;
    }

    public int getHitPoints()
    {
        return hitPoints;
    }

    public int getWounds()
    {
        return wounds;
    }
}
