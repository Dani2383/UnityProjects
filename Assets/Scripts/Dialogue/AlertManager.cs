using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    private bool dialogueIsPlaying;

    private static AlertManager instance;

    private void Awake() {

        if (instance != null) {
            Debug.LogWarning("Existen más de un DialogueManager");
        }
        instance = this;
    }

    public static AlertManager GetInstance() {
        return instance;
    }

    private void Start() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update() {
        if (!dialogueIsPlaying) {
            return;
        }
    }

    public void EnterAlertMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        dialogueText.text = currentStory.Continue();
    }

    public void ExitAlertMode() {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

}
