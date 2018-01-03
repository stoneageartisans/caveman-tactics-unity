using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button resumeButton;
    public GameObject confirmDialog;

    void Awake()
    {
        hideConfirmDialog();
        setResumeButtonState();
    }

    void hideConfirmDialog()
    {
        confirmDialog.SetActive(false);
    }

    void setResumeButtonState()
    {
        if(ApplicationManager.instance.resumeState == Constants.AppState.MainMenu)
        {
            resumeButton.interactable = false;
        }
        else
        {
            resumeButton.interactable = true;
        }
    }

    void showConfirmDialog()
    {
        confirmDialog.SetActive(true);
    }

    public void cancelButtonClicked()
    {
        hideConfirmDialog();
    }

    public void exitButtonClicked()
    {
        ApplicationManager.instance.appState = Constants.AppState.Exiting;
        ApplicationManager.instance.checkAppState();
    }

    public void okButtonClicked()
    {
        hideConfirmDialog();

        ApplicationManager.instance.reset();
        ApplicationManager.instance.newGameStarted = true;
        ApplicationManager.instance.resumeState = Constants.AppState.MainMenu;
        ApplicationManager.instance.appState = Constants.AppState.Character;
        ApplicationManager.instance.changeScreen();
    }

    public void resumeButtonClicked()
    {
        ApplicationManager.instance.appState = ApplicationManager.instance.resumeState;
        ApplicationManager.instance.changeScreen();
    }

    public void settingsButtonClicked()
    {
        ApplicationManager.instance.appState = Constants.AppState.Settings;
        ApplicationManager.instance.changeScreen();
    }

    public void startButtonClicked()
    {
        if( ApplicationManager.instance.resumeState != Constants.AppState.MainMenu )
        {
            showConfirmDialog();
        }
        else
        {
            ApplicationManager.instance.newGameStarted = true;
            ApplicationManager.instance.appState = Constants.AppState.Character;
            ApplicationManager.instance.changeScreen();
        }
    }
}
