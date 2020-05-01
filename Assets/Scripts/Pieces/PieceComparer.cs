using System.Collections.Generic;
using UnityEngine;

public class PieceComparer : IComparer<GameObject>
{
	#region Private Variables

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

    #endregion

    #region Private Methods

    #endregion

    // x: -1, y: 1
    public int Compare(GameObject x, GameObject y)
    {
	    if (!x) return 1;
	    if (!y) return -1;
	    if (x.transform.position.x > y.transform.position.x)
	    {
		    return 1;
	    }
	    return -1;
    }
}
