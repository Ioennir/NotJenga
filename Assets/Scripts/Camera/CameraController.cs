using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class CameraController : MonoBehaviour
{

    #region Pulic Attributes

    public Transform target;


    #endregion

    #region Private Attributes

    private Camera _c;
    private CylindricCoordinates _cc;
    private int _goUp = 0;
    private int _goDown = 0;
    private Vector2 _inputs = Vector2.zero;
    private bool _transitioning = false;

    [SerializeField] private float transitionTime;
    [SerializeField] public float horizontalDistance;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float verticalSpeed;
    [SerializeField] public Vector3 offset;

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

    private void Start()
    {
        target = FindObjectOfType<Tower>().transform;
        
        _c = GetComponent<Camera>();
        _cc = new CylindricCoordinates(
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
    }

    #endregion

    #region Public Methods

    

    #endregion

    #region Private Methods

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

    #endregion
    
}
