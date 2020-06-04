using System.Collections.Generic;
using UnityEngine;

public class PullBehaviour : MonoBehaviour
{
	#region Private Variables
	
	private PieceDragAndDropScript _currentPieceDrag;

	private PieceShoot _currentPieceShoot;

	private GameObject _currentPiece;

	private bool dragging = false;

	private Tower _tower;

	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

    private void Start()
    {
	    _tower = FindObjectOfType<Tower>();
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

	    List<GameObject> floorPieces = _tower.PiecesOnTheFloor();
	    if (_tower.badPlaced.Count > 0 && !_tower.badPlaced.Contains(_currentPiece))
	    {
		    Debug.Log("LOSE");
		    return true;
	    }
	    
		if (floorPieces.Count > 1 || floorPieces.Count == 1 && floorPieces.IndexOf(_currentPiece) <= -1)
		{	
			Debug.Log("LOSE");
			return true;
		}

		if (floorPieces.Count == 1)
		{
			_currentPiece.GetComponent<PieceCheckCollision>().OnDestroy();
			return true;
		}
		
	    if (dragging)
	    {
		    _currentPieceDrag.DragJenga();
		    return false;
	    }
	    _currentPieceShoot.DragJenga();
	    return false;
    }

    public void Dispose()
    {
	    _currentPiece = null;
    }
    
    #endregion

    #region Private Methods

    #endregion
}
