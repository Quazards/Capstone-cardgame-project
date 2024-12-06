using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [HideInInspector] public GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
        private void Start()
        {
            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame()
        {
            this.gameData = new GameData();
        }

        public void LoadGame()
        {
            this.gameData = dataHandler.Load();

            if(this.gameData == null)
            {
                Debug.Log("No data was found. Initializing data to default");
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
            Debug.Log("Loaded Health = " + gameData.playerCurrentHealth + " Loaded Resolution = " + 
            gameData.CurrentResolution.width + " x " + gameData.CurrentResolution.height);

        }

        public void SaveGame()
        {
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }
            Debug.Log("Saved Health = " + gameData.playerCurrentHealth + " Saved Resolution = " + 
            gameData.CurrentResolution.width + " x " + gameData.CurrentResolution.height);


            dataHandler.Save(gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

    [System.Obsolete]
    private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }