using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] string fileName;
    GameData gameData;
    List<IDataPersistence> dataPersistenceObjects;
    FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }
        instance = this;
        SceneManager.activeSceneChanged += OnSceneWasSwitched;
    }

    void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    void OnSceneWasSwitched(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.Scene currentScene)
    {
        if (currentScene.name != "Main Menu")
        {
            dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }
    }

    public void NewGame()
    {
        gameData = new GameData();
        gameData.ResetAll();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        // Load saved data from file
        if (gameData == null)
        {
            NewGame();
            SaveGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        // Update data in scripts;
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        // Save data to file;
        dataHandler.Save(gameData);
    }

    void OnApplicationQuit()
    {
        SaveGame();
        SceneManager.activeSceneChanged -= OnSceneWasSwitched;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public GameData GetGameManager()
    {
        return gameData;
    }

    public bool SaveExists()
    {
        return dataHandler.SaveExists();
    }
}