using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Player
{
    public Constants.TacticalStance tacticalStance;

    int statMin;

    public Opponent( int statDefaultValue, int statMinValue, int statMaxValue, int startingPoints, int id )
        : base( statDefaultValue, statMinValue, statMaxValue, startingPoints, id )
    {
        this.id = id;
        statMin = statMinValue;

        allotUnspentPoints();
        determineTacticalStance();
    }

    /* My eventual intent will be to make this tweakable in the editor. Currently the decision-engine
     * is weighted as follows:
     *     Rasise stat is 3 of 6 (50%)
     *     Keep stat is 1 of 6   (17%)
     *     Lower stat is 2 of 6  (33%)
     */
    void allotUnspentPoints()
    {
        const int BRAWN = 0;
        const int AGILITY = 1;
        const int BRAINS = 2;
        const int STAMINA = 3;
        const int TOTAL_STATS = 4;

        const int RAISE = 0;
        const int KEEP = 1;
        const int LOWER = 2;

        int currentStat = BRAWN;

        while( unspentPoints > 0 )
        {
            int choice;

            // Get random number from 1 to 6
            int n = Random.Range( 1, 7 );

            // Raise stat if 1, 2 or 3
            if( n > 0 && n < 4 )
            {
                choice = RAISE;
            }
            // Keep stat same if 4
            else if( n > 3 && n < 5 )
            {
                choice = KEEP;
            }
            // Lower stat if 5 or 6
            else
            {
                choice = LOWER;
            }

            switch( choice )
            {
                case RAISE:
                    switch( currentStat )
                    {
                        case BRAWN:
                            increaseStat( ref brawn );
                            break;
                        case AGILITY:
                            increaseStat( ref agility );
                            break;
                        case BRAINS:
                            increaseStat( ref brains );
                            break;
                        case STAMINA:
                            increaseStat( ref stamina );
                            break;
                    }
                    break;
                case KEEP:
                    // Do nothing
                    break;
                case LOWER:
                    switch( currentStat )
                    {
                        case BRAWN:
                            decreaseStat( ref brawn );
                            break;
                        case AGILITY:
                            decreaseStat( ref agility );
                            break;
                        case BRAINS:
                            decreaseStat( ref brains );
                            break;
                        case STAMINA:
                            decreaseStat( ref stamina );
                            break;
                    }
                    break;
            }

            // Move to the next stat
            currentStat ++;

            // When the last stat is done, go back to the beginning
            if( currentStat == TOTAL_STATS )
            {
                currentStat = BRAWN;
            }
        }

        calculateStats();

        /*
        Debug.Log( "Brawn: " + brawn );
        Debug.Log( "Agility: " + agility );
        Debug.Log( "Brains: " + brains );
        Debug.Log( "Stamina: " + stamina );
        Debug.Log( "Hit Points: " + getHitPoints() );
        Debug.Log( "Action Points: " + getActionPoints() );
        Debug.Log( "Initiative: " + getInitiative() );
        Debug.Log( "Defense: " + getDefense() );
        Debug.Log( "Damage: " + getDamageMin() + " - " + getDamageMax() );
        Debug.Log( "Weapon: " + weapon.ToString() );
        Debug.Log( "Unspent Points: " + unspentPoints );
        Debug.Log( "Tactical Stance: " + tacticalStance.ToString() );
        */
    }

    void decreaseStat( ref int stat )
    {
        if( stat > statMin )
        {
            stat --;
            unspentPoints ++;
        }
    }

    /* This will be used to determine how the AI is placed on the map, and how what
     * actions are more likely to be selected during combat
     */
    void determineTacticalStance()
    {
        // Get random number from 0 to 2
        tacticalStance = (Constants.TacticalStance) Random.Range( 0, 3 );
    }

    void increaseStat( ref int stat )
    {
        if( stat < statMax )
        {
            if( unspentPoints > 0 )
            {
                stat ++;
                unspentPoints --;
            }
        }
    }
}
