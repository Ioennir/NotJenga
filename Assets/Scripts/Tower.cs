using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// The idea of this script is to hold the pieces in order (from bottom to top, left to right)
/// </summary>
public class Tower : MonoBehaviour
{
	#region Private Variables

	private List<GameObject> _pieces = new List<GameObject>();

	private Pool _pool;

	public bool canBuild = false;
	
	#endregion

	#region Public Variables

	#endregion

	#region Properties

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
    /// Puts on top and orders it. (THE PIECE MUST BE ALREADY PUT ON THE TOP OF THE JENGA TOWER)
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

    public List<GameObject> GetTopPieces()
    {
	    int countOfPiecesInTheSameRow = 0;
	    for (int i = _pieces.Count - 1; i >= _pieces.Count - 3; i--)
	    {
		    if (Math.Abs(_pieces[_pieces.Count - 1].transform.position.y - _pieces[i].transform.position.y) < 0.1f)
		    {
			    countOfPiecesInTheSameRow++;
		    }
	    }
	    return _pieces.GetRange(_pieces.Count - countOfPiecesInTheSameRow, countOfPiecesInTheSameRow);
    }


    #endregion

    #region Private Methods

    #endregion
}
