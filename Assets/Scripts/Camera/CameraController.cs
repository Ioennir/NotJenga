using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    #region Pulic Attributes

    public Transform target;
    public Vector3 offset;
    public float horizontalDistance;
    public float rotationSpeed;
    public float verticalSpeed;
    
    #endregion

    #region Private Attributes

    private Camera _c;
    private CylindricCoordinates _cc;

    #endregion

    #region Monobehaviour

    private void Start()
    {
        _c = GetComponent<Camera>();
        _cc = new CylindricCoordinates(
            target.position, 
            horizontalDistance,
            0f, 
            0f
        );

        transform.position = target.transform.position + offset + _cc.toCarthesian();
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector2 inputs = new Vector2(xInput, yInput);
        
        UpdateCylindricalCoordinates(dt, inputs);

        transform.position = target.position + _cc.toCarthesian();
        transform.rotation = UpdateCameraRotation();
    }

    #endregion

    #region Public Methods

    

    #endregion

    #region Private Methods

    private void ResetCameraPosition()
    {
        _cc.Angle = 0f;
        _cc.Height = 0f;
    }

    private void UpdateCylindricalCoordinates(float delta, Vector2 inputs)
    {
        float horSpeed = (rotationSpeed * inputs.x) * delta;
        float verSpeed = (verticalSpeed * inputs.y) * delta;

        _cc.Angle += horSpeed;
        _cc.Height += verSpeed;
    }

    private Quaternion UpdateCameraRotation()
    {
        Quaternion newRot = Quaternion.LookRotation(-transform.position, Vector3.up);
        return newRot;
    }

    private void UpdateSphericalPosition(float delta, Vector2 inputs)
    {
        float horAngleOffset = inputs.x * delta;
        float verAngleOffset = inputs.y * delta;
    }

    #endregion

}
