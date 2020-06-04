using System;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Configuration related class
///
/// Will hold jenga information from other scenes, like the data to load etc.
///
/// You can also save the current scene data here.
/// </summary>
public class Config : MonoBehaviour
{
	#region Private Variables

	private static JengaData _data;

	private bool _loadingData = false;

	private SaveSystem.Informer<SavedGamesData> _loadingSavedGamesData;

	private static Config _instance;
	
	#endregion

	#region Public Variables
	
	
	#endregion

	#region Properties

	public static bool SavingData => _instance._loadingData || _instance._loadingSavedGamesData != null;
	
	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (_instance)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
		if (SaveSystem.Exists("game_data.json")) return;
		// This should be almost instant so it will not be in another thread.
		SaveSystem.Save("game_data.json", new SavedGamesData());
		Debug.Log($"Created game_data.json because none was found at {SaveSystem.PersistentDataPath}");
	}

	private void Update()
    {
	    // Constantly checks if there is a game loading.
	    KeepSavingJengaGame();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Tries to get the config if there is some. If you wanna load config call LoadJengaConfig.
    ///
    /// For bug prevention, LoadConfig sets to null the configuration so you have to explicitly load a config again.
    ///
    /// </summary>
    /// <returns>If there is no config, it will return null.</returns>
    public static JengaData GetJengaConfig()
    {
	    JengaData data = _data;
	    _data = null;
	    return data;
    }
    
    /// <summary>
    /// Loads jenga data so when we are in the gameplay scene
    /// we can have custom setups. if there is no config loaded it will spawn a standard tower.
    /// </summary>
    /// <param name="data"></param>
    public static void LoadJengaConfig(JengaData data)
    {
	    _data = data;
    }

    
    /// <summary>
    /// Will start saving the jenga game in another thread.
    /// </summary>
    /// <returns></returns>
    public static bool SaveJengaGame(GameObject[] pieces)
    {
	    if (_instance._loadingData || _instance._loadingSavedGamesData != null)
	    {
		    Debug.Log("CAN'T SAVE");
		    return false;
	    }
	    _instance._loadingData = true;
	    if (_data == null)
	    {
		    _data = new JengaData { id = _instance.GetInstanceID() };
	    }
	    _data.jengas = new JengaPieceData[pieces.Length];
	    
	    for (int i = 0; i < pieces.Length; ++i)
	    {
		    _data.jengas[i] = new JengaPieceData
		    {
			    position = pieces[i].transform.position,
			    rotation = pieces[i].transform.rotation.eulerAngles,
			    scale = pieces[i].transform.localScale
		    };
	    }
	    // We will keep with more login in KeepSavingJengaGame when this is finished loading
	    _instance._loadingSavedGamesData = SaveSystem.LoadOnAnotherThread<SavedGamesData>("game_data.json");
	    return true;
    }

    /// <summary>
    /// Keeps going with the saving of the jenga game
    /// from where we left off on SaveJengaGame()
    /// </summary>
    private void KeepSavingJengaGame()
    {
	    bool checkIfTheGameFinishedSaving = !_loadingData && _loadingSavedGamesData != null;
	    if (checkIfTheGameFinishedSaving)
	    {
		    if (_loadingSavedGamesData.loaded || _loadingSavedGamesData.error)
		    {
			    Debug.Log("Finished saving data.");
			    _loadingSavedGamesData = null;
		    }
	    }
	    bool checkIfThereIsDataLoadingOrSaving = !_loadingData || _loadingSavedGamesData == null;
	    if (checkIfThereIsDataLoadingOrSaving) 
	    {
		    return;
	    }
	    if (_loadingSavedGamesData.error)
	    {
		    _loadingData = false;
		    _loadingSavedGamesData = null;
		    return;
	    }
	    if (!_loadingSavedGamesData.loaded)
	    {
		    return;
	    }
	    // This means that the GameData finished loading, so we must modify it
	    // and save it with our new data
	    SavedGamesData dataGame = _loadingSavedGamesData.data;
	    int idWeAreSearchingFor = _data.id;
	    int idx = -1;
	    // Find if our game exists.
	    for (int i = 0; i < dataGame.games.Length; ++i)
	    {
		    if (dataGame.games[i].id != idWeAreSearchingFor) continue;
		    idx = i;
		    break;
	    }
	    // If it does not exist add it into the game
	    if (idx == -1)
	    {
		    int oldSize = dataGame.games.Length;
			Array.Resize(ref dataGame.games, oldSize + 1);
			dataGame.games[oldSize] = _data;
	    }
	    // It exists
	    else
	    {
		    dataGame.games[idx].jengas = _data.jengas;    
	    }
	    // Save again and get the informer so we can check on checkIfTheGameFinishedSaving
	    _loadingSavedGamesData = SaveSystem.SaveOnAnotherThread("game_data.json", dataGame);
	    _loadingData = false;
    }

    public static SaveSystem.Informer<SavedGamesData> LoadGamesData()
    {
	    return SaveSystem.LoadOnAnotherThread<SavedGamesData>("game_data.json");;
    }

    public static SaveSystem.Informer<SavedGamesData> SaveGameData(SavedGamesData data)
    {
	    return SaveSystem.SaveOnAnotherThread("game_data.json", data);
    }
    

    #endregion

    #region Private Methods

    #endregion
}
