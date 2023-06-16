using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuCarga : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    public Sprite[] frames;
    private GameObject player;

    [SerializeField] int fps;

    public bool active;
    
    void Start()
    {
        player = GameObject.Find("Protagonista");
        player.GetComponent<IsometricPlayerMovement>().speed = 12;
        active = true;
        inventory.GetComponent<InventorySystem>().inventoryLoad();
        //StartCoroutine(load());
    }

    
    void Update()
    {
        if(active){
            int index = (int)(Time.time * fps) % frames.Length;
            GetComponent<Image>().sprite= frames[index];
        }
    }
    
    private IEnumerator load(){
        yield return new WaitForSeconds(11);
        //inventory.GetComponent<InventorySystem>().inventoryLoad();
        //gameObject.SetActive(false);
        //active = false;
        //player.GetComponent<IsometricPlayerMovement>().speed = 12;
    }
}

