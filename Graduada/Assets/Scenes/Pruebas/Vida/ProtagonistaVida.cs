using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProtagonistaVida : MonoBehaviour
{
    public GameObject proyectil;

    public GameObject persistentDataManager;

    [SerializeField] private GameObject inventoryObject;

    [SerializeField] private GameObject healthIcon;
    [SerializeField] private GameObject noItem;
    [SerializeField] private GameObject music; 

    public GameObject MenuMuerteUI, cambita;
    public Image[] corazones;
    public int health;
    GameObject player;
    AudioSource sonido;
    bool semaforo = true;
    MenuMuerte muero;
    bool pierdo;
    Vector3 mouse, direction;
    Ray castPoint;

    //100 health 


    void Start()
    {
        persistentDataManager = GameObject.FindGameObjectWithTag("persistent");
        sonido = GameObject.Find("golpeSectario").GetComponent<AudioSource>();
        pierdo = true;
    }

    private IEnumerator invencibilidad(){
        yield return new WaitForSeconds(1);
        pierdo = true;
    }
    public void LoseHealth()
    {
        if(pierdo){
            pierdo = false;
            health--;
            corazones[health].enabled = false;
        }

        if (health == 0)
        {
            persistentDataManager.GetComponent<PersistentData>().returnWithDeath();
            MenuMuerteUI.SetActive(true);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
            muero = cambita.GetComponent<MenuMuerte>();
            muero.GameDeath = true;
        }
    }

    public void GainHealth()
    {
        if (corazones.Length > health)
        {
            if(inventoryObject.GetComponent<InventorySystem>().pizza()){
                corazones[health].enabled = true;
                health++;
                StartCoroutine(imageShow());
            }else{
                StartCoroutine(noIconShow());
            }
        }
    }
    private void Update()
    {
        if(!pierdo) StartCoroutine(invencibilidad());


        if (Input.GetKeyDown(KeyCode.M))
        {
            LoseHealth();
        }
        if (Input.GetButton("item1"))
        {
            GainHealth();
        }
    }

    void OnCollisionEnter(UnityEngine.Collision collision)
    {

        if (collision.gameObject.name == "Proyectil(Clone)" || collision.gameObject.name == "Bolsa de basura(Clone)" )
        {   
            sonido.Play();
            Debug.Log("choca");
            LoseHealth();
            Destroy(collision.gameObject);
        }
        
    }

    private IEnumerator imageShow(){
        healthIcon.SetActive(true);
        yield return new WaitForSeconds(1);
        healthIcon.SetActive(false);
    }

    private IEnumerator noIconShow(){
        noItem.SetActive(true);
        yield return new WaitForSeconds(1);
        noItem.SetActive(false);
    }
}