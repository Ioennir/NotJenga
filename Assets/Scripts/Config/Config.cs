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

	private bool _savingData = false;

	private SaveSystem.Informer<SavedGamesData> _loadingSavedGamesData;

	private static Config _instance;
	#endregion

	#region Public Variables
	
	
	#endregion

	#region Properties

	public static bool SavingData => _instance._savingData || _instance._loadingSavedGamesData != null;
	
	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		if (!_instance)
		{
			_instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		if (!SaveSystem.Exists("game_data.json"))
		{
			// This should be almost instant so it will not be in another thread.
			SaveSystem.Save("game_data.json", new SavedGamesData());
			Debug.Log("lol");
			Debug.Log($"{SaveSystem.PersistentDataPath}");
		}
	}

	private void Start()
    {
	    
    }

	private void Update()
    {
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
	    if (_instance._savingData || _instance._loadingSavedGamesData != null)
	    {
		    Debug.Log("CANT SAVE");
		    return false;
	    }
	    _instance._savingData = true;
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
	    _instance._loadingSavedGamesData = SaveSystem.LoadOnAnotherThread<SavedGamesData>("game_data.json");
	    
	    return true;
    }

    /// <summary>
    /// Keeps going with the saving of the jenga game
    /// from where we left off on SaveJengaGame()
    /// </summary>
    private void KeepSavingJengaGame()
    {
	    if (!_savingData && _loadingSavedGamesData != null)
	    {
		    if (_loadingSavedGamesData.loaded || _loadingSavedGamesData.error)
		    {
			    Debug.Log("RESET");
			    _loadingSavedGamesData = null;
		    }
	    }
	    if (!_savingData || _loadingSavedGamesData == null) 
	    {
		    return;
	    }
	    if (_loadingSavedGamesData.error)
	    {
		    _savingData = false;
		    return;
	    }
	    if (!_loadingSavedGamesData.loaded)
	    {
		    return;
	    }
	    SavedGamesData dataGame = _loadingSavedGamesData.data;
	    int idWeAreSearchingFor = _data.id;
	    int idx = -1;
	    for (int i = 0; i < dataGame.games.Length; ++i)
	    {
		    if (dataGame.games[i].id != idWeAreSearchingFor) continue;
		    idx = i;
		    break;
	    }
	    if (idx == -1)
	    {
		    int oldSize = dataGame.games.Length;
			Array.Resize(ref dataGame.games, oldSize + 1);
			dataGame.games[oldSize] = _data;
	    }
	    else
	    {
		    dataGame.games[idx].jengas = _data.jengas;    
	    }
	    _loadingSavedGamesData = SaveSystem.SaveOnAnotherThread("game_data.json", dataGame);
	    _savingData = false;
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
