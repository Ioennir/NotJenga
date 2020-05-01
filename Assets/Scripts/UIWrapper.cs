using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWrapper : MonoBehaviour
{

#region Public Variables
    public Text currentPlayer;
    public Text currentTurn;
    public Text turnTimer;
#endregion

#region Private Variables

#endregion

#region Properties

#endregion

#region MonoBehaviour
    void Start()
    {
        
    }

    void Update()
    {
        
    }
#endregion

#region Public Methods
    public void UpdateUI(int playerNum, int turn)
    {
        currentPlayer.text = "CurrentPlayer: " + playerNum;
        currentTurn.text = "CurrentTurn: " + turn;
    }

    public void UpdateUITurnTimer(int turnTime, int timerValue)
    {
        turnTimer.text = "" + (turnTime - timerValue);
    }
#endregion
}
