using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    public static InventorySystem current;
    [SerializeField]
    public Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    [SerializeField]
    public List<InventoryItem> inventory;

    public List<InventoryItem> inventoryToLoad;

    public GameObject inventoryPanel;

    [SerializeField] private InventoryItemData pizzaItemData;
    [SerializeField] private InventoryItemData beastItemData;
    

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        
        //inventoryLoad();
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value)){
            return value;
        }
        return null;
    }

    public void Add(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value)){
            value.AddToStack();
            inventoryPanel.GetComponent<Panel>().UpdateInventory();
        }
        else{
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
            inventoryPanel.GetComponent<Panel>().UpdateInventory();
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value)){
            value.RemoveFromStack();

            if(value.stackSize == 0){
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }

            inventoryPanel.GetComponent<Panel>().UpdateInventory();
        }
    }

    public void inventoryLoad(){
        inventoryToLoad = GameObject.Find("Persistent Inventory").GetComponent<PersistentData>().loadData();
        print("INVENTORY TO LOAD CREATED: " + inventoryToLoad);

        for(int i = 0; i< inventoryToLoad.Count; i++){
            inventory.Add(inventoryToLoad[i]);
        }

        inventoryPanel.GetComponent<Panel>().UpdateInventory();
    }

    public bool pizza(){
        bool pizza = false;
        int pizzaPos = -1;
        
        for(int i = 0; i < inventoryToLoad.Count; i++){
            if(inventoryToLoad[i].data == pizzaItemData ){
                pizza = true;
                pizzaPos = i;
            }
        }

        if(pizza && (pizzaPos != -1)){
  
            if(inventoryToLoad[pizzaPos].stackSize == 1){
                inventoryToLoad.RemoveAt(pizzaPos);
                for(int i = 0; i < inventory.Count; i++){
                    if(inventory[i].data == pizzaItemData){
                        inventory.RemoveAt(i);
                    }
                }
            } else{
                inventoryToLoad[pizzaPos].RemoveFromStack();
            }

            inventoryPanel.GetComponent<Panel>().UpdateInventory();
        }
        
        return pizza;
    }

    public bool beast(){
        bool beast = false;
        int beastPos = -1;
        
        for(int i = 0; i < inventoryToLoad.Count; i++){
            if(inventoryToLoad[i].data == beastItemData ){
                print("encontrado");
                beast = true;
                beastPos = i;
            }
        }

        if(beast && (beastPos != -1)){

            if(inventoryToLoad[beastPos].stackSize == 1){
                inventoryToLoad.RemoveAt(beastPos);
                for(int i = 0; i < inventory.Count; i++){
                    if(inventory[i].data == beastItemData){
                        inventory.RemoveAt(i);
                    }
                }
            } else{
                inventoryToLoad[beastPos].RemoveFromStack();
            }

            inventoryPanel.GetComponent<Panel>().UpdateInventory();
        }
        
        return beast;
    }

}


[Serializable]
public class InventoryItem
{
    public InventoryItemData data;
    public int stackSize;

    public InventoryItem(InventoryItemData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack(){
        stackSize++;
    }

    public void RemoveFromStack(){
        stackSize--;
    }
}