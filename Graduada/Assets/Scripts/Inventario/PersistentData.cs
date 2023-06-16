using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    [SerializeField]
    public Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    [SerializeField]
    public List<InventoryItem> inventory;

    private ItemObject itemobject;

    private float conservasUber;
    private float conservasMuerte;

    [SerializeField] private InventoryItemData pizzaItemData;
    [SerializeField] private InventoryItemData beastItemData;


    public bool dialogPlayed;


    void Awake(){

        conservasUber = 0.7f;
        conservasMuerte = 0.2f;

        int numOfThis = FindObjectsOfType<PersistentData>().Length;

         if (numOfThis != 1)
        {
            Destroy(this.gameObject);
        }

        else
        {
        DontDestroyOnLoad(gameObject);
        }

        dialogPlayed = false;
    }

    public void returnWithUber(){
        print("Ha vuelto con Uber");

        returnHome(conservasUber);
    }

    public void returnWithDeath(){
        print("Ha vuelto muerto");

        returnHome(conservasMuerte);
    }

    public void returnHome(float conservas){
        print("Ha vuelto con el Uber");
        List<InventoryItem> totalInventory = GameObject.FindGameObjectWithTag("inventario").GetComponent<InventorySystem>().inventory;
        List<InventoryItem> inventoryObtained = new List<InventoryItem>();
        
        for(int i = 0; i < totalInventory.Count; i++){
            if(totalInventory[i].data != pizzaItemData && totalInventory[i].data != beastItemData){
                inventoryObtained.Add(totalInventory[i]);
            }
        }

        bool exist = false;

        int totalItems = 0;

        for(int i = 0; i < inventoryObtained.Count; i++){
            totalItems = totalItems + inventoryObtained[i].stackSize;
        }

        int itemsToDelete = totalItems - Mathf.RoundToInt((float) totalItems* conservas);
        
        print("Hay que borrar: " + itemsToDelete + " de " + totalItems);

        //-------------------------------------------------------------------------------------------------------------------

        for(int i = 0; i< itemsToDelete; i++){
            int itemStack =  Random.Range(0,inventoryObtained.Count);
            inventoryObtained[itemStack].RemoveFromStack();
            if(inventoryObtained[itemStack].stackSize == 0) inventoryObtained.RemoveAt(itemStack);
        }

        //-------------------------------------------------------------------------------------------------------------------

        List<int> stacksBorrar = new List<int>();

        for(int stackNumber = 0; stackNumber < inventoryObtained.Count; stackNumber++){

            print("STACK OBTENIDO: " + stackNumber);

            exist = false;

            for(int stackNumberP = 0; stackNumberP < inventory.Count; stackNumberP++){

                print("STACK OBTENIDO: " + stackNumber +" ES: " + inventoryObtained[stackNumber].data  +" Y STACK DEL INVENTARIO: " + stackNumberP + " ES: " + inventory[stackNumberP].data);

                if(inventoryObtained[stackNumber].data == inventory[stackNumberP].data){
                    print("SON EL MISMO");
                    exist = true;
                    for(int added = 0; added < inventoryObtained[stackNumber].stackSize; added++ ){
                        inventory[stackNumberP].stackSize++;
                    }

                    stacksBorrar.Add(stackNumber);
                    //inventoryObtained.RemoveAt(stackNumber); 
                }

                if(exist) break;
            }

        }

        for(int i =0; i < stacksBorrar.Count; i++){
            inventoryObtained.RemoveAt(stacksBorrar[i]); 
        }

        print("DESPUES DE HACER LOS CALCULOS DE STACK, HAY QUE METER: " + inventoryObtained.Count);

        inventoryObtained.AddRange(GameObject.FindGameObjectWithTag("inventario").GetComponent<InventorySystem>().inventoryToLoad);

        for(int i = 0; i < inventoryObtained.Count; i ++) inventory.Add(inventoryObtained[i]);

    }


    public List<InventoryItem> loadData(){
        print("loading inventory");

        List<InventoryItem> inventoryToLoad = new List<InventoryItem>();

        for(int i = 0; i < inventory.Count; i++){
            bool rmv = false;

            if(inventory[i].data == pizzaItemData){
                int pizzaStack = inventory[i].stackSize;
                print("PIZZAAS " + pizzaStack);
                rmv = true;
            }else if(inventory[i].data == beastItemData){
                int beastStack = inventory[i].stackSize;
                print("BEAST " + beastStack);
                rmv = true;    
            }

            if(rmv){
                inventoryToLoad.Add(inventory[i]);
                inventory.RemoveAt(i); 
                i--;
            } 
        }

        return inventoryToLoad;
    }


    
    public void Add(InventoryItemData referenceData)
    {
        Debug.Log(referenceData);
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value)){
            value.AddToStack();
        }

        else{
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
            print("SE HA AÑADIDO");
        }
    }

    public void AddT(InventoryItemData referenceData){
        bool added = false;

        for(int i = 0; i < inventory.Count; i++){
            if(inventory[i].data == referenceData){
                added = true;
                inventory[i].AddToStack();
            }
        }

        if(!added){
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
        }
    }

    public InventoryItemData RemoveT(InventoryItemData referenceData){
        InventoryItemData removed = null;

        for(int i = 0; i < inventory.Count; i++){
            if(inventory[i].data == referenceData){
                inventory[i].RemoveFromStack();

                removed = referenceData;

                if(inventory[i].stackSize == 0){
                    inventory.RemoveAt(i);
                }

            }
        }

        return removed;
    }

    public void Remove(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem value)){
            value.RemoveFromStack();

            if(value.stackSize == 0){
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }
}
