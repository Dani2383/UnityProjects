using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerEnterPiso : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [SerializeField] public GameObject persistent;

    private bool haIniciado;
    private void Update() {
     if(!haIniciado && !persistent.GetComponent<PersistentData>().dialogPlayed){
     DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
     }
     haIniciado = true;
     persistent.GetComponent<PersistentData>().dialogPlayed = true;
    }

    private void Awake() {
        haIniciado = false;
        persistent = GameObject.Find("Persistent Inventory");
    }
}
