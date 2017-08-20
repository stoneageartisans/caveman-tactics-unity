using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject agility;
    public GameObject brains;
    public GameObject brawn;
    public GameObject stamina;
    public Text actionPoints;
    public Text damage;
    public Text defense;
    public Text hitPoints;
    public Text unspentPoints;
    public Text weapon;

    Text agilityValue;
    Text brainsValue;
    Text brawnValue;
    Text staminaValue;

    int previousAgilgity;
    int previousBrains;
    int previousBrawn;
    int previousStamina;
    int previousUnspentPoints;

	void Awake()
    {
        brawnValue = brawn.GetComponentsInChildren<Text>()[1];
        agilityValue = agility.GetComponentsInChildren<Text>()[1];
        brainsValue = brains.GetComponentsInChildren<Text>()[1];
        staminaValue = stamina.GetComponentsInChildren<Text>()[1];
	}
	
    void Start()
    {
        previousBrawn = ApplicationManager.instance.player.brawn;
        previousAgilgity = ApplicationManager.instance.player.agility;
        previousBrains = ApplicationManager.instance.player.brains;
        previousStamina = ApplicationManager.instance.player.stamina;
        previousUnspentPoints = ApplicationManager.instance.player.unspentPoints;

        updateFields();
    }

    void decreaseStat( ref int stat, int min )
    {
        if( stat > min )
        {
            stat --;
            ApplicationManager.instance.player.unspentPoints ++;
        }

        ApplicationManager.instance.player.calculateStats();
        updateFields();
    }

    void increaseStat( ref int stat, int max )
    {
        if( stat < max )
        {
            if( ApplicationManager.instance.player.unspentPoints > 0 )
            {
                stat ++;
                ApplicationManager.instance.player.unspentPoints --;
            }
        }

        ApplicationManager.instance.player.calculateStats();
        updateFields();
    }

    void undoChanges()
    {
        ApplicationManager.instance.player.brawn = previousBrawn;
        ApplicationManager.instance.player.agility = previousAgilgity;
        ApplicationManager.instance.player.brains = previousBrains;
        ApplicationManager.instance.player.stamina = previousStamina;
        ApplicationManager.instance.player.unspentPoints = previousUnspentPoints;

        ApplicationManager.instance.player.calculateStats();
    }

    void updateFields()
    {
        brawnValue.text = ApplicationManager.instance.player.brawn.ToString();
        agilityValue.text = ApplicationManager.instance.player.agility.ToString();
        brainsValue.text = ApplicationManager.instance.player.brains.ToString();
        staminaValue.text = ApplicationManager.instance.player.stamina.ToString();

        hitPoints.text = ApplicationManager.instance.player.getHitPoints().ToString();
        defense.text = ApplicationManager.instance.player.getDefense().ToString();
        unspentPoints.text = ApplicationManager.instance.player.unspentPoints.ToString();
        actionPoints.text = ApplicationManager.instance.player.getAction().ToString();
        weapon.text = ApplicationManager.instance.player.weapon.ToString();
        damage.text = ( ApplicationManager.instance.player.getDamageMin() + " - " + ApplicationManager.instance.player.getDamageMax() );
    }

    public void acceptButtonClicked()
    {
        switch( ApplicationManager.instance.resumeState )
        {
            case Constants.AppState.MainMenu:
                ApplicationManager.instance.resumeState = Constants.AppState.PreRound;
                ApplicationManager.instance.appState = Constants.AppState.PreRound;
                break;
            case Constants.AppState.PreRound:
                ApplicationManager.instance.appState = Constants.AppState.PreRound;
                break;
            case Constants.AppState.PostRound:
                ApplicationManager.instance.appState = Constants.AppState.PostRound;
                break;
        }

        ApplicationManager.instance.changeScreen();
    }

    public void cancelButtonClicked()
    {
        undoChanges();

        switch( ApplicationManager.instance.resumeState )
        {
            case Constants.AppState.MainMenu:
                ApplicationManager.instance.resumeState = Constants.AppState.MainMenu;
                ApplicationManager.instance.appState = Constants.AppState.MainMenu;
                break;
            case Constants.AppState.PreRound:
                ApplicationManager.instance.appState = Constants.AppState.PreRound;
                break;
            case Constants.AppState.PostRound:
                ApplicationManager.instance.appState = Constants.AppState.PostRound;
                break;
        }

        ApplicationManager.instance.changeScreen();
    }

    public void minusButtonClicked( string statName )
    {
        switch( statName.ToLower() )
        {
            case "brawn":
                decreaseStat( ref ApplicationManager.instance.player.brawn, ApplicationManager.instance.player.brawnMin );
                break;
            case "agility":
                decreaseStat( ref ApplicationManager.instance.player.agility, ApplicationManager.instance.player.agilityMin );
                break;
            case "brains":
                decreaseStat( ref ApplicationManager.instance.player.brains, ApplicationManager.instance.player.brainsMin );
                break;
            case "stamina":
                decreaseStat( ref ApplicationManager.instance.player.stamina, ApplicationManager.instance.player.staminaMin );
                break;
        }
    }

    public void plusButtonClicked( string statName )
    {
        switch( statName.ToLower() )
        {
            case "brawn":
                increaseStat( ref ApplicationManager.instance.player.brawn, ApplicationManager.instance.player.statMax );
                break;
            case "agility":
                increaseStat( ref ApplicationManager.instance.player.agility, ApplicationManager.instance.player.statMax );
                break;
            case "brains":
                increaseStat( ref ApplicationManager.instance.player.brains, ApplicationManager.instance.player.statMax );
                break;
            case "stamina":
                increaseStat( ref ApplicationManager.instance.player.stamina, ApplicationManager.instance.player.statMax );
                break;
        }
    }
}
