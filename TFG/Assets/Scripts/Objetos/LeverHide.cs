using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverHide : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] public GameObject visualCue;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [SerializeField] public Sprite leverUp;

    [SerializeField] public Sprite leverDown;

    [SerializeField] public GameObject fogOfWar;

    private bool playerInRange;

    public bool down;

    private void Awake() {
        playerInRange = false;
        visualCue.SetActive(false);
        down = false;
        fogOfWar.SetActive(true);
    }

    private void Update() {
        if (playerInRange) {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
                AudioManager.instance.PlaySound("Lever Sound");
                if (!down) {
                    gameObject.GetComponent<SpriteRenderer>().sprite = leverDown;
                    fogOfWar.SetActive(false);
                    down = true;
                }
                else{
                    gameObject.GetComponent<SpriteRenderer>().sprite = leverUp;
                    fogOfWar.SetActive(true);
                    down = false;
                }
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
