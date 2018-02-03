using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

//  Locking undesirable functions

    void Start()
    {
        Screen.autorotateToPortraitUpsideDown = Screen.autorotateToPortrait = false;
        Screen.orientation = ScreenOrientation.Landscape;
    }

//  Main menu user interface simplest script

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/category/GAME");
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}
