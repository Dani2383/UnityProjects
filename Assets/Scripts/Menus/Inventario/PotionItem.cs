using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : MonoBehaviour, IDataPersistence
{
    public bool collected = false;

    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data) {
        data.objectCollected.TryGetValue(id, out collected);
        if (collected) {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data) {

        if (data.objectCollected.ContainsKey(id)) {
            data.objectCollected.Remove(id);
        }
        data.objectCollected.Add(id, collected);
    }

    
}
