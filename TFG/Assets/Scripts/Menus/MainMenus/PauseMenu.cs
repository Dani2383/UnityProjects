using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;

    public GameObject uiPauseMenu;
    public GameObject uiOptionsMenu;

    private GameObject slotContainer;

    // Update is called once per frame
    void Update()
    {
        if (VictoryMenu.GameIsPaused == true) return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void Awake() {
        uiPauseMenu.SetActive(false);
        GameIsPaused = false;
        slotContainer = GameObject.FindGameObjectWithTag("SlotContainer");
    }

    public void Resume() 
    {
        Cursor.visible = false;
        uiPauseMenu.SetActive(false);
        uiOptionsMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() 
    {
        Cursor.visible = true;
        slotContainer.SetActive(false);
        uiPauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame() {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }
}
