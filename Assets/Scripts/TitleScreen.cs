using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{	
	void Update()
    {
        if( Input.anyKeyDown )
        {
            ApplicationManager.instance.appState = Constants.AppState.MainMenu;
            ApplicationManager.instance.changeScreen();
        }
	}
}
