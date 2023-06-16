using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public static bool GameIsPaused;

    public GameObject uiVictoryMenu;

    public GameObject uiWinMenu;

    public GameObject uiLoadingScreen;

    private GameObject slotContainer;

    private GameObject player;

    private void Awake() {
        uiVictoryMenu.SetActive(false);
        uiLoadingScreen.SetActive(false);
        uiWinMenu.SetActive(false);
        GameIsPaused = false;
        slotContainer = GameObject.FindGameObjectWithTag("SlotContainer");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Resume() {
        Cursor.visible = false;
        uiVictoryMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        uiLoadingScreen.SetActive(true);
        player.transform.position = new Vector3(0f,0f,0f);
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(SaveTime());
        
    }

    public void Victory() {
        Cursor.visible = true;
        slotContainer.SetActive(false);
        uiVictoryMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame() {
        Time.timeScale = 1f;
        GameIsPaused = false;
        player.transform.position = new Vector3(0f, 0f, 0f);
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(0);  
    }

    public void YouWin() {
        StartCoroutine(finishBoss());
        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Player") {
            Victory();
        }
    }

    private IEnumerator SaveTime() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(DataPersistenceManager.instance.GetSceneIndex());
    }

    private IEnumerator finishBoss() {
        yield return new WaitForSeconds(1f);
        slotContainer.SetActive(false);
        uiWinMenu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
