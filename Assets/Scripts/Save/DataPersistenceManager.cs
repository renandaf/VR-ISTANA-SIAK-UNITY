using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false; //kalo mau testing

    [Header("File Storage Config")]
    [SerializeField] private string fileName;


    private GameData _gameData;
    private List<IDataPersistence> _dataPersistencesObjects;
    private FileDataHandler _dataHandler;
    private string _selectedProfileId = "test";
    public static DataPersistenceManager instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more data persistance instance");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _dataPersistencesObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        _selectedProfileId = newProfileId;
    }

    public void NewGame()
    {
        this._gameData = new GameData();
        _selectedProfileId = System.Guid.NewGuid().ToString();
        _dataHandler.Save(_gameData, _selectedProfileId);
    } 

    public void LoadGame()
    {
        //load data dari file
        _gameData = _dataHandler.Load(_selectedProfileId);
        //cuma untuk testing di dalam scene lain
        if (this._gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        if (this._gameData == null)
        {
            return;
        }
       foreach(IDataPersistence dataPersintenceObj in _dataPersistencesObjects)
        {
            dataPersintenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        if (this._gameData == null)
        {
            return;
        }
        foreach (IDataPersistence dataPersintenceObj in _dataPersistencesObjects)
        {
            dataPersintenceObj.SaveData(_gameData);
        }
        //save data ke file
        _dataHandler.Save(_gameData, _selectedProfileId);   
    }

    public void DeleteSave(string profileId)
    {
        _dataHandler.Delete(profileId);
    }

    public bool HasGameData()
    {
        return _gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return _dataHandler.LoadAllProfile();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistencesObjects);
    }
}
