using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
   public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) {
            Debug.LogError("Se ha encontrado otra instancia de DataPersistenceManager en la escena");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public GameObject decoy;
    public GameObject sword;
    public GameObject waterBall;
    public GameObject fireBall;
    public GameObject healthPotion;
    public GameObject superHealthPotion;
    public GameObject megaHealthPotion;

    public GameObject healthPotionLoot;
    public GameObject superHealthPotionLoot;
    public GameObject megaHealthPotionLoot;

    public void DropRandomItem(Vector3 position) {
        Debug.Log("Se murió");
        if (Random.Range(0.0f, 100.0f) <= 70.0f) {
            Debug.Log("Entro a mirar");
            float probability = Random.Range(0.0f, 100.0f);
            Debug.Log(probability);
            if (probability <= 20.0f) {
                Instantiate(megaHealthPotionLoot, position, Quaternion.identity);
            }
            else if(probability <= 50.0f) {
                Instantiate(superHealthPotionLoot, position, Quaternion.identity);
            } else {
                Instantiate(healthPotionLoot, position, Quaternion.identity);
            }
        }
    }
}
