using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnTargets : MonoBehaviour
{
    public float maxTimeEnabled = 20000f;
    public float totalPoints = 0f;
    [SerializeField] private bool spawnTarget;
    private int targetQuantity;
    public GameObject[] targets;
    public int finishCounter = 5;
    public GameObject startGameUI;
    public Text punctuationText;
    [SerializeField] private AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        targetQuantity = gameObject.transform.childCount;
        spawnTarget = true;
        totalPoints = 0f;
        finishCounter = 5;
        Debug.LogError("Vas a funcionar o que lo que");
    }

    private void OnEnable() {
        maxTimeEnabled = 20000f;
        spawnTarget = true;
        totalPoints = 0f;
        finishCounter = 5;
    }

    // Update is called once per frame
    void Update()
    {

        if(maxTimeEnabled > 5000f) maxTimeEnabled--;
        if (spawnTarget) {
            Debug.LogError("Entro en la corrutina");
            StartCoroutine("SpawnTarget");
        }
    }

    private IEnumerator SpawnTarget() {
        Debug.LogError("entramos en corrutina");
        spawnTarget = false;
        bool spawned = false;
        while (!spawned) {
            Debug.LogError("Estamos intentando spawnear");
            int targetIndex = Random.Range(0, targetQuantity);
            Debug.LogError("Vamos a spawnear el " + targetIndex);
            if (!targets[targetIndex].activeSelf) {
                Debug.LogError("Como no esta activo, lo activo");
                targets[targetIndex].SetActive(true);
                spawned = true;
            }
        }
        yield return new WaitForSeconds(maxTimeEnabled / 10000f);
        spawnTarget = true;
    }

    public void FinishGame() {
        punctuationText.text = "Puntuación anterior: " + totalPoints;
        music.Stop();
        startGameUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
