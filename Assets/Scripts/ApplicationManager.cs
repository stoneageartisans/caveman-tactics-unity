using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager instance;

    public AudioSource music;
    public Constants.AppState appState = Constants.AppState.TitleScreen;
    public Constants.AppState resumeState = Constants.AppState.MainMenu;
    public bool musicOn = true;
    public bool sfxOn = true;
    public GameObject playerToken;
    public GameObject opponentToken;
    public int opponentStartingPoints = 10;
    public int playerStartingPoints = 10;
    public int statDefaultValue = 10;
    public int statMaxValue = 20;
    public int statMinValue = 5;

    [HideInInspector]
    public ArrayList opponents;

    [HideInInspector]
    public ArrayList tokens;

    [HideInInspector]
    public ArrayList roundOrder;

    [HideInInspector]
    public bool newGameStarted;
    
    [HideInInspector]
    public bool playerHasAdvantage;
    
    [HideInInspector]
    public Dictionary<string, Hexagon> hexagons;

    [HideInInspector]
    public GameObject hexGrid;

    [HideInInspector]
    public int round;

    [HideInInspector]
    public Player player;

    void Awake()
    {
        if( instance == null )
        {
            instance = this;
            DontDestroyOnLoad( instance.gameObject );
        }
        else
        {
            Debug.Log( "ERROR: An instance of " + gameObject.name + " already exists." );
            Destroy( gameObject );
        }
    }

	void Start()
    {
        Invoke( "changeScreen", 2.5F );

        if( music != null )
        {
            music.Play();
        }

        player = new Player( statDefaultValue, statMinValue, statMaxValue, playerStartingPoints, 0 );
        opponents = new ArrayList();
        roundOrder = new ArrayList();
        newGameStarted = false;
        playerHasAdvantage = true;
        round = 1;
	}
	
	void Update()
    {
        // TODO
	}
    
    void clearHex( string hexId )
    {
        if( hexId != null )
        {
            ( hexagons[hexId] ).isObstructed = false;
            ( hexagons[hexId] ).isOccupied = false;
            ( hexagons[hexId] ).setOverlay( Constants.HexOverlay.Blank );
        }
        else
        {
            // log debug message
        }
    }

    void clearHexes( List<string> hexIdList )
    {
        foreach( string hexId in hexIdList )
        {
            if( hexId != null )
            {
                ( hexagons[hexId] ).isObstructed = false;
                ( hexagons[hexId] ).isOccupied = false;
                ( hexagons[hexId] ).setOverlay( Constants.HexOverlay.Blank );
            }
            else
            {
                // log debug message
            }
        }
    }

    void createTokens()
    {
        if( newGameStarted )
        {
            tokens = new ArrayList();

            tokens.Insert( player.id, Instantiate<GameObject>( playerToken ).gameObject );

            foreach( Opponent opponent in opponents )
            {
                tokens.Insert( opponent.id, Instantiate<GameObject>( opponentToken ).gameObject );
            }

            foreach( GameObject token in tokens )
            {
                token.SetActive( false );
            }
        }
    }

    void determineInitiative()
    {
        if( newGameStarted )
        {
            roundOrder.Clear();

            int[] ids = new int[round + 1];
            float[] initiatives = new float[round + 1];

            ids[0] = player.id;
            initiatives[0] = player.getInitiative();

            foreach( Opponent opponent in opponents )
            {
                ids[opponent.id] = opponent.id;
                initiatives[opponent.id] = opponent.getInitiative();
            }

            int tempId = 0;
            float tempInitiative = 0;

            for( int i = round; i > -1; i -- )
            {
                for( int j = round - 1; j > -1; j -- )
                {
                    if( initiatives[j + 1] < initiatives[j] )
                    {
                        tempId = ids[j + 1];
                        ids[j + 1] = ids[j];
                        ids[j] = tempId;

                        tempInitiative = initiatives[j + 1];
                        initiatives[j + 1] = initiatives[j];
                        initiatives[j] = tempInitiative;
                    }
                }
            }

            for( int i = 0; i <= round; i ++ )
            {
                roundOrder.Insert( i, ids[i] );
            }

            roundOrder.Reverse();
        }
    }

    void determineTacticalAdvantage()
    {
        if( newGameStarted )
        {
            playerHasAdvantage = true;

            int playerResult = statCheck( player.brains );

            foreach( Opponent opponent in opponents )
            {
                if( statCheck( opponent.brains ) > playerResult )
                {
                    playerHasAdvantage = false;
                    break;
                }
            }
        }
    }

    List<string> getAvailableHexIds( string hexId, int hexDistance, bool insideRange = true )
    {
        List<string> idList = new List<string>();

        float distance = ( float ) hexDistance * Constants.HEX_SPACING;

        Vector3 hexPosition = ( hexagons[hexId] ).transform.position;

        foreach( Hexagon hexagon in hexagons.Values )
        {
            if( !hexagon.isOccupied && !hexagon.isObstructed )
            {
                if( insideRange )
                { 
                    if( Vector3.Distance( hexPosition, hexagon.transform.position ) < distance )
                    {
                        idList.Add( hexagon.id );
                    }
                }
                else
                {
                    if( Vector3.Distance( hexPosition, hexagon.transform.position ) > distance )
                    {
                        idList.Add( hexagon.id );
                    }
                }
            }
        }

        // Debugging
        /*foreach( string id in idList )
        {
            Debug.Log( "Hex with id == " + id + " is within " + hexDistance + " hexes of Hexagon " + hexId );
        }*/

        return idList;
    }

    void generateOpponents()
    {
        if( newGameStarted )
        {
            opponents.Clear();

            for( int i = 0; i < round; i ++ )
            {
                opponents.Insert( i, new Opponent( statDefaultValue, statMinValue, statMaxValue, opponentStartingPoints, i + 1 ) );
            }
        }
    }

    int getDiceRoll()
    {
        // Roll 2 "11-sided" dice
        int die1 = Random.Range( 0, 11 ); // 0 to 10
        int die2 = Random.Range( 0, 11 ); // 0 to 10

        // Returns a number from 0 to 20, slightly weighted towards 10
        return ( die1 + die2 );
    }

    void placeOpponents()
    {
        if( playerHasAdvantage )
        {
            foreach( Opponent opponent in opponents )
            {
                switch( opponent.tacticalStance )
                {
                    case Constants.TacticalStance.Aggressive:
                        // Place adjacent to player
                        break;
                    case Constants.TacticalStance.Balanced:
                        // Place 2 - 3 hexes from player
                        break;
                    case Constants.TacticalStance.Defensive:
                        // Place 4+ hexes from player
                        break;
                }
            }
        }
        else
        {
            foreach( Opponent opponent in opponents )
            {
                switch( opponent.tacticalStance )
                {
                    case Constants.TacticalStance.Aggressive:
                        // Place in center area of grid
                        break;
                    case Constants.TacticalStance.Balanced:
                        // Place in random hex
                        break;
                    case Constants.TacticalStance.Defensive:
                        // Place in outer hex
                        break;
                }
            }
        }
    }

    void placeTokens()
    {
        if( newGameStarted )
        {
            if( playerHasAdvantage )
            {
                placeOpponents();
            }
            else
            {
                // placing of opponents will wait until begin button clicked
            }

            newGameStarted = false;
        }
    }

    int statCheck( int statValue )
    {
        // Success is defined as a "roll" that is less than or equal to the stat
        return ( statValue - getDiceRoll() );
    }

    public void changeScreen()
    {
        SceneManager.LoadScene( appState.ToString() );
    }

    public void checkAppState()
    {
        switch( appState )
        {
            case Constants.AppState.TitleScreen:
                break;
            case Constants.AppState.MainMenu:
                break;
            case Constants.AppState.Settings:
                break;
            case Constants.AppState.Character:
                break;
            case Constants.AppState.PreRound:
                generateOpponents();
                determineTacticalAdvantage();
                determineInitiative();
                createTokens();
                placeTokens();
                break;
            case Constants.AppState.PlayerTurn:
                break;
            case Constants.AppState.AiTurn:
                break;
            case Constants.AppState.PostRound:
                break;
            case Constants.AppState.Exiting:
                exitGame();
                break;
        }
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void highlightHex( string hexId )
    {
        if( hexId != null )
        {
            ( hexagons[hexId] ).isHighlighted = true;
            ( hexagons[hexId] ).setOverlay( Constants.HexOverlay.Highlighted );
        }
        else
        {
            // log debug message
        }
    }

    public void highlightHexes( List<string> hexIdList )
    {
        foreach( string hexId in hexIdList )
        {
            if( hexId != null )
            {
                ( hexagons[hexId] ).isHighlighted = true;
                ( hexagons[hexId] ).setOverlay( Constants.HexOverlay.Highlighted );
            }
            else
            {
                // log debug message
            }
        }
    }

    public void initializeHexGrid( ref GameObject hexGrid )
    {
        hexagons = new Dictionary<string, Hexagon>();

        Component[] hexArray = hexGrid.GetComponentsInChildren( typeof( Hexagon ) );

        foreach( Hexagon hexagon in hexArray )
        {
            hexagons.Add( hexagon.id, hexagon );
        }
    }

    public void lockHexGrid()
    {
        foreach( Hexagon hexagon in hexagons.Values )
        {
            hexagon.isSelectable = false;
        }
    }
    
    public void obstructHex( string hexId )
    {
        if( hexId != null )
        {
            ( hexagons[hexId] ).isObstructed = true;
            ( hexagons[hexId] ).setOverlay( Constants.HexOverlay.Obstructed );
        }
        else
        {
            // log debug message
        }
    }
    
    public void occupyHex( string hexId )
    {
        if( hexId != null )
        {
            ( hexagons[hexId] ).isOccupied = true;
        }
        else
        {
            // log debug message
        }
    }

    public void placePlayerToken( Vector3 position, string hexId )
    {
        GameObject token = ( GameObject ) tokens[player.id];
        token.SetActive( true );
        token.transform.position = position;

        clearHex( player.currrentHexId );
        player.previousHexId = player.currrentHexId;
        
        occupyHex( hexId );
        player.currrentHexId = hexId;

        highlightHexes( getAvailableHexIds( hexId, 2, false ) );
    }

    public void rotatePlayerToken( int direction )
    {
        player.facing = direction;

        ( ( GameObject ) tokens[player.id] ).transform.rotation = Constants.FACING[player.facing];
    }

    public void toggleMusic( bool musicState )
    {
        if( music != null )
        {
            musicOn = musicState;

            if( musicOn )
            {
                music.Play();
            }
            else
            {
                music.Stop();
            }
        }
    }

    public void toggleSfx( bool sfxState )
    {
        sfxOn = sfxState;
    }

    public void unlockHexGrid()
    {
        foreach( Hexagon hexagon in hexagons.Values )
        {
            hexagon.isSelectable = true;
        }
    }
}
