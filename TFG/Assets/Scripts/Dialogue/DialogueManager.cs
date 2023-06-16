using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]

    //[SerializeField] private GameObject dialoguePanel;

    private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    public bool DialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private void Awake() {

        if(instance != null) {
            Debug.LogWarning("Existen más de un DialogueManager");
        }
        instance = this;
        dialoguePanel = GameObject.FindGameObjectWithTag("DialoguePanel");
    }

    public static DialogueManager GetInstance() {
        return instance;
    }

    private void Start() { 
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update() {
        if (!DialogueIsPlaying) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            ContinueHistory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueHistory();
    }

    private IEnumerator ExitDialogueMode() {

        yield return new WaitForSeconds(0.2f);
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueHistory() {
        if (currentStory.canContinue) {
            dialogueText.text = currentStory.Continue();
        } else {
            StartCoroutine(ExitDialogueMode());
        }
    }
}
