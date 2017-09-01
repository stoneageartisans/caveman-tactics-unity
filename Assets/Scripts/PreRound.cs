using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreRound : MonoBehaviour
{
    public Button gotItButton;
    public GameObject hexGrid;
    public GameObject instructionDialog;
    public Text actionPoints;
    public Text agility;
    public Text brains;
    public Text brawn;
    public Text damage;
    public Text defense;
    public Text hitPoints;
    public Text stamina;
    public Text unspentPoints;
    public Text weapon;

	void Start()
    {
        brawn.text = ApplicationManager.instance.player.brawn.ToString();
        agility.text = ApplicationManager.instance.player.agility.ToString();
        brains.text = ApplicationManager.instance.player.brains.ToString();
        stamina.text = ApplicationManager.instance.player.stamina.ToString();
        hitPoints.text = formatHitPoints();
        actionPoints.text = formatActionPoints();
        defense.text = ApplicationManager.instance.player.getDefense().ToString();
        weapon.text = ApplicationManager.instance.player.weapon.ToString();
        damage.text = ( ApplicationManager.instance.player.getDamageMin() + " - " + ApplicationManager.instance.player.getDamageMax() );
        unspentPoints.text = ApplicationManager.instance.player.unspentPoints.ToString();

        if( ApplicationManager.instance.newGameStarted )
        {
            ApplicationManager.instance.initializeHexGrid( ref hexGrid );
            showInstructionDialog();
        }

        ApplicationManager.instance.checkAppState();
	}

    string formatActionPoints()
    {
        return (
            ApplicationManager.instance.player.getActionPoints().ToString() +
            " / " +
            ApplicationManager.instance.player.getAction()
        );
    }

    string formatHitPoints()
    {
        return (
            ( ApplicationManager.instance.player.getHitPoints() - ApplicationManager.instance.player.getWounds() ).ToString() +
            " / " +
            ApplicationManager.instance.player.getHitPoints().ToString()
        );
    }

    void hideInstructionDialog()
    {
        instructionDialog.SetActive( false );

        ApplicationManager.instance.unlockHexGrid();
    }

    void showInstructionDialog()
    {
        ApplicationManager.instance.lockHexGrid();

        instructionDialog.SetActive( true );
    }

    public void characterButtonClicked()
    {
        ApplicationManager.instance.appState = Constants.AppState.Character;
        ApplicationManager.instance.changeScreen();
    }

    public void gotItButtonClicked()
    {
        hideInstructionDialog();
    }

    public void menuButtonClicked()
    {
        ApplicationManager.instance.appState = Constants.AppState.MainMenu;
        ApplicationManager.instance.changeScreen();
    }
}
