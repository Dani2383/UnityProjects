using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {

    [SerializeField] private List<TKey> keys = new();

    [SerializeField] private List<TValue> values = new();

    //almacena el diccionario en formato lista
    public void OnBeforeSerialize() {
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey,TValue> pair in this) {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    //Carga el diccionario desde las listas
    public void OnAfterDeserialize() {

        this.Clear();

        if(keys.Count != values.Count) {
            Debug.LogError("Se ha producido un error al combinar los valores" +
                "de las claves del diccionario");
        }

        for (int i=0; i < keys.Count; i++) {
            this.Add(keys[i], values[i]);
        }
    }
}
