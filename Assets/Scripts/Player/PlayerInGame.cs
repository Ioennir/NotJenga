using UnityEngine;

public class PlayerInGame : MonoBehaviour
{
	public enum State
	{
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

	private TurnController _turnController;
	
	/// <summary>
	/// The current state of this component
	///
	/// Never change state directly from here.
	/// </summary>
	private StateTurn currentState = new StateTurn();

	private ColocationBehaviour _colocationBehaviour;

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
	    _turnController = FindObjectOfType<TurnController>();
	    _tower = FindObjectOfType<Tower>();
	    currentState.player = _turnController.CurrentPlayer;
	    currentState.currentState = State.SelectingPiece;
    }

	private void Update()
    {
	    // Change the state if turn != this turn
        ChangeStateIf(_turnController.CurrentPlayer != currentState.player, State.SelectingPiece, false);
		
        // Test 
        ChangeStateIf(_tower.towerAlreadyBuilt, State.PlacingPiece, false);
        switch (currentState.currentState)
        {
	        case State.SelectingPiece:
	        {
		        if (!_tower.towerAlreadyBuilt) return;
		        // Selecting piece behaviour.
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
    /// Won't do NOTHING if the state is the same as before. (Prevention of constantly calling this methon on Update)
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
