using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour, IDataPersistence
{
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private GameObject arrow, decoy,fireball;
    GameObject thrownDecoy;
    private Vector2 mousePos, rotation;
    public string[] objectsArray;
    public int[] objectsQuantity;
    public GameObject[] slots, slotsOrdered;
    private int isSelected;
    private GameObject slotContainer;

    //Decoy -->
    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    bool canUseObject;

    private float launchForce;
    private bool reloaded = true;

    private void Update() {

        if (DialogueManager.GetInstance().DialogueIsPlaying) {
            return;
        }

        if (VictoryMenu.GameIsPaused == true || PauseMenu.GameIsPaused == true) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Cursor.visible = false;
            slots[isSelected].GetComponent<Image>().color = new Color(255,255,255,0.4f);
            isSelected = 0;
            slots[isSelected].GetComponent<Image>().color = Color.red;
            StartCoroutine(ShowInventory());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Cursor.visible = false;
            slots[isSelected].GetComponent<Image>().color = new Color(255, 255, 255, 0.4f);
            isSelected = 1;
            slots[isSelected].GetComponent<Image>().color = Color.red;
            StartCoroutine(ShowInventory());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Cursor.visible = true;
            slots[isSelected].GetComponent<Image>().color = new Color(255, 255, 255, 0.4f);
            isSelected = 2;
            slots[isSelected].GetComponent<Image>().color = Color.red;
            StartCoroutine(ShowInventory());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            Cursor.visible = false;
            slots[isSelected].GetComponent<Image>().color = new Color(255, 255, 255, 0.4f);
            isSelected = 3;
            slots[isSelected].GetComponent<Image>().color = Color.red;
            StartCoroutine(ShowInventory());
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            Cursor.visible = false;
            slots[isSelected].GetComponent<Image>().color = new Color(255, 255, 255, 0.4f);
            isSelected = 4;
            slots[isSelected].GetComponent<Image>().color = Color.red;
            StartCoroutine(ShowInventory());
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            Cursor.visible = false;
            slots[isSelected].GetComponent<Image>().color = new Color(255, 255, 255, 0.4f);
            isSelected = 5;
            slots[isSelected].GetComponent<Image>().color = Color.red;
            StartCoroutine(ShowInventory());
        }

        try {
            if(slots[isSelected].transform.GetChild(1).name == "Decoy Button(Clone)"){
                if(points == null){
                    points = new GameObject[numberOfPoints];
                    for(int i = 0; i < numberOfPoints; i++){
                        points[i] = Instantiate(point, arrowTransform.position, Quaternion.identity);
                    }
                }
                GetMousePos();
                launchForce = Vector2.Distance(arrowTransform.position,  Camera.main.ScreenToWorldPoint(Input.mousePosition)) * 3;

                for(int i = 0; i < numberOfPoints; i++){
                    points[i].transform.position = PointPosition(i * spaceBetweenPoints);
                    points[i].GetComponent<SpriteRenderer>().enabled = true;
                }

            } else {
                if(points != null){
                    for(int i = 0; i < numberOfPoints; i++){
                        points[i].GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        } catch(Exception e){
            if(points != null){
                for(int i = 0; i < numberOfPoints; i++){
                    points[i].GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        try {
            if(Input.GetMouseButtonDown(0) && canUseObject && !gameObject.GetComponent<Movement>().IsDucked()){
                canUseObject = false;
                StartCoroutine("UsingObject");
                GameObject slot = slots[isSelected].transform.GetChild(1).gameObject;
                TextMeshProUGUI text = slots[isSelected].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

                switch (slot.name) {
                    case "Health Button(Clone)":
                        objectsQuantity[isSelected] -= 1;
                        AudioManager.instance.PlaySound("Potion");
                        if (objectsQuantity[isSelected] <= 0) {
                            objectsQuantity[isSelected] = 0;
                            Destroy(slot);
                            
                        }
                        gameObject.GetComponent<PlayerState>().health = Math.Min(6,gameObject.GetComponent<PlayerState>().health + 1);
                        break;
                    case "SuperHealth Button(Clone)":
                        objectsQuantity[isSelected] -= 1;
                        AudioManager.instance.PlaySound("Potion");
                        if (objectsQuantity[isSelected] <= 0) {
                            objectsQuantity[isSelected] = 0;
                            Destroy(slot);
                            
                        }
                        gameObject.GetComponent<PlayerState>().health = Math.Min(6, gameObject.GetComponent<PlayerState>().health + 3);
                        break;
                    case "MegaHealth Button(Clone)":
                        objectsQuantity[isSelected] -= 1;
                        AudioManager.instance.PlaySound("Potion");
                        if (objectsQuantity[isSelected] <= 0) {
                            objectsQuantity[isSelected] = 0;
                            Destroy(slot);
                            
                        }
                        gameObject.GetComponent<PlayerState>().health = Math.Min(6, gameObject.GetComponent<PlayerState>().health + 6);
                        break;
                    case "Sword Button(Clone)":
                        if(gameObject.GetComponent<PlayerState>().canAttack) gameObject.GetComponent<PlayerState>().StartAttack();
                        break;
                    case "WaterBall Button(Clone)":
                        GetMousePos();
                        if(Input.GetMouseButtonDown(0) && reloaded && !GameObject.FindWithTag("Player").GetComponent<Movement>().isHidded){
                            Debug.Log("Flecha paso 1");
                            AudioManager.instance.PlaySound("WaterBall");
                            SpawnArrow();
                            StartCoroutine("Reload");
                        }
                        break;
                    case "FireBall Button(Clone)":
                        GetMousePos();
                        if (Input.GetMouseButtonDown(0) && reloaded && !GameObject.FindWithTag("Player").GetComponent<Movement>().isHidded) {
                            Debug.Log("Flecha paso 1");
                            AudioManager.instance.PlaySound("FireBall");
                            SpawnFireBall();
                            StartCoroutine("Reload");
                        }
                        break;
                    case "Decoy Button(Clone)":
                        if(Input.GetMouseButtonDown(0) && reloaded && !GameObject.FindWithTag("Player").GetComponent<Movement>().isHidded){
                            Debug.Log(reloaded);
                            AudioManager.instance.PlaySound("Decoy");
                            ThrowDecoy();
                            StartCoroutine("DecoyDisapear");
                        }
                        break;
                    default :
                        break;
                }
                if (objectsQuantity[isSelected] <= 1) {
                    text.SetText("");
                }else { 
                    text.SetText(objectsQuantity[isSelected].ToString()); 
                }
            }
        } catch(Exception e){}
        
    }
    private void Start() {
        canUseObject = true;
        slots[isSelected].GetComponent<Image>().color = Color.red;
        slotContainer.SetActive(false);
    }

    private void Awake() {
        slots = GameObject.FindGameObjectsWithTag("Slot");
        IEnumerable<GameObject> slotsOrdered = slots.OrderBy(slots => slots.transform.name);
        slots = slotsOrdered.ToArray();
        objectsArray = new string[6];
        objectsQuantity = new int[6];
        for(int i = 0; i < objectsQuantity.Length; i++){
            objectsQuantity[i] = 0;
        }
        slotContainer = GameObject.FindGameObjectWithTag("SlotContainer");
    }

    public void LoadData(GameData data) {
        this.objectsQuantity = data.objectsQuantity;
        this.objectsArray = data.objectsArray;
        this.isSelected = data.isSelected;
        RefreshInventory();
    }

    public void SaveData(GameData data) {
        data.objectsQuantity = this.objectsQuantity;
        data.objectsArray = this.objectsArray; 
        data.isSelected = this.isSelected;
    }

    private IEnumerator ShowInventory() {
        slotContainer.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        slotContainer.SetActive(false);
    }

    public void RefreshInventory() {
        int i = 0;
        while (i < objectsQuantity.Length) {
            if (objectsQuantity[i] > 0) {
                name = objectsArray[i];
                int slotsNumber = slots[i].transform.childCount;
                if (slotsNumber <= 1 || name == "FireBall Button" ) {
                    switch (name) {
                        case "Health Button":
                            Instantiate(ItemAssets.Instance.healthPotion, slots[i].transform, false);
                            break;
                        case "SuperHealth Button":
                            Instantiate(ItemAssets.Instance.superHealthPotion, slots[i].transform, false);
                            break;
                        case "MegaHealth Button":
                            Instantiate(ItemAssets.Instance.megaHealthPotion, slots[i].transform, false);
                            break;
                        case "Decoy Button":
                            Instantiate(ItemAssets.Instance.decoy, slots[i].transform, false);
                            break;
                        case "WaterBall Button":
                            Instantiate(ItemAssets.Instance.waterBall, slots[i].transform, false);
                            break;
                        case "FireBall Button":
                            Debug.Log("Caracoli");
                            Instantiate(ItemAssets.Instance.fireBall, slots[i].transform, false);
                            break;
                        case "Sword Button":
                            Instantiate(ItemAssets.Instance.sword, slots[i].transform, false);
                            break;
                        default:
                            break;
                    }
                }
                TextMeshProUGUI text = slots[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                if (objectsQuantity[i] > 1) {
                    text.SetText(objectsQuantity[i].ToString());
                } else {
                    text.SetText("");
                }
            }
            i++;
        }
    }

    void GetMousePos(){ 
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotation = mousePos - (Vector2)transform.GetChild(0).transform.position;
        transform.GetChild(0).transform.right = -rotation;
    }
    Vector2 PointPosition(float time){
       return (Vector2)arrowTransform.position + (rotation.normalized * launchForce * time) + 0.5f * Physics2D.gravity * (time * time);
    }
    private void SpawnArrow(){
        Debug.Log("Flecha paso 2");
        GameObject firedArrow = Instantiate(arrow, arrowTransform.position, Quaternion.identity);
        reloaded = false;              
    }

    private void SpawnFireBall() {
        Debug.Log("Flecha paso 2");
        GameObject firedArrow = Instantiate(fireball, arrowTransform.position, Quaternion.identity);
        reloaded = false;
    }

    private void ThrowDecoy(){
        thrownDecoy = Instantiate(decoy, arrowTransform.position, Quaternion.identity);
        thrownDecoy.GetComponent<Rigidbody2D>().velocity =  -transform.GetChild(0).transform.right * launchForce;
        reloaded = false;
    }
    private IEnumerator Reload(){
        yield return new WaitForSeconds(0.6f);
        reloaded = true;
    }

    private IEnumerator DecoyDisapear() {
        yield return new WaitForSeconds(3f);
        Destroy(thrownDecoy);
        reloaded = true;
    }

    private IEnumerator UsingObject(){
        yield return new WaitForSeconds(0.5f);
        canUseObject = true;
    }
}
