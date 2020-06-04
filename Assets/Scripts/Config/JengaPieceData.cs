using UnityEngine;

[System.Serializable]
public enum MaterialData {
	Brown,
	LightBrown
}

[System.Serializable]
public class JengaPieceData
{
	#region Private Variables

	#endregion

	#region Public Variables

	public Vector3      position;
	public Vector3      scale;
	public Vector3 	    rotation;
	public MaterialData material;
	
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
}
