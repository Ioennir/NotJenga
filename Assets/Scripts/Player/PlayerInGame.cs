using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SelectionBehaviour))]
[RequireComponent(typeof(ColocationBehaviour))]
public class PlayerInGame : MonoBehaviour
{
	public enum State
	{
		WaitingForGameToStart,
		SelectingPiece,
		PullingPiece,
		PlacingPiece,
		WaitingForNextTurn,
		End,
	}

	public struct StateTurn
	{
		public int player;
		public State currentState;
	}

#region Private Variables


	
	/// <summary>
	/// The current state of this component
	///
	/// Never change state directly from here.
	/// </summary>
	private StateTurn currentState = new StateTurn();

	///  Behaviours
	
	private SelectionBehaviour _selectionBehaviour;

	private ColocationBehaviour _colocationBehaviour;

	private PullBehaviour _pullBehaviour;

	private SelectionScript _selectionWithMouse;
	

	/// Utils
	
	private TurnController _turnController;
	
	private CameraController _cameraController;

	private Tower _tower;

	private GameObject _currentJenga;

	private float _deltaUntilNextTurn = 0.0f;

	[SerializeField] private float waitingUntilColocation = 3.0f;

	private bool _end = false;

	private SaveSystem.Informer<SavedGamesData> _informer;
	
	#endregion

	#region Public Variables

	#endregion

	#region Properties

	/// <summary>
	/// Always use this Property to change state of this component or it will bug out
	/// </summary>
	public State StateCurrentTurn
	{
		set
		{
			if (currentState.currentState == State.PlacingPiece)
			{
				_colocationBehaviour.DisposeTurn();
			}
			currentState.currentState = value;
		}
		get => currentState.currentState;

	}
	#endregion

	#region MonoBehaviour

    private void Start()
    {
	    _cameraController = FindObjectOfType<CameraController>();
	    _colocationBehaviour = GetComponent<ColocationBehaviour>();
	    _pullBehaviour = GetComponent<PullBehaviour>();
	    _selectionWithMouse = GetComponent<SelectionScript>();
	    
	    _selectionBehaviour = GetComponent<SelectionBehaviour>();
	    _turnController = FindObjectOfType<TurnController>();
	    _tower = FindObjectOfType<Tower>();
	    currentState.player = _turnController.CurrentPlayer;
	    currentState.currentState = State.SelectingPiece;
    }
    
	private void Update()
    {
	    // Change the state if turn != this turn
        ChangeStateIf(_turnController.CurrentPlayer != currentState.player && StateCurrentTurn != State.WaitingForGameToStart, State.SelectingPiece, false);
        // Test 
        // ChangeStateIf(_tower.towerAlreadyBuilt, State.PlacingPiece, false);
        switch (currentState.currentState)
        {
	        case State.WaitingForGameToStart:
	        {
		        if (ChangeStateIf(_tower.towerAlreadyBuilt, State.SelectingPiece, false))
		        {
			        List<GameObject> top = _tower.GetTopPieces();
			        _cameraController.Target = top[0].gameObject.transform;
		        }
		        break;
	        }
	        
	        case State.SelectingPiece:
	        {
		        if (Input.GetKey(KeyCode.P) && !Config.SavingData)
		        {
			        Debug.Log("Starting save...");
			        Config.SaveJengaGame(_tower.Pieces);
		        }
		        if (!_tower.towerAlreadyBuilt) return;
		        GameObject r = _selectionBehaviour.Tick();
		        r = _selectionWithMouse.Tick(r);
		        // Selecting piece behaviour.
		        if (ChangeStateIf(r, State.PullingPiece, false))
		        {
			        _cameraController.Target = r.transform;
			        _selectionBehaviour.Dispose();
			        _currentJenga = r;
		        }
		        break;
	        }

	        case State.PullingPiece:
	        {
		        ChangeStateIf(_pullBehaviour.Tick(_currentJenga), State.WaitingForNextTurn, false);
		        break;
	        }

	        case State.WaitingForNextTurn:
	        {
		        if (_tower.PiecesOnTheFloor().Count > 0 || _tower.badPlaced.Count > 0 && !_tower.badPlaced.Contains(_currentJenga))
		        {
			        ChangeStateIf(true, State.End, false);
			        return;
		        }
		        _deltaUntilNextTurn += Time.deltaTime;
		        ChangeStateIf(
			        _deltaUntilNextTurn >= waitingUntilColocation && 
		            (_deltaUntilNextTurn = 0.01f) > -1f, 
			        State.PlacingPiece, 
			        false);
		        break;
	        }

	        case State.PlacingPiece:
	        {
		        ChangeStateIf(_colocationBehaviour.Tick(), State.SelectingPiece, true);
		        break;
	        }

	        case State.End:
	        {
		        if (_end && _informer != null && _informer.loaded)
		        {
			        Debug.Log("END");
			        return;
		        }
		        DestroyGameFromSave();
		        break;
	        }
        }
        
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Change the state if the conditions are met.
    ///
    /// Won't do NOTHING if the state is the same as before. (Prevention of constantly calling this method on Update)
    /// </summary>
    /// <param name="condition">If it should change the state</param>
    /// <param name="state">State to change to</param>
    /// <param name="changeTurn">If it should move to the next turn as well</param>
    /// <returns></returns>
    public bool ChangeStateIf(bool condition, State state, bool changeTurn)
    {
	    if (state == StateCurrentTurn) return false;
	    if (!condition) return false;
	    if (changeTurn)
	    {
		    _turnController.MoveNextTurn();
	    }

	    if (StateCurrentTurn == State.PullingPiece)
	    {
		    _pullBehaviour.Dispose();
	    }

	    if (changeTurn && StateCurrentTurn != State.PlacingPiece)
	    {
		    Debug.Log("lost");
	    }
	    
	    if (StateCurrentTurn == State.SelectingPiece)
		    _selectionWithMouse.DisposeTurn();
	    
	    currentState.player = _turnController.CurrentPlayer;
	    StateCurrentTurn = state;
	    return true;
    }
    
    
    
    #endregion

    #region Private Methods

    private void DestroyGameFromSave()
    {
	    if (_end) return;
	    if (Config.GetJengaConfig() == null)
	    {
		    _end = true;
		    return;
	    }
	    if (_informer == null)
	    {
		    _informer = Config.LoadGamesData();
		    
	    }
	    JengaData data = Config.GetJengaConfig();
	    if (_informer.error)
	    {
		    _end = true;
		    _informer.loaded = true;
		    return;
	    }
	    if (!_informer.loaded) return;
	    _informer.data.games = _informer.data.games.Where(game => game.id != data.id).ToArray();
	    _informer = Config.SaveGameData(_informer.data);
	    _end = true;
    }
    
    
    #endregion
}
