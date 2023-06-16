using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject uiDeathMenu;

    public void Awake(){
        uiDeathMenu.SetActive(false);
    }

    public void ImDead() {
        StartCoroutine("DeathTime");
        
    }

    public void Resume() {
        Cursor.visible = false;
        uiDeathMenu.SetActive(false);
        Time.timeScale = 1f;
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadScene(DataPersistenceManager.instance.GetSceneIndex());
    }

    public void QuitGame() {
        Time.timeScale = 1f;
        uiDeathMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private IEnumerator DeathTime() {
        yield return new WaitForSeconds(1.5f);
        uiDeathMenu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

}
