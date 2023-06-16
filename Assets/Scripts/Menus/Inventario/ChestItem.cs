using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour {

    private ChestTrigger trigger;

    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }

    private void Awake() {
       trigger = gameObject.GetComponent<ChestTrigger>();
    }

    public void LoadData(GameData data) {
        data.chestCollected.TryGetValue(id, out trigger.collected);
        if (trigger.collected) {
            trigger.visualCue.SetActive(false);
        }
    }

    public void SaveData(GameData data) {

        if (data.chestCollected.ContainsKey(id)) {
            data.chestCollected.Remove(id);
        }
        data.chestCollected.Add(id, trigger.collected);
    }
}
