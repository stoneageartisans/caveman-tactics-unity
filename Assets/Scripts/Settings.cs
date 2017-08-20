using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle sfxToggle;

    void Awake()
    {
        musicToggle.isOn = ApplicationManager.instance.musicOn;
        sfxToggle.isOn = ApplicationManager.instance.sfxOn;
    }

    public void musicToggleChanged()
    {
        ApplicationManager.instance.toggleMusic( musicToggle.isOn );
    }

    public void sfxToggleChanged()
    {
        ApplicationManager.instance.toggleSfx( sfxToggle.isOn );
    }

    public void okButtonClicked()
    {
        ApplicationManager.instance.appState = Constants.AppState.MainMenu;
        ApplicationManager.instance.changeScreen();
    }
}
