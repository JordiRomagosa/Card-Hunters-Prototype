using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ButtonPressedStartGame()
    {
        //SceneManager.LoadScene("Game");
    }
    
    public void ButtonPressedManageDeck()
    {
        SceneManager.LoadScene("DeckShowcase");
    }
    
    public void ButtonPressedExitGame()
    {
        Application.Quit();
    }
}
