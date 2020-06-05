using System;
using UnityEngine;

public class PieceCheckCollision : MonoBehaviour
{
	#region Private Variables
	private bool _inFloor = false;
	private bool _addedToList = false;
	private Tower _tower;
	private bool _badPlaced = false;
	private Rigidbody _rb;
	
	#endregion

	#region Public Variables
	public Material prevMaterial;
	#endregion

	#region Properties

	public bool InFloor => _inFloor;

	public bool BadPlaced => _badPlaced;

	public Rigidbody Rb => _rb;
	
	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void Start()
    {
	    _tower = FindObjectOfType<Tower>();
    }

	private void Update()
	{
		if (_addedToList) return;

		Vector3 angles = transform.localRotation.eulerAngles;
		angles.x = ClampAngle(angles.x);
		angles.z = ClampAngle(angles.z);
		bool condition = true;
		
		if ((Math.Abs(angles.x) > 12.5f || Math.Abs(angles.z) > 12.5f && condition) && !_badPlaced && _tower.towerAlreadyBuilt)
		{
			_badPlaced = true;
			_tower.badPlaced.Add(gameObject);
		}
		if (!_inFloor) return;
		
		_tower.AddPieceOnFloor(this);
	    _addedToList = true;
    
    }

    #endregion

    #region Public Methods
	
    #endregion

    #region Private Methods

    private float ClampAngle(float angle)
    {
	    return angle > 350 ? angle - 360 : angle;
    }

    private void OnTriggerStay(Collider other)
    {
	    if (other.gameObject.CompareTag("Floor"))
	    {
		    _inFloor = true;
	    }
    }


    public void OnDestroy()
    {
	    if (_inFloor)
	    {
		    _tower.DestroyPieceOnFloor(this);
	    }

	    if (_badPlaced)
	    {
		    _tower.badPlaced.Remove(gameObject);
	    }
	    GetComponent<Renderer>().material = PieceOriginalMaterial.Get(gameObject).originalMaterial;
	    gameObject.tag = "Finish";
    }

    #endregion
}
