using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Encontrar los objetos persistentes
using System.Linq;
using UnityEngine.SceneManagement;

//Clase singleton
public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("File Storage Config")]

    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption;

    private FileDataHandler dataHandler;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake() {
        if(instance != null) {
            Debug.Log("Se ha encontrado otra instancia de DataPersistenceManager en la escena");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    /*private void OnSceneUnloaded(Scene scene) {
        SaveGame();
    }*/

    public void Start() {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    public void NewGame() {
        this.gameData = new GameData();
    }

    public void LoadGame() {

        this.gameData = dataHandler.Load();

        if (this.gameData == null && initializeDataIfNull) {
            NewGame();
        }

        if (this.gameData == null) {
            Debug.Log("No se encontraron datos. Inicializando datos");
            return;
        }

  
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.LoadData(gameData);
        }
        
    }

    public void SaveGame() {

        if (this.gameData == null) {
            Debug.Log("No se encontraron datos. Se necesita iniciar un nuevo juego");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SaveData(gameData);
        }

        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() { 
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    /*private void OnApplicationQuit() {
        SaveGame();
    }*/

    public int GetSceneIndex() {
        int res = gameData.sceneIndex;
        return res;
    }

    public bool HasGameData() {
        return gameData != null;
    }
}
