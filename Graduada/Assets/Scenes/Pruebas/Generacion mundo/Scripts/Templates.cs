using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Templates : MonoBehaviour
{
    public GameObject[] BottomRooms;
    public GameObject[] TopRooms;
    public GameObject[] LeftRooms;
    public GameObject[] RightRooms;
    public GameObject enemy;
    public GameObject carga;
    public int maxHab;
    public static int numHab;
    public static bool bar = false;
    private bool proseguir = false;
    void Start(){
        // StartCoroutine(reiniciar(0.5f));
        StartCoroutine(esperar(7));
            
        }
    void Update(){
         if(bar == true){
             carga.SetActive(false);
         }
    }

        // private IEnumerator reiniciar(float segundos){
        //     yield return new WaitForSeconds(segundos);
        //     if(bar == true){
        //         Debug.Log(bar);
        //         bar = false;
        //         SceneManager.LoadScene(4);
        //     }else{ proseguir = true;}
        // }
        private IEnumerator esperar(int segundos){
            yield return new WaitForSeconds(segundos);
            if(bar == false){
                SceneManager.LoadScene(4);
            }
        }
        
}
