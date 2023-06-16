using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trueque : MonoBehaviour
{
    public GameObject panelInventario;
    // Start is called before the first frame update
    public List<GameObject> dialogos;
    public List<GameObject> todos;
    private List<GameObject> orden;
    private List<int> nums;
    private List<int> ordenContactos;
    private List<GameObject> contactos;
    private int anteriorIndex = 5;
    public List<GameObject> texto;
    public List<GameObject> textosOrdenados;
    public List<InventoryItemData> objetos;
    public List<InventoryItemData> objetosOrdenados;

    public List<InventoryItemData> objetosDar;
    public List<InventoryItemData> objetosDarOrdenados;
    public InventoryItemData prueba;
    public GameObject inventario;
    private PersistentData inventory;

    public GameObject noDisponible;
    
    void Start()
    {
       inventario = GameObject.Find("Persistent Inventory");
       inventory = inventario.GetComponent<PersistentData>();
       //inventory.AddT(prueba);
       orden = new List<GameObject>();
       nums = new List<int>();
       ordenContactos = new List<int>();
       contactos = new List<GameObject>();
       for(int i = 0; i <4; i++){
           nums.Add(i);
       } 
       if(dialogos.Count > 1) arrayNum(nums);
       activarContactos();
       ordenar(dialogos);
    }


    void ordenar(List<GameObject> dialogos){
        int index = Random.Range(0, dialogos.Count);
        GameObject conver = dialogos[index];
        GameObject abandono = texto[index];
        InventoryItemData obj = objetos[index];
        InventoryItemData objDar = objetosDar[index];
        dialogos.RemoveAt(index);
        texto.RemoveAt(index);
        objetos.RemoveAt(index);
        objetosDar.RemoveAt(index);
        orden.Add(conver);
        textosOrdenados.Add(abandono);
        objetosOrdenados.Add(obj);
        objetosDarOrdenados.Add(objDar);
        if (dialogos.Count >= 1) {
            ordenar(dialogos);
        }
    }
    void arrayNum(List<int> nums){
        int index = Random.Range(0, nums.Count);
        int conver = nums[index];
        nums.RemoveAt(index);
        ordenContactos.Add(conver);
        if (nums.Count >= 1) {
            arrayNum(nums);
        }
    }
    void activarContactos(){

        int j = 0;
        for(int i=0; i<16; i += 4){
            contactos.Add(todos[i+ordenContactos[j]]);
            j++;
        }
        for(int i=0; i<4; i++){
            contactos[i].SetActive(true);
        }
    }

    public void contacto1(){
        if(anteriorIndex <=4){
            orden[anteriorIndex].SetActive(false);
        }
        orden[0].SetActive(true);
        anteriorIndex = 0;
    }
    public void contacto2(){
        if(anteriorIndex <=4){
            orden[anteriorIndex].SetActive(false);
        }
        orden[1].SetActive(true);
        anteriorIndex = 1;
    }
    public void contacto3(){
        if(anteriorIndex <=4){
            orden[anteriorIndex].SetActive(false);
        }
        orden[2].SetActive(true);
        anteriorIndex = 2;
    }
    public void contacto4(){
        if(anteriorIndex <=4){
            orden[anteriorIndex].SetActive(false);
        }
        orden[3].SetActive(true);
        anteriorIndex = 3;
    }

    public void accept(){
        if(inventory.RemoveT(objetosDarOrdenados[anteriorIndex]) != null ){
            inventory.AddT(objetosOrdenados[anteriorIndex]);
            GameObject.Find("Cruz").SetActive(false);
            GameObject.Find("Tick").SetActive(false);
            textosOrdenados[anteriorIndex].SetActive(true);
        }else{
            noDisponible.SetActive(true);
            StartCoroutine(desactivar(2));
        }        
        
    }
    public void cancel(){
        GameObject.Find("Cruz").SetActive(false);
        GameObject.Find("Tick").SetActive(false);
        textosOrdenados[anteriorIndex].SetActive(true);
    }

    private IEnumerator desactivar(int segundos){
        yield return new WaitForSeconds(segundos);
        noDisponible.SetActive(false);
    }
}
