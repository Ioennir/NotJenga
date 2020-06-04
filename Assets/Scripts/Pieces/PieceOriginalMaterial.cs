using UnityEngine;

public class PieceOriginalMaterial : MonoBehaviour
{
	#region Private Variables

	#endregion

	#region Public Variables
	[HideInInspector]
	public Material originalMaterial;
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

    public static PieceOriginalMaterial Get(GameObject jenga)
    {
	    return jenga.GetComponent<PieceOriginalMaterial>();
    }
    #endregion

    #region Private Methods

    #endregion
}
