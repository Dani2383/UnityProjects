using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake() {
        playerInRange = false;
        
    }

    private void Update() {
        if (playerInRange) {  
            AlertManager.GetInstance().EnterAlertMode(inkJSON);
        } else {
            AlertManager.GetInstance().ExitAlertMode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Player")) {
            playerInRange = false;
        }
    }
}
