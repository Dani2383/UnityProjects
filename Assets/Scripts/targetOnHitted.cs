using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetOnHitted : MonoBehaviour
{

    public bool readyToSpawn = true;
    public spawnTargets targetManager;
    private float maxTimeEnabled;
    private bool hitted;
    [SerializeField] private Material oldMaterial;
    [SerializeField] private Material hittedMaterial;
    private MeshRenderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();
        m_renderer.material = oldMaterial;
        hitted = false;
    }

    private void OnEnable() {
        m_renderer = GetComponent<MeshRenderer>();
        maxTimeEnabled = targetManager.maxTimeEnabled;
        m_renderer.material = oldMaterial;
        hitted = false;
        StartCoroutine("TimeDisable");
    }
    // Update is called once per frame

    private IEnumerator TimeDisable() {
        yield return new WaitForSeconds(maxTimeEnabled / 10000f);
        if (!hitted) {
            targetManager.finishCounter--;
            if (targetManager.finishCounter <= 0) targetManager.FinishGame();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator HittedDisable() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    public void OnHitted() {
        if (!hitted) {
            hitted = true;
            targetManager.finishCounter = 5;
            targetManager.totalPoints += 1;
            m_renderer.material = hittedMaterial;
            StartCoroutine("HittedDisable");
        }



    }
}
