using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Random = UnityEngine.Random;


/// <summary>
/// The idea of this script is to hold the pieces in order (from bottom to top, left to right)
/// </summary>
public class Tower : MonoBehaviour
{
	
	#region Private Variables

	private List<GameObject> _pieces = new List<GameObject>();
	private List<GameObject> _floorPieces = new List<GameObject>();

	private Pool _pool;
	
	#endregion

	#region Public Variables
	
	/// <summary>
	/// Variable will be true
	/// </summary>
	public bool towerAlreadyBuilt = false;
	
	public List<GameObject> badPlaced = new List<GameObject>();
	#endregion

	#region Properties

	public Pool Pool => _pool;

	public GameObject[] Pieces => _pieces.ToArray();

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		_pool = FindObjectOfType<Pool>();
	}

	private int added = 0;

	private void Update()
	{
		/*if (added > 5 || !canBuild)
		{
			return;
		}

		added++;
		GameObject g = _pool.Instantiate();
		g.transform.parent = gameObject.transform;
		PutOnTop(g);*/

		/*if (Time.time % 5 < 0.1f)
		{
			int index = _pieces.Count - 1;
			CameraController cam = FindObjectOfType<CameraController>();
			cam.Target = _pieces[index].transform;
		}*/
	}

	#endregion

    #region Public Methods

    public void AddPiece(GameObject p)
    {
	    _pieces.Add(p);
    }

 
    /// <summary>
    /// Removes piece from the tower (FROM THE LIST)
    /// so you can put it again on the top.
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public GameObject RemovePiece(GameObject p)
    {
	    _pieces.Remove(p);
	    return p;
    }

    /// <summary>
    /// Puts on top and orders it. (THE PIECE MUST BE PHYSICALLY PUT ON THE TOP OF THE JENGA TOWER)
    /// </summary>
    /// <param name="p"></param>
    public void PutOnTop(GameObject p)
    {
	    _pieces.Add(p);
	    int countOfPiecesInTheSameRow = 0;
	    for (int i = _pieces.Count - 1; i >= _pieces.Count - 3; i--)
	    {
		    if (Math.Abs(_pieces[_pieces.Count - 1].transform.position.y - _pieces[i].transform.position.y) < 0.1f)
		    {
			    countOfPiecesInTheSameRow++;
		    }
	    }
		
	    // Already ordered.
	    if (_pieces.Count % 3 == 0) return;
	    
	    // TODO Use this when we can, but i won't do it for the moment because implementing IComparer would require breaking changes in github.
	    // _pieces.Sort(_pieces.Count - countOfPiecesInTheSameRow, countOfPiecesInTheSameRow, );
	    var ordered = _pieces.GetRange(_pieces.Count - countOfPiecesInTheSameRow, countOfPiecesInTheSameRow).OrderBy(g => g.transform.position.x);
	    var rest = _pieces.GetRange(0, _pieces.Count - countOfPiecesInTheSameRow);
	    _pieces = rest.Concat(ordered.ToList()).ToList();
    }

    /// <summary>
    /// Gets a piece starting from the offset, and will move x times horizontal and x times vertical.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    /// <returns></returns>
    public GameObject SelectPiece(ref int offset, int horizontal, int vertical)
    {
	    if (horizontal == 0 && vertical == 0)
	    {
		    return _pieces[offset];
	    }
	    List<GameObject> top = GetTopPieces();
	    if (horizontal != 0)
	    {
		    // todo More logic?
		    if (offset + horizontal < _pieces.Count - top.Count && offset + horizontal >= 0 && (offset += horizontal) > -1);
		    
		    return _pieces[offset];
	    }

	   
	    int count = offset + vertical;
	    while (count < _pieces.Count - top.Count && count >= 0 && SameRow(_pieces[count], _pieces[offset]))
	    {
		    count += vertical;
	    }

	    offset = Mathf.Clamp(count, 0, _pieces.Count - 1 - top.Count);
	    
	    return _pieces[offset];
    }

    public List<GameObject> PiecesOnTheFloor()
    {
	    return _floorPieces;
    }

    public void AddPieceOnFloor(PieceCheckCollision piece)
    {
	    _floorPieces.Add(piece.gameObject);
    }

    public void DestroyPieceOnFloor(PieceCheckCollision piece)
    {
	    _floorPieces.Remove(piece.gameObject);
	    _pieces.Remove(piece.gameObject);
    }

    
    /// <summary>
    /// Get top pieces (will get ONLY the last row) So for example we have a jenga like
    /// - - -  ** Will return this row
    /// - - -
    /// - - -
    ///   
    /// Another case
    /// -   -  ** Will return this row  
    /// - - -
    /// -   -
    ///   - -
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetTopPieces()
    {
	    int countOfPiecesInTheSameRow = 0;
	    for (int i = _pieces.Count - 1; i >= _pieces.Count - 3; i--)
	    {
		    if (Math.Abs(_pieces[_pieces.Count - 1].transform.localPosition.y - _pieces[i].transform.localPosition.y) < 0.1f)
		    {
			    countOfPiecesInTheSameRow++;
		    }
	    }

	    return _pieces.GetRange(_pieces.Count - countOfPiecesInTheSameRow, countOfPiecesInTheSameRow);
    }

     public static bool SameRow(GameObject j1, GameObject j2)
    {
	    return Math.Abs(j1.transform.position.y - j2.transform.position.y) < 0.1f;
    }


     public struct MinimumPiece
     {
	     public float diff;
	     public bool sufficientDifference;
	     public Vector3 position;
     }
     public static MinimumPiece SamePlace(GameObject j1, List<GameObject> others, bool inX)
     {
	     MinimumPiece piece = new MinimumPiece();
	     Vector3 pos = j1.transform.position;
	     piece.sufficientDifference = true;
	     for (int i = 0; i < others.Count; i++)
	     {
		     float diffX = Math.Abs(pos.x - others[i].transform.position.x);
		     float diffZ = Math.Abs(pos.z - others[i].transform.position.z);
		     
		     if (inX && diffX <= 0.1f)
		     {
			     piece.sufficientDifference = false;
			     piece.diff = diffX + diffZ;
			     piece.position = pos;
			     return piece;
		     }

		     if (!inX && diffZ <= 0.1f)
		     {
			     piece.sufficientDifference = false;
			     piece.diff = diffX + diffZ;
			     piece.position = pos;
			     return piece;
		     }
	     }

	     return piece;
     }


    #endregion

    #region Private Methods

    #endregion
    
}
