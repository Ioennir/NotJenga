using System;
using UnityEngine;

public class SelectionBehaviour : MonoBehaviour
{
	#region Private Variables

	private Tower _towerData;
	private int _selectedPiece = 0;
	private GameObject _currentPiece = null;
	private Material _prevMaterial;

	[SerializeField]
	private Material selectionMaterial;

	
	private float _timerForNextInput = 0.05f;
	private float _accumulatorForNextInput = 0.05f;

	#endregion

	#region Public Variables

	#endregion

	#region Properties

	#endregion

	#region MonoBehaviour

    private void Start()
    {
	    _towerData = FindObjectOfType<Tower>();
	    
    }

    private void Update()
    {
	    if (_accumulatorForNextInput >= _timerForNextInput) return;
	    _accumulatorForNextInput += Time.deltaTime;
    }

    #endregion

    #region Public Methods

    public GameObject Tick()
    {
	    if (_accumulatorForNextInput < _timerForNextInput) return null;
	    _accumulatorForNextInput = 0.0f;
	    // SPACE
	    float select = Input.GetAxisRaw("Jump");
	    float vertical = Input.GetAxisRaw("Vertical");
	    float horizontal = Input.GetAxisRaw("Horizontal");
	    GameObject nextPiece = _towerData.SelectPiece(ref _selectedPiece, (int) horizontal, (int) vertical);
	    PieceOriginalMaterial originalNextPiece = PieceOriginalMaterial.Get(nextPiece);
	    if (nextPiece == _currentPiece)
	    {
		    MeshRenderer rend = nextPiece.GetComponent<MeshRenderer>();
		    return select > 0.5f && (rend.material = originalNextPiece.originalMaterial) ? _currentPiece : null;
	    }
	    if (_currentPiece)
	    {
		    MeshRenderer rend = _currentPiece.GetComponent<MeshRenderer>();
		    PieceOriginalMaterial originalPiece = PieceOriginalMaterial.Get(_currentPiece);
		    rend.material = originalPiece.originalMaterial;
	    }
	    MeshRenderer rendNext = nextPiece.GetComponent<MeshRenderer>();
	    _prevMaterial = rendNext.material;
	    rendNext.material = selectionMaterial;
	    _currentPiece = nextPiece;
	    return select > 0.5f && (rendNext.material = selectionMaterial) ? _currentPiece : null;
    }

    public void Dispose(GameObject selected)
    {
	    if (!_currentPiece)
	    {
		    _selectedPiece = 0;
		    _prevMaterial = null;
		    return;
	    }

	    if (selected != _currentPiece)
	    {
		    _currentPiece.GetComponent<MeshRenderer>().material = _currentPiece.GetComponent<PieceOriginalMaterial>().originalMaterial;
		    
	    }
	    _currentPiece = null;
	    
    }

    public void DisposeState()
    {
	    _currentPiece = null;
	    _selectedPiece = 0;
	    _prevMaterial = null;
    }

    #endregion

    #region Private Methods

    #endregion
}
