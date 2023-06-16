using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]

    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private GameObject continueIcon;

    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private TextMeshProUGUI displayNameText;
    private static DialogueManager instance;

    private Story currentStory;

    private Coroutine displayLineCoroutine;

    public bool dialogueIsPlaying {get; private set;}

    private const string SPEAKER_TAG = "speaker";

    private bool canContinueToNextLine = false;

    private void Awake() {

        if(instance != null){

            Debug.LogWarning("ERROR");
        }
        instance = this;
    }

    private void Update(){

        if(!dialogueIsPlaying){
            return;
        }

        if ( canContinueToNextLine && Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
            StartCoroutine(Esperar());
        }
    }
    public static DialogueManager GetInstance(){
        return instance;
    }

    private void Start(){
      dialogueIsPlaying = false;
      dialoguePanel.SetActive(false); 
    }

    public void EnterDialogueMode(TextAsset inkJSON){

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
        
    }

    private void ExitDialogueMode(){
        
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void ContinueStory(){

        if(currentStory.canContinue){

            if(displayLineCoroutine != null){
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            HandleTags(currentStory.currentTags);
        }
        else{
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags){
        foreach(string tag in currentTags){

            string [] splitTag = tag.Split(':');
            if(splitTag.Length !=2){
                Debug.LogError("Error de TAG");
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKey){
                case SPEAKER_TAG:
                displayNameText.text = tagValue;
                break;
                default:
                Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                break;
            }
        }
    }

    IEnumerator Esperar(){
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator DisplayLine(string line){
        dialogueText.text = "";

        continueIcon.SetActive(false);

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray()){

            if(Input.GetKey(KeyCode.Return)){
                dialogueText.text = line;
                break;
            }

            if(letter == '<' || isAddingRichTextTag){

                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if(letter == '>'){
                    isAddingRichTextTag = false;
                }

            }
            else {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            
        }

        canContinueToNextLine = true;
        continueIcon.SetActive(true);

    }
}
