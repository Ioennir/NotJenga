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
		PlacingPiece
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

	/// Utils
	
	private TurnController _turnController;
	
	private CameraController _cameraController;

	private Tower _tower;

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
			        var top = _tower.GetTopPieces();
			        _cameraController.Target = top[0].gameObject.transform;
		        }
		        break;
	        }
	        
	        case State.SelectingPiece:
	        {
		        if (!_tower.towerAlreadyBuilt) return;
		        var r = _selectionBehaviour.Tick();
		        Debug.Log(r);
		        // Selecting piece behaviour.
		        if (ChangeStateIf(r, State.PullingPiece, false))
		        {
			        _cameraController.Target = r.transform;
		        }
		        break;
	        }

	        case State.PullingPiece:
	        {
		        break;
	        }

	        case State.PlacingPiece:
	        {
		        ChangeStateIf(_colocationBehaviour.Tick(), State.SelectingPiece, true);
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
	    currentState.player = _turnController.CurrentPlayer;
	    StateCurrentTurn = state;
	    return true;
    }
    
    
    
    #endregion

    #region Private Methods

    #endregion
}
