using System;
using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    
    #region Enumerators

    public enum CameraState
    {
        MovingAroundTower,
        MovingAroundPiece,
        TravellingToDestination,
        FreeMovement
    }

    #endregion
    
    #region Structs

    public struct CameraStateMachine
    {
        public int player;
        public CameraState currentState;
    }

    public struct CameraInputs
    {
        public Vector2 axes;
        public int goUp;
        public int goDown;
        public int zoomIn;
        public int zoomOut;
    }
    
    #endregion
    
    #region Pulic Attributes

    
    #endregion

    #region Private Attributes
    
    private bool _transitioning = false;

    public float transitionTime;
    public float horizontalDistance;
    public float rotationSpeed;
    public float verticalSpeed;
    public float zoomSpeed;
    public Vector3 offset;

    // New version XDD ///////////////////////////////////////////////////////////////////////////////////////
    
    // Target from which the camera will be centered on
    private Transform _target;

    // Camera instance from the scene (it will take the main camera)
    private Camera _camera;
    
    // The Cylindrical Coordinates that the camera will take
    private CylindricalCoordinates _coordinates = new CylindricalCoordinates();

    // Camera current state
    private CameraStateMachine _cameraState = new CameraStateMachine();
    
    // Camera inputs to control the camera
    private CameraInputs _cameraInputs = new CameraInputs();

    #endregion

    #region Properties

    public Transform Target
    {
        get => _target;
        set
        {
            _coordinates.Center = value.position;
            _target = value;
            StartCoroutine(nameof(OnTargetChanged));
        }
    }

    #endregion
    
    #region Monobehaviour
    
    private void Start()
    {
        _target = FindObjectOfType<Tower>().transform;

        _camera = GetComponent<Camera>();
        
        _cameraState.currentState = CameraState.MovingAroundTower;

        _coordinates = new CylindricalCoordinates(_target.position, horizontalDistance, 0f, 0f);

        transform.position = _target.transform.position + offset + _coordinates.toCarthesian();

        offset = _target.position;
    }

    /// <summary>
    /// Read remarks
    /// </summary>
    /// <remarks>
    /// PROPOSAL (GABI): Maybe we should call the Update method of this class from another component
    /// (like a state machine) so we know when the camera is gonna be moved for sure.
    /// </remarks>
    private void Update()
    {
        // Retrieve Delta time
        //float dt = Time.deltaTime;

        // Retrieve inputs
        //RetrieveInputs();
        
        // Update camera position data
        //UpdateCylindricalCoordinates(dt,_goUp, _goDown, _inputs);

        //transform.position = target.position + _cc.toCarthesian();
        //transform.rotation = UpdateCameraRotation();
        
        // New version XDD /////////////////////////////////////////////////////////////////////////////////////////

        switch (_cameraState.currentState)
        {
            case CameraState.MovingAroundTower:
            {
                MovingAroundTowerBehaviour();
            }
                break;
            
            case CameraState.MovingAroundPiece:
            {
                MovingAroundPieceBehaviour();
            }
                break;
            
            case CameraState.TravellingToDestination:
            {
                TravellingToDestinationBehaviour();
            }
                break;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Makes the camera focus a required piece by its position
    /// </summary>
    /// <param name="piecePos"> The desired to focus piece's position in the Tower</param>
    public void FocusPiece(int piecePos)
    {
        // Make the player unable to move the camera to begin the new targeting
        Tower t = FindObjectOfType<Tower>();
        GameObject newPiece = t.SelectPiece(ref piecePos, 0, 0);
        Vector3 newTarget = newPiece.transform.position;
        
        
    }

    #endregion

    #region Private Methods

    private void MovingAroundTowerBehaviour()
    {
        float delta = Time.deltaTime;
        _cameraInputs = GetInputs();
        
        MoveCamera(delta, _cameraInputs);
        
        MoveOffset(delta, _cameraInputs);
    }

    private void MovingAroundPieceBehaviour()
    {
        
    }
    
    private void MoveCamera(float dt, CameraInputs inputs)
    {
        // Movement around tower
        float rotSpeed = inputs.axes.x * (rotationSpeed * dt);
        _coordinates.Angle += rotSpeed;
        
        // Vertical movement
        int verticalDir = inputs.goUp + inputs.goDown;
        float verSpeed = verticalDir * (verticalSpeed * dt);
        _coordinates.Height += verSpeed;
        _coordinates.Height = Mathf.Clamp(_coordinates.Height, 0f, 15f);

        // Zoom (it is just changing radius, not really a zoom)
        int zoomDir = inputs.zoomIn + inputs.zoomOut;
        float zmSpeed = zoomDir * (zoomSpeed * dt);
        _coordinates.Radius += zmSpeed;
        _coordinates.Radius = Mathf.Clamp(_coordinates.Radius, 2f, 10f);

        // Move camera around the tower given the inputs
        transform.position = _target.position + _coordinates.toCarthesian();
        
        // Rotate camera to look to offset
        transform.rotation = UpdateCameraRotation();
    }

    private void MoveOffset(float dt, CameraInputs inputs)
    {
        // Vertical movement with W and S
        float verSpeed = inputs.axes.y * (verticalSpeed * dt);
        Debug.Log(offset);
        // Clamp the Y value of the offset
        offset.y += verSpeed;
        offset.y = Mathf.Clamp(offset.y, 0f, 15f);
    }

    private void TravellingToDestinationBehaviour()
    {
        
    }

    

    private CameraInputs GetInputs()
    {
        CameraInputs newInputs = new CameraInputs();

        // Retrieve axes
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        newInputs.axes = new Vector2(x, y);
        
        // Up and Down keys
        newInputs.goUp = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
        newInputs.goDown = Input.GetKey(KeyCode.LeftControl) ? -1 : 0;
        
        // Zoom In or Zoom Out keys
        newInputs.zoomIn = Input.GetKey(KeyCode.Z) ? -1 : 0;
        newInputs.zoomOut = Input.GetKey(KeyCode.X) ? 1 : 0;

        return newInputs;
    }
    
    /// Old Version Below ////////////////////////////////////////////////////////////////////////////////////////////////

    private Quaternion UpdateCameraRotation()
    {
        Quaternion newRot = Quaternion.LookRotation(-transform.position + offset, Vector3.up);
        return newRot;
    }

    private IEnumerator OnTargetChanged()
    {
        float newHeight = _target.position.y / 2;
        float startHeight = _coordinates.Height;
        float elapsedT = 0.0f;

        while (elapsedT < 1.0f)
        {
            elapsedT += 0.2f;
            _coordinates.Height = Mathf.Lerp(startHeight, newHeight, elapsedT);
            offset = _target.position;
            yield return new WaitForSeconds(0.2f);
        }

    }

    #endregion
    
}
