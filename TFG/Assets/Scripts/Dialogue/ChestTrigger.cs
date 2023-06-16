using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] public GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Spawn Object")]
    [SerializeField] private GameObject spawnObject;

    [Header("Player")]
    [SerializeField] private GameObject player;

    private bool playerInRange;

    public bool collected;

    private void Awake() {
        playerInRange = false;
        visualCue.SetActive(false);
        collected = false;
    }

    private void Update() {
        if (playerInRange && !DialogueManager.GetInstance().DialogueIsPlaying && !collected) {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
                AudioManager.instance.PlaySound("Chest Open");
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                Instantiate(spawnObject, gameObject.transform.position, Quaternion.identity);
                collected = true;
            }
        } else {
            visualCue.SetActive(false);
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
