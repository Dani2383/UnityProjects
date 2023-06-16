using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuPausa : MonoBehaviour
{
    public static bool GameIsPaused= false;
    public bool GameIsInMenuPause= true;
    public GameObject MenuPausaUI, MenuOpcionesUI, MenuInventarioUI, botonVolverJuego,botonInventario,botonOpciones,botonVolverMenu, cambita;

    private GameObject inventory;

    LogicaCambioNivel cambiar = new LogicaCambioNivel();

    MenuPausaOpciones misOpciones;

    MenuInventario miInventario;

    // Update is called once per frame
    private void Start() {
      MenuPausaUI.SetActive(false);

      inventory = GameObject.Find("Persistent Inventory");

    }
    private void Update()
    {
     if(GameIsPaused && GameIsInMenuPause){
        if(EventSystem.current.currentSelectedGameObject==null){
          StartCoroutine(Esperar());
       }
     }
     if(Input.GetKeyDown(KeyCode.I) && !GameIsPaused){
       MenuInventarioUI.SetActive(true);
       Time.timeScale = 0f;
       GameIsPaused = true;
       miInventario=cambita.GetComponent<MenuInventario>();
       miInventario.GameIsInControles = true;
     }

      if(Input.GetKeyDown(KeyCode.Return) &&  GameIsPaused && GameIsInMenuPause && EventSystem.current.currentSelectedGameObject!=null){
       
       switch(EventSystem.current.currentSelectedGameObject.name){
         case "VolverAJugar":
         Resume();
         break;
         case "Volver":
         LoadMenu();
         break;
         case "Inventario":
         GameIsInMenuPause = false;
         MenuInventarioUI.SetActive(true);
         MenuPausaUI.SetActive(false);
         EventSystem.current.SetSelectedGameObject(null);
         miInventario=cambita.GetComponent<MenuInventario>();
         miInventario.GameIsInControles = true;
         break;
         case "Opciones":
         GameIsInMenuPause = false;
         MenuOpcionesUI.SetActive(true);
         MenuPausaUI.SetActive(false);
         EventSystem.current.SetSelectedGameObject(null);
         misOpciones=cambita.GetComponent<MenuPausaOpciones>();
         misOpciones.GameIsInOptions = true;
         break;
         default :
         break;
       }
      }
        if(Input.GetKeyDown(KeyCode.Escape)){
                Pause();
        }
    }
    public void Resume(){
      MenuPausaUI.SetActive(false);
      Time.timeScale = 1f;
      GameIsPaused = false; 
      EventSystem.current.SetSelectedGameObject(null);
    }
    public void Pause(){
      MenuPausaUI.SetActive(true);
      Time.timeScale = 0f;
      GameIsPaused = true;
    }

    public void LoadMenu(){
       EventSystem.current.SetSelectedGameObject(null);
       
       inventory.GetComponent<PersistentData>().returnHome(0.0f);

       cambiar.cargarMenuPrincipal();
       Time.timeScale = 1f;
    }

    IEnumerator Esperar(){
        yield return new WaitForSecondsRealtime(0.2f);
        EventSystem.current.SetSelectedGameObject(botonVolverJuego); 
  }
}
