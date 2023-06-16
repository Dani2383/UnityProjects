using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;

    private void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Player") {
            for (int i = 0; i < inventory.slots.Length; i++) {
                if (inventory.objectsArray[i] == itemButton.name) {
                    inventory.objectsQuantity[i] += 1;
                    Destroy(gameObject);
                    inventory.RefreshInventory();
                    break;
                }
                if(itemButton.name == "FireBall Button" && inventory.objectsArray[i] == "WaterBall Button") {
                    inventory.objectsArray[i] = "FireBall Button";
                    Destroy(inventory.slots[i].transform.GetChild(1).gameObject);
                    Destroy(gameObject);
                    inventory.RefreshInventory();
                    break;
                }
            }
            
        }
    }

    private IEnumerator Refresh(){
        yield return new WaitForSeconds(1f);
        inventory.RefreshInventory();
    }
}