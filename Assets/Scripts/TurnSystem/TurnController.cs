using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    //NOTE(Ioennir): We should create objects that represent the different player along with their scores etc to update the UI etc
#region Public Variables
    public UIWrapper UI;
#endregion

#region Private Variables
    [SerializeField]
    private int _turnCounter = 0;
    [SerializeField]
    private int _playerCounter = 0;
    [SerializeField]
    private int _currentPlayer = 0;
    [SerializeField]
    private float _turnDuration = 30.0f;
    private bool _timedTurns = true;
    [SerializeField]
    private bool _gameStarted = false;
    [SerializeField]
    private float _currentTurnTimer = 0.0f;

#endregion

#region Properties
    public int CurrentTurn
    {
        get { return _turnCounter; }
        set { _turnCounter = value; }
    }

    public int CurrentPlayer
    {
        get { return _currentPlayer; }
        set { _currentPlayer = value; }
    }

    public float CurrentTurnTimer
    {
        get { return _currentTurnTimer; }
        set { _currentTurnTimer = value; }
    }

    /// <summary>
    /// Returns the number of player
    /// </summary>
    public int PlayerCounter => _playerCounter;
#endregion

#region MonoBehaviour

    void Start()
    {
        UI = FindObjectOfType<UIWrapper>();
        //IMPORTANT(Ioennir): This InitializeGameTurns call will be moved to the interface in the main menu, this is only for testing and show purpose.
        // This starts when the tower is being constructed, change it to when the tower has already been constructed.
        InitializeGameTurns(2, 40.0f);

    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (_gameStarted)
        {
            UpdateTurnTimer(dt);
        }
    }

#endregion

#region Public Methods

    /// <summary>
    /// This function should be called when the user hits the play button. A small menu asking the user to prompt number of players and turn duration
    /// in seconds and every other rules that they would want to use in their game.
    /// This must be called once the tower has been constructed.
    /// <paramref name="playerNumber"/> Number of players.
    /// <paramref name="singleTurnDuration"/> If It's less than or 0.0f, the turns won't be timed.
    /// </summary>
    public void InitializeGameTurns(int playerNumber, float singleTurnDuration)
    {
        _playerCounter = playerNumber;
        _timedTurns = singleTurnDuration > 0.0f;
        _turnDuration = singleTurnDuration;
        CurrentPlayer = Random.Range(0, _playerCounter);
        CurrentTurn = 1;
        _gameStarted = true;
        UI.UpdateUI(CurrentPlayer, CurrentTurn);
    }

    public void MoveNextTurn()
    {
        CurrentPlayer++;
        CurrentPlayer %= _playerCounter;
        CurrentTurnTimer = 0.0f;
        CurrentTurn++;
        Debug.Log("[TURN CONTROLLER]: Changing to turn number: " + CurrentTurn);
        UI.UpdateUI(CurrentPlayer, CurrentTurn);
    }

#endregion

#region Private Methods
    private void UpdateTurnTimer(float dt)
    {
        CurrentTurnTimer += dt;
        if (CurrentTurnTimer >= _turnDuration)
        {
            MoveNextTurn();
            CurrentTurnTimer = 0.0f;
        }
        int turnTemp = (int)(Mathf.Floor(CurrentTurnTimer * 10.0f) / 10.0f);
        int turnTime = (int)_turnDuration;
        UI.UpdateUITurnTimer(turnTime, turnTemp);
    }
#endregion
}
