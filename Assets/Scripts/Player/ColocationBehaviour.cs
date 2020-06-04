using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ColocationBehaviour : MonoBehaviour
{
	#region Private Variables

	private Pool _jengaPool;

	private Tower _towerData;

	private TowerGenerator _tower;

	private List<GameObject> _topPieces = new List<GameObject>();

	private bool _doingTurn = false;

	private GameObject _imaginaryJenga = null;

	private GameObject _parentOfImaginaryJenga = null;

	private float _accumulatorUntilNextInput = 0.0f;

	private float _timerUntilNextInput = 0.4f;

	private int _currentJenga = 0;

	private TurnController _turnController;
	

	private Vector3 _pos = new Vector3();

	[FormerlySerializedAs("_matTransparent")] [SerializeField]
	private Material matTransparent;

	/// <summary>
	/// TODO Set this via a function
	/// </summary>
	[FormerlySerializedAs("_originalMaterial")] [SerializeField]
	private Material originalMaterial;

	#endregion

	#region Public Variables

	#endregion

	#region Properties

	public bool DoingTurn => _doingTurn;
	
	#endregion

	#region MonoBehaviour

	private void Start()
	{
		_towerData = FindObjectOfType<Tower>();
		_jengaPool = FindObjectOfType<Pool>();
		_tower = FindObjectOfType<TowerGenerator>();
		_turnController = FindObjectOfType<TurnController>();
	}

	private void Update()
	{
		/*
		// This is only testing, the Tick() method will be called in another State Machine
		if (!_towerData.towerAlreadyBuilt) return;
		Tick();
		if (!_doingTurn) return;
		*/
		_accumulatorUntilNextInput += Time.deltaTime;
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// TODO GABI: Get the material of the last extracted piece
	///
	/// </summary>
	/// <returns>If the player is done placing the piece</returns>
	public bool Tick()
	{
		PlacingOfPiece();
		float inputHorizontal = Input.GetAxisRaw("RightMoveJengaColocation");
		float inputVertical = Input.GetAxisRaw("Jump");
		// If you wanna end the turn
		if (inputVertical > 0.5f && _accumulatorUntilNextInput > _timerUntilNextInput)
		{
			_accumulatorUntilNextInput = 0.0f;
			_doingTurn = false;
			return true;
		}
		// If there are no inputs
		if (inputHorizontal < 0.5f || _accumulatorUntilNextInput < _timerUntilNextInput)
		{
			return false;
		}

		// If we are putting in an empty row
		if (_topPieces.Count >= 3)
		{
			_currentJenga = (_currentJenga + 1) % _topPieces.Count;
			/*
			_posX = ClampOver(
				_posX + 0.5f, 
				_tower.TowerCenter.x - _imaginaryJenga.transform.localScale.x,
				_tower.TowerCenter.x + _imaginaryJenga.transform.localScale.x);
				*/
			ClampOverXZ();
		}
		// If we are not in an empty row just pass position
		else
		{
			/*_posX = _posX = ClampOver(
				_posX + 0.5f, 
				_tower.TowerCenter.x - _imaginaryJenga.transform.localScale.x,
				_tower.TowerCenter.x + _imaginaryJenga.transform.localScale.x);
		*/
			ClampOverXZ();
		}

		
		_accumulatorUntilNextInput = 0.0f;

		return false;
	}

	/// <summary>
	/// Call it when you are gonna stop using ColocationBehaviour or things will bug out
	/// </summary>
	public void DisposeTurn()
	{
		// This is an EXTREME case and shouldn't happen often.
		if (!_imaginaryJenga)
		{
			PlacingOfPiece();
		}
		_topPieces = new List<GameObject>();
		Collider col = _imaginaryJenga.GetComponent<Collider>();
		col.isTrigger = false;
		Rigidbody rb = _imaginaryJenga.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		MeshRenderer rend = _imaginaryJenga.GetComponent<MeshRenderer>();
		rend.material = originalMaterial;
		_towerData.PutOnTop(_imaginaryJenga);
		_imaginaryJenga.name = $"Piece put by player {_turnController.CurrentPlayer}";
		_imaginaryJenga = null;
		_currentJenga = 0;
		_doingTurn = false;
		
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Logic of the new piece
	/// </summary>
	private void PlacingOfPiece()
	{
		if (_topPieces.Count == 0)
		{
			// NOTE: (GABI)
			// Get top pieces (will get ONLY the last row) So for example we have a jenga like
			// - - - << will return this row
			// - - -
			// - - -

			// Another case
			// -   -  << Will return this row  
			// - - -
			// - - -
			// - - -

			_topPieces = _towerData.GetTopPieces();
		}

		if (!_imaginaryJenga)
		{
			_imaginaryJenga = _jengaPool.Instantiate();
			_imaginaryJenga.transform.localScale = _topPieces[0].transform.localScale;
			MeshRenderer rend = _imaginaryJenga.GetComponent<MeshRenderer>();
			rend.material = matTransparent;
			
			Collider col = _imaginaryJenga.GetComponent<Collider>();
			col.isTrigger = true;
			Rigidbody rb = _imaginaryJenga.GetComponent<Rigidbody>();
			rb.isKinematic = true;
			_imaginaryJenga.name = "Imaginary Jenga [CHOOSING POSITION]";
			// NOTE (GABI): This adds it into the tower jerarchy so we don't have to change coordinates below
			_tower.AddPiece(_imaginaryJenga);
			
		}
		Vector3 imaginaryJengaPosition = _imaginaryJenga.transform.position;
		Vector3 currentJengaPosition = _topPieces[_currentJenga].transform.position;
		// If we are putting a piece on an empty row
		if (_topPieces.Count == 3) 
		{
			// Rotate the piece (because the rotation differs between rows)
			_imaginaryJenga.transform.rotation =
				Quaternion.AngleAxis(Math.Abs(_topPieces[_currentJenga].transform.rotation.eulerAngles.y) + 90.0f,
					Vector3.up);
			// Put the piece above
			imaginaryJengaPosition.y = currentJengaPosition.y + _tower.PieceHeight;
		}
		// Non empty row
		else
		{
			// Use the same rotation as its peers
			_imaginaryJenga.transform.rotation = _topPieces[_currentJenga].transform.rotation;
			imaginaryJengaPosition.y = currentJengaPosition.y + _tower.PieceHeight;
		}
		// New rotation
		float rot = Math.Abs(_imaginaryJenga.transform.rotation.eulerAngles.y);
		// If we need to move in Z axis or X axis.
		bool conditionForUsingMovementInZAxis = rot > 80f && rot < 170 || rot > 260 && rot < 290;
		do
		{
			imaginaryJengaPosition.z = conditionForUsingMovementInZAxis ? _pos.z : _tower.TowerCenter.z;
			imaginaryJengaPosition.x =conditionForUsingMovementInZAxis ? _tower.TowerCenter.x : _pos.x;
			// Update position
			_imaginaryJenga.transform.localPosition = imaginaryJengaPosition;
			// Check if there is overlapping in x or z axis with another piece in the same row
			// (only for rows that have less than 3 pieces, because if
			// there are 3 pieces you are putting the new piece on top)
		} while (Tower.SamePlace(
			         _imaginaryJenga, 
			         _topPieces, 
			         !conditionForUsingMovementInZAxis) && 
					 _topPieces.Count != 3 &&
		         // update pos in another iteration if the conditions are met.
		             ClampOverXZ()
		 );
		
		
		_imaginaryJenga.transform.position = imaginaryJengaPosition;
	}

	/// <summary>
	/// Applies ClampOver to x and z positions, adding localScale and having
	/// tower center as a reference
	/// </summary>
	/// <returns></returns>
	private bool ClampOverXZ()
	{
		Vector3 localScale = _imaginaryJenga.transform.localScale;
		_pos.x = ClampOver(_pos.x + localScale.x,
			_tower.TowerCenter.x - localScale.x,
			_tower.TowerCenter.x + localScale.x);
		_pos.z = ClampOver(_pos.z + localScale.z,
			_tower.TowerCenter.z - localScale.z,
			_tower.TowerCenter.z + localScale.z);
		return true;
	}
	
	/// <summary>
	/// Clamps the value. If <param name="n"></param> is more than <param name="max"></param>
	/// if will be set to <param name="min"></param> and viceversa.
	/// </summary>
	/// <param name="n"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <param name="offset">Because we are working with floats you might have an offset</param>
	/// <returns></returns>
	public float ClampOver(float n, float min, float max, float offset = 0.1f)
	{
		if (n > max + offset)
		{
			n = min;
			return n;
		}

		if (n < min + offset)
		{
			n = max;
		}

		return n;
	}
	#endregion
}
