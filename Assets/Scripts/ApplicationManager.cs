﻿using System.Collections;
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
    public int opponentStartingPoints = 10;
    public int playerStartingPoints = 10;
    public int statDefaultValue = 10;
    public int statMaxValue = 20;
    public int statMinValue = 5;

    [HideInInspector]
    public bool playerHasAdvantage;

    [HideInInspector]
    public int round;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public ArrayList opponents;

    [HideInInspector]
    public ArrayList roundOrder;

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
        playerHasAdvantage = true;
        round = 1;
	}
	
	void Update()
    {
        // TODO
	}

    void determineInitiative()
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
            Debug.Log( " Combatant id " + ids[i] + " is #" + i + " in round with " + initiatives[i] + " initiative" );
        }

        roundOrder.Reverse();
    }

    void determineTacticalAdvantage()
    {
        playerHasAdvantage = true;

        int playerResult = statCheck( player.brains );

        foreach( Player opponent in opponents )
        {
            if( statCheck( opponent.brains ) > playerResult )
            {
                playerHasAdvantage = false;
                break;
            }
        }
    }

    void generateOpponents()
    {
        opponents.Clear();

        for( int i = 0; i < round; i ++ )
        {
            opponents.Add( new Opponent( statDefaultValue, statMinValue, statMaxValue, opponentStartingPoints, i + 1 ) );
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
        Debug.Log( sfxState.ToString() );
        Debug.Log( sfxOn.ToString() );
    }
}
