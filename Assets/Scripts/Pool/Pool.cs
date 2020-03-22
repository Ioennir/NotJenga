using System;
using UnityEngine;

public class Pool : MonoBehaviour
{
	#region Private Variables
	
	private uint _activeGameObjects = 0;
	private GameObject[] _pool;
	
	// Keep track of the next gameObject
	private uint _currentGameObject = 0;
	
	
	[Header("Configuration")]
	
	[SerializeField] private GameObject prefab;
	[SerializeField] private uint maxSize = 500;
	
	[Header("Testing and logs")]
	[SerializeField] private bool printErrorLogs = false;
	
	#endregion

	#region Public Variables

	#endregion

	#region Properties
	
	/// <summary>
	/// Current maxSize
	/// </summary>
	public uint MaxSize => maxSize;
	
	/// <summary>
	/// The current number of active gameObjects in the scene of this pool
	/// </summary>
	public uint ActiveGameObjects => _activeGameObjects;
	
	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		_pool = new GameObject[maxSize];
		for (int i = 0; i < maxSize; ++i)
		{
			_pool[i] = Instantiate(prefab);
			_pool[i].SetActive(false);
		}
	}

	#endregion

    #region Public Methods

    /// <summary>
    /// Instantiate a GameObject. If the pool is full of active objects the pool will resize.
    /// </summary>
    /// <returns></returns>
    public GameObject Instantiate()
    {
	    // TODO: (GABI) Maybe give the option to destroy an object?
	    // Resize
	    if (_activeGameObjects >= maxSize)
	    {
		    Resize();
	    }
	    // Advance if necessary _currentGameObject index
	    Next();
	    _activeGameObjects++;
	    _pool[_currentGameObject].SetActive(true);
	    return _pool[_currentGameObject];
    }

    /// <summary>
    /// Destroys the specified target
    /// Inside, it will just deactivate it if it finds it
    /// </summary>
    /// <param name="target"></param>
    /// <returns>if the operation was successful</returns>
    public bool Destroy(GameObject target)
    {
	    if (_activeGameObjects == 0)
	    {
		    DebugError("Empty pool, unable to Destroy");
		    return false;
	    }
	    int i;
	    // Try to find the target
	    for (i = 0; i < maxSize && _pool[i] != target; ++i) {};
		if (i == maxSize)
	    {
		    DebugError($"{target} not found in the pool!");
		    return false;
	    }
		if (_pool[i].activeSelf == false)
		{
			DebugError("Target is already deactivated");
			return false;
		}
	    _pool[i].SetActive(false);
	    _activeGameObjects--;
	    return true;
    }
    
    #endregion

    #region Private Methods

    /// <summary>
    /// Resizes the pool size by 2
    /// </summary>
    private void Resize()
    {
	    uint newSize = maxSize * 2;
	    Array.Resize(ref _pool, (int) newSize);
	    for (uint i = maxSize; i < newSize; ++i)
	    {
		    _pool[i] = Instantiate(prefab);
		    _pool[i].SetActive(false);
	    }
	    maxSize = newSize;
    }

    /// <summary>
    /// Advance _currentGameObject to the next one that is available
    /// </summary>
    private void Next()
    {
	    for (;
		    _pool[_currentGameObject].activeSelf; 
		    _currentGameObject = _currentGameObject + 1 % maxSize
		) {}
    }

    /// <summary>
    /// Debug errors if printErrorLogs is set to true
    /// </summary>
    /// <param name="toPrint"></param>
    private void DebugError(object toPrint)
    {
	    if (!printErrorLogs) return;
	    Debug.LogError(toPrint);
    }
    
    #endregion
}
