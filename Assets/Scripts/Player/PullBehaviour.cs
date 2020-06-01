using UnityEngine;

public class PullBehaviour : MonoBehaviour
{
	#region Private Variables
	
	private PieceDragAndDropScript _currentPieceDrag;

	private PieceShoot _currentPieceShoot;

	private GameObject _currentPiece;

	private bool dragging = false;

	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

    private void Start()
    {

    }

	private void Update()
    {
        
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jenga"></param>
    /// <returns>If the turn is finished</returns>
    public bool Tick(GameObject jenga)
    {

	    if (Input.GetKey(KeyCode.Alpha1))
	    {
		    dragging = true;
	    }

	    if (Input.GetKey(KeyCode.Alpha2))
	    {
		    dragging = false;
	    }
	    
	    if (!_currentPiece)
	    {
		    _currentPiece = jenga;
		    _currentPieceShoot = jenga.GetComponent<PieceShoot>();
		    _currentPieceDrag = jenga.GetComponent<PieceDragAndDropScript>();
	    }

	    if (dragging)
	    {
		    // TODO Return true if the Dragging is done else false if it's not done.
		    _currentPieceDrag.DragJenga();
		    return false;
	    }
	    _currentPieceShoot.DragJenga();
	    return false;
    }
    
    #endregion

    #region Private Methods

    #endregion
}
