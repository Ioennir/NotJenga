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
        TravellingToDestination
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

    public Transform target;
    
    #endregion

    #region Private Attributes

    private Camera _c;
    private CylindricalCoordinates _cc;
    private int _goUp = 0;
    private int _goDown = 0;
    private Vector2 _inputs = Vector2.zero;
    private bool _transitioning = false;

    public float transitionTime;
    public float horizontalDistance;
    public float rotationSpeed;
    public float verticalSpeed;
    public float zoomSpeed;
    public Vector3 offset;

    // New version XDD ///////////////////////////////////////////////////////////////////////////////////////
    
    // Camera instance from the scene (it will take the main camera)
    private Camera _camera;
    
    // The Cylindrical Coordinates that the camera will take
    private CylindricalCoordinates _coordinates;

    // Camera current state
    private CameraStateMachine _cameraState = new CameraStateMachine();
    
    // Camera inputs to control the camera
    private CameraInputs _cameraInputs = new CameraInputs();

    #endregion

    #region Properties

    public Transform Target
    {
        get => target;
        set
        {
            _cc.Center = value.position;
            target = value;
            StartCoroutine(nameof(OnTargetChanged));
        }
    }

    #endregion
    
    #region Monobehaviour

    
    private void Awake()
    {
        
    }

    private void Start()
    {
        target = FindObjectOfType<Tower>().transform;
        
        _c = GetComponent<Camera>();
        _cc = new CylindricalCoordinates(
            target.position, 
            horizontalDistance,
            0f, 
            0f
        );

        transform.position = target.transform.position + offset + _cc.toCarthesian();
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
        float dt = Time.deltaTime;

        // Retrieve inputs
        RetrieveInputs();
        
        // Update camera position data
        UpdateCylindricalCoordinates(dt,_goUp, _goDown, _inputs);

        transform.position = target.position + _cc.toCarthesian();
        transform.rotation = UpdateCameraRotation();
        
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
        float verSpeed = inputs.axes.y * verticalDir * (verticalSpeed * dt);
        _coordinates.Height += verSpeed;
        _coordinates.Height = Mathf.Clamp(_coordinates.Height, 0f, 15f);

        // Zoom (it is just changing radius, not really a zoom)
        int zoomDir = inputs.zoomIn + inputs.zoomOut;
        float zmSpeed = zoomDir * (zoomSpeed * dt);
        _coordinates.Radius += zmSpeed;
        _coordinates.Radius = Mathf.Clamp(_coordinates.Radius, 2f, 10f);
        
    }

    private void MoveTarget()
    {
        
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
        newInputs.zoomIn = Input.GetKey(KeyCode.Z) ? 1 : 0;
        newInputs.zoomOut = Input.GetKey(KeyCode.X) ? -1 : 0;

        return newInputs;
    }
    
    private void RetrieveInputs()
    {
        // Vertical movement of camera
        _goUp = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
        _goDown = Input.GetKey(KeyCode.LeftControl) ? -1 : 0;
        
        // Horizontal position and vertical rotation of camera
        _inputs.x = Input.GetAxisRaw("Horizontal");
        _inputs.y = Input.GetAxisRaw("Vertical");
    }

    private void ResetCameraPosition()
    {
        _cc.Angle = 0f;
        _cc.Height = 0f;
    }

    private void UpdateCylindricalCoordinates(float delta, int upwards, int downwards, Vector2 axes)
    {
        // Get rotation and vertical speed values
        float rotSpeed = (rotationSpeed * axes.x) * delta;
        float verSpeed = verticalSpeed * (upwards + downwards) * delta;

        // Apply changes to Cylindrical Coordinates
        _cc.Angle += rotSpeed;
        _cc.Height += verSpeed;
        
        // Update offset so we don't need to update rotation
        offset.y += (verticalSpeed * axes.y) * delta;
        offset.y = Mathf.Clamp(offset.y, 0.0f, 10.0f);
    }

    private Quaternion UpdateCameraRotation()
    {
        Quaternion newRot = Quaternion.LookRotation(-transform.position + offset, Vector3.up);
        return newRot;
    }

    private void UpdateSphericalPosition(float delta, Vector2 inputs)
    {
        float horAngleOffset = inputs.x * delta;
        float verAngleOffset = inputs.y * delta;
    }

    private IEnumerator OnTargetChanged()
    {
        float newHeight = target.position.y / 2;
        float startHeight = _cc.Height;
        float elapsedT = 0.0f;

        while (elapsedT < 1.0f)
        {
            elapsedT += 0.2f;
            _cc.Height = Mathf.Lerp(startHeight, newHeight, elapsedT);
            offset = target.position;
            yield return new WaitForSeconds(0.2f);
        }

    }

    private void InitialAnimation()
    {
        
    }

    #endregion
    
}
