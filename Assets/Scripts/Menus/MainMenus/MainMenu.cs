using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    private void Awake() {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        AudioManager.instance.StopMusic("LevelTheme");
        AudioManager.instance.PlayMusic("MenuTheme");
    }

    public void PlayGame() {
        DataPersistenceManager.instance.NewGame();
        DataPersistenceManager.instance.SaveGame();
        AudioManager.instance.StopMusic("MenuTheme");
        AudioManager.instance.PlayMusic("LevelTheme");
        SceneManager.LoadScene(1);
    }

    public void ContinueGame() {
        try{ 
            SceneManager.LoadScene(DataPersistenceManager.instance.GetSceneIndex());
            AudioManager.instance.StopMusic("MenuTheme");
            AudioManager.instance.PlayMusic("LevelTheme");
        }catch(Exception e){
            PlayGame();
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PlayMenuButton() {
        AudioManager.instance.PlaySound("MenuButton");
    }
}
