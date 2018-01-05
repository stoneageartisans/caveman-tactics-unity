using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    public static ApplicationManager instance;

    public Constants.AppState appState = Constants.AppState.TitleScreen;
    public Constants.AppState resumeState = Constants.AppState.MainMenu;
    public bool musicOn = true;
    public bool sfxOn = true;
    public int opponentStartingPoints = 10;
    public int playerStartingPoints = 10;
    public int statDefaultValue = 10;
    public int statMaxValue = 20;
    public int statMinValue = 5;
     
    public AudioSource music;
    public GameObject hexGrid;
    public GameObject opponentToken;
    public GameObject playerToken;

    [HideInInspector] public ArrayList opponents;
    [HideInInspector] public ArrayList roundOrder;
    [HideInInspector] public bool newGameStarted;
    [HideInInspector] public bool opponentsPlaced;
    [HideInInspector] public bool playerHasAdvantage;    
    [HideInInspector] public Dictionary<string, Hexagon> hexagons;
    [HideInInspector] public int round;
    [HideInInspector] public Player player;

    ArrayList tokens;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
            DontDestroyOnLoad(hexGrid);
        }
        else
        {
            Debug.Log("ERROR: An instance of " + gameObject.name + " already exists.");
            Destroy(gameObject);
        }
    }

	void Start()
    {
        Invoke("changeScreen", Constants.SPLASH_DELAY);

        if(music != null)
        {
            music.Play();
        }

        player = new Player(statDefaultValue, statMinValue, statMaxValue, playerStartingPoints, 0);
        opponents = new ArrayList();
        roundOrder = new ArrayList();
        newGameStarted = false;
        opponentsPlaced = false;
        round = Constants.STARTING_ROUND;

        initializeHexGrid();
	}
	
	void Update()
    {
        // TODO
	}

    public void beginRound()
    {
        lockHexGrid();

        lockPlayerStatMinimums();

        if(!playerHasAdvantage)
        {
            placeOpponents();
        }

        //resumeState = Constants.AppState.AiTurn OR Constants.AppState.PlayerTurn
        //appState = Constants.AppState.AiTurn OR Constants.AppState.PlayerTurn

        //changeScreen();
    }

    public void changeScreen()
    {
        SceneManager.LoadScene( appState.ToString() );
    }

    public void checkAppState()
    {
        switch(appState)
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
    
    void clearHex(string hexId)
    {
        if(hexId != null)
        {
            ( hexagons[hexId] ).isObstructed = false;
            ( hexagons[hexId] ).isOccupied = false;
            ( hexagons[hexId] ).setOverlay(Constants.HexOverlay.Blank);
        }
        else
        {
            Debug.Log("ERROR: The hex Id is null");
        }
    }

    void clearHexGrid()
    {
        foreach(string hexId in hexagons.Keys)
        {
            clearHex(hexId);
        }
    }

    void createTokens()
    {
        tokens = new ArrayList();

        tokens.Insert(player.id, Instantiate<GameObject>(playerToken).gameObject);

        foreach(Opponent opponent in opponents)
        {
            tokens.Insert(opponent.id, Instantiate<GameObject>(opponentToken).gameObject);
        }

        foreach(GameObject token in tokens)
        {
            token.SetActive(false);
        }
    }

    void determineInitiative()
    {
        if(newGameStarted)
        {
            roundOrder.Clear();

            int[] ids = new int[round + 1];
            float[] initiatives = new float[round + 1];

            ids[0] = player.id;
            initiatives[0] = player.getInitiative();

            foreach(Opponent opponent in opponents)
            {
                ids[opponent.id] = opponent.id;
                initiatives[opponent.id] = opponent.getInitiative();
            }

            int tempId = 0;
            float tempInitiative = 0;

            for(int i = round; i > -1; i --)
            {
                for(int j = round - 1; j > -1; j --)
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

            for(int i = 0; i <= round; i ++)
            {
                roundOrder.Insert( i, ids[i] );
            }

            roundOrder.Reverse();
        }
    }

    void determineTacticalAdvantage()
    {
        if(newGameStarted)
        {
            playerHasAdvantage = true;

            int playerResult = statCheck(player.brains);

            foreach(Opponent opponent in opponents)
            {
                if( statCheck(opponent.brains) > playerResult )
                {
                    playerHasAdvantage = false;
                    break;
                }
            }
        }
    }

    public void exitGame()
    {
        Application.Quit();
    }
        
    void generateOpponents()
    {
        if(newGameStarted)
        {
            opponents.Clear();

            for(int i = 0; i < round; i ++)
            {
                opponents.Insert( i, new Opponent(statDefaultValue, statMinValue, statMaxValue, opponentStartingPoints, i + 1) );
            }
        }
    }

    List<string> getAllHexIdsAsList()
    {
        List<string> allHexIds = new List<string>();

        foreach(string hexId in hexagons.Keys)
        {
            allHexIds.Add(hexId);
        }

        return allHexIds;
    }

    float getAngleToHexagon(string currentHexId, string targetHexId)
    {
        float angle;

        Vector3 direction = ( hexagons[targetHexId] ).transform.position - ( hexagons[currentHexId] ).transform.position; 

        angle = Mathf.Atan2( direction.y, direction.x ) * (-180) / Mathf.PI;

        if(angle < 0)
        {
            angle += 360;
        }

        Debug.Log("Result of getAngleToHexagon(" + currentHexId + ", " + targetHexId + ") is " + angle);

        return angle;
    }

    List<string> getAvailableHexIds(string hexId, int hexDistance, bool insideRange = true)
    {
        List<string> idList = new List<string>();

        float distance = (float) hexDistance * Constants.HEX_SPACING;

        Vector3 hexPosition = ( hexagons[hexId] ).transform.position;

        foreach(Hexagon hexagon in hexagons.Values)
        {
            if(!hexagon.isOccupied && !hexagon.isObstructed)
            {
                if(insideRange)
                { 
                    if( Vector3.Distance(hexPosition, hexagon.transform.position) < distance )
                    {
                        idList.Add(hexagon.id);
                    }
                }
                else
                {
                    if( Vector3.Distance(hexPosition, hexagon.transform.position) > distance )
                    {
                        idList.Add(hexagon.id);
                    }
                }
            }
        }

        return idList;
    }


    int getDiceRoll()
    {
        // Roll 2 "11-sided" dice
        int die1 = Random.Range(0, 11); // 0 to 10
        int die2 = Random.Range(0, 11); // 0 to 10

        // Returns a number from 0 to 20, slightly weighted towards 10
        return (die1 + die2);
    }

    List<string> getDiffOfHexIds(List<string> primaryHexIds, List<string> hexIdsToRemove)
    {
        List<string> diffedHexIds = primaryHexIds;

        foreach(string hexIdToRemove in hexIdsToRemove)
        {
            diffedHexIds.Remove(hexIdToRemove);
        }

        return diffedHexIds;
    }

    int getDirection(float angle)
    {
        if(angle > 30 && angle <= 90)
        {
            return Constants.SOUTHEAST;
        }
        else if(angle > 90 && angle <= 150)
        {
            return Constants.SOUTHWEST;
        }
        else if(angle > 150 && angle <= 210)
        {
            return Constants.WEST;
        }
        else if(angle > 210 && angle <= 270)
        {
            return Constants.NORTHWEST;
        }
        else if(angle > 270 && angle <= 330)
        {
            return Constants.NORTHEAST;
        }
        else
        {
            return Constants.EAST;
        }
    }

    string getRandomHexId(List<string> hexIdList)
    {
        return ( hexIdList[ Random.Range(0, hexIdList.Count) ] );
    }

    public void highlightHex(string hexId)
    {
        if(hexId != null)
        {
            ( hexagons[hexId] ).isHighlighted = true;
            ( hexagons[hexId] ).setOverlay(Constants.HexOverlay.Highlighted);
        }
        else
        {
            // log debug message
        }
    }

    public void highlightHexes(List<string> hexIdList)
    {
        foreach(string hexId in hexIdList)
        {
            if(hexId != null)
            {
                ( hexagons[hexId] ).isHighlighted = true;
                ( hexagons[hexId] ).setOverlay(Constants.HexOverlay.Highlighted);
            }
            else
            {
                // log debug message
            }
        }
    }

    public void initializeHexGrid()
    {
        hexagons = new Dictionary<string, Hexagon>();

        Component[] hexArray = hexGrid.GetComponentsInChildren( typeof(Hexagon) );

        foreach(Hexagon hexagon in hexArray)
        {
            hexagons.Add(hexagon.id, hexagon);
        }
    }

    public void lockHexGrid()
    {
        foreach(Hexagon hexagon in hexagons.Values)
        {
            hexagon.isSelectable = false;
        }
    }

    void lockPlayerStatMinimums()
    {
        player.brawnMin = player.brawn;
        player.agilityMin = player.agility;
        player.brainsMin = player.brains;
        player.staminaMin = player.stamina;
    }

    public void obstructHex(string hexId)
    {
        if( hexId != null )
        {
            ( hexagons[hexId] ).isObstructed = true;
            ( hexagons[hexId] ).setOverlay(Constants.HexOverlay.Obstructed);
        }
        else
        {
            // log debug message
        }
    }

    public void occupyHex(string hexId)
    {
        if(hexId != null)
        {
            ( hexagons[hexId] ).isOccupied = true;
        }
        else
        {
            // log debug message
        }
    }

    public void placeOpponents()
    {
        if(opponentsPlaced)
        {
            foreach(Opponent opponent in opponents)
            {
                placeOpponentToken(opponent.currentHexId, opponent);
            }
        }
        else
        {
            string hexId = null;
            int direction = 0;

            if(playerHasAdvantage)
            {
                foreach(Opponent opponent in opponents)
                {
                    Debug.Log( "Tactical Stance: " + opponent.tacticalStance.ToString() );

                    switch(opponent.tacticalStance)
                    {
                        case Constants.TacticalStance.Aggressive:
                            // Place in center area of grid
                            hexId = Constants.CENTER_HEXES[Random.Range(0, Constants.CENTER_HEXES.Length)];
                            direction = Random.Range(0, Constants.MAX_DIRECTIONS);
                            break;
                        case Constants.TacticalStance.Balanced:
                            // Place in random hex
                            hexId = getRandomHexId( getAllHexIdsAsList() );
                            direction = Random.Range(0, Constants.MAX_DIRECTIONS);
                            break;
                        case Constants.TacticalStance.Defensive:
                            // Place in a corner hex
                            hexId = Constants.CORNER_HEXES[Random.Range(0, Constants.CORNER_HEXES.Length)];
                            direction = getDirection( getAngleToHexagon( hexId, Constants.CENTER_HEXES[0] ) );
                            break;
                    }

                    Debug.Log("Direction is " + direction);

                    placeOpponentToken(hexId, opponent);
                    rotateOpponentToken(direction, opponent);
                }
            }
            else
            {
                foreach(Opponent opponent in opponents)
                {
                    Debug.Log( "Tactical Stance: " + opponent.tacticalStance.ToString() );

                    switch(opponent.tacticalStance)
                    {
                        case Constants.TacticalStance.Aggressive:
                            // Place adjacent to player
                            hexId = getRandomHexId(getAvailableHexIds(player.currentHexId, 1));
                            direction = getDirection( getAngleToHexagon( hexId, player.currentHexId ) );
                            break;
                        case Constants.TacticalStance.Balanced:
                            // Place 2 - 3 hexes from player
                            hexId = getRandomHexId( getDiffOfHexIds( getAvailableHexIds(player.currentHexId, 3), getAvailableHexIds(player.currentHexId, 1) ) );
                            direction = getDirection( getAngleToHexagon( hexId, player.currentHexId ) );
                            break;
                        case Constants.TacticalStance.Defensive:
                            // Place 4+ hexes from player
                            hexId = getRandomHexId(getAvailableHexIds(player.currentHexId, 3, false));
                            direction = getDirection( getAngleToHexagon( hexId, player.currentHexId ) );
                            break;
                    }

                    Debug.Log("Direction is " + direction);

                    placeOpponentToken(hexId, opponent);
                    rotateOpponentToken(direction, opponent);
                }
            }

            opponentsPlaced = true;
        }
    }

    public void placeOpponentToken(string hexId, Opponent opponent)
    {
        ( (GameObject) tokens[opponent.id] ).SetActive(true);
        ( (GameObject) tokens[opponent.id] ).transform.position = ( hexagons[hexId] ).transform.position;

        if(opponent.currentHexId != null)
        {
            clearHex(opponent.currentHexId);
        }

        opponent.previousHexId = opponent.currentHexId;

        occupyHex(hexId);
        opponent.currentHexId = hexId;
    }

    public void placePlayerToken(string hexId)
    {
        ( (GameObject) tokens[player.id] ).SetActive(true);
        ( (GameObject) tokens[player.id] ).transform.position = ( hexagons[hexId] ).transform.position;

        if(player.currentHexId != null)
        {
            clearHex(player.currentHexId);
        }

        player.previousHexId = player.currentHexId;

        occupyHex(hexId);
        player.currentHexId = hexId;
    }

    void placeTokens()
    {
        if(newGameStarted)
        {
            if(playerHasAdvantage)
            {
                placeOpponents();
            }
            else
            {
                // placing of opponents will wait until begin button clicked
            }
        }
        else
        {
            placePlayerToken(player.currentHexId);
            rotatePlayerToken(player.facing);

            if(opponentsPlaced)
            {
                foreach(Opponent opponent in opponents)
                {
                    placeOpponentToken(opponent.currentHexId, opponent);
                    rotateOpponentToken(opponent.facing, opponent);
                }
            };
        }
    }

    public void reset()
    {
        player = new Player(statDefaultValue, statMinValue, statMaxValue, playerStartingPoints, 0);
        opponents = new ArrayList();
        roundOrder = new ArrayList();
        newGameStarted = false;
        opponentsPlaced = false;
        round = Constants.STARTING_ROUND;
        resumeState = Constants.AppState.MainMenu;
        clearHexGrid();
    }

    public void rotatePlayerToken(int direction)
    {
        player.facing = direction;

        ( (GameObject) tokens[player.id] ).transform.rotation = Constants.FACING[player.facing];
    }

    public void rotateOpponentToken(int direction, Opponent opponent)
    {
        opponent.facing = direction;

        ( (GameObject) tokens[opponent.id] ).transform.rotation = Constants.FACING[opponent.facing];
    }

    int statCheck(int statValue)
    {
        // Success is defined as a "roll" that is less than or equal to the stat
        return ( statValue - getDiceRoll() );
    }

    public void toggleMusic(bool musicState)
    {
        if(music != null)
        {
            musicOn = musicState;

            if(musicOn)
            {
                music.Play();
            }
            else
            {
                music.Pause();
            }
        }
    }

    public void toggleSfx(bool sfxState)
    {
        sfxOn = sfxState;
    }

    public void unlockHexGrid()
    {
        foreach(Hexagon hexagon in hexagons.Values)
        {
            hexagon.isSelectable = true;
        }
    }
}
