using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScriptJenga : MonoBehaviour
{
    #region Attributes

    // Public
    [Header("Camera Values")]
    public GameObject target;
    public GameObject[] arrayTargets;
    public float distToTarget = 2.0f;
    public float horizontalAngle = 0.0f;
    public float verticalAngle = 75.0f;
    public Vector3 focusedPoint;
    public float horizontalSpeed = 45.0f;
    public float verticalSpeed = 45.0f;
    //Camera movement while going to another point
    public float angularSpeed = 90f;
    public float angularAcceleration = 90f;
    public float speed;
    public float acceleration = 3f;

    // Private
    private SphericalPosition cameraPosition;
    private float zoomMaxSpeed = 2.0f;
    private float zoomMaxAccel = 0.5f;
    private int arrayPos;
    private int arrayLength;
    private float angularVel = 0.0f;
    private Vector3 velocity = new Vector3(0f, 0f, 0f);

    #endregion

    #region Monobehaviour
    void Start()
    {
        cameraPosition.Theta = verticalAngle;
        cameraPosition.Phi = horizontalAngle;
        cameraPosition.Distance = distToTarget;
        arrayPos = 0;
        arrayLength = arrayTargets.Length - 1;
        transform.position = target.transform.position + cameraPosition.ToCarthesian();
    }

    void Update()
    {
        float dt = Time.deltaTime;

        Vector2 cameraInputs = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        UpdateDistanceToTarget(dt);

        UpdateChangeTarget();

        UpdateSphericalPosition(dt, cameraInputs);

        transform.rotation = UpdateCameraRotation();

        transform.LookAt(target.transform.position + focusedPoint);

        transform.position = target.transform.position + cameraPosition.ToCarthesian();

        target = arrayTargets[arrayPos];
    }
    #endregion

    #region Methods

    Quaternion UpdateCameraRotation()
    {
        Quaternion ret = new Quaternion();

        ret = Quaternion.LookRotation(-transform.position, Vector3.up);

        return ret;
    }

    void UpdateSphericalPosition(float delta, Vector2 inputs)
    {
        float horAngleOffset = delta * inputs.x * 2;
        float verAngleOffset = delta * inputs.y * 2;
        cameraPosition.Theta += verAngleOffset;
        cameraPosition.Phi += horAngleOffset;
    }

    void UpdateDistanceToTarget(float delta)
    {
        Camera thisCamera = GetComponent<Camera>();

        float fovIncrement = 0.25f;

        if (Input.GetKey(KeyCode.Z))
        {
            cameraPosition.Distance -= zoomMaxSpeed * delta;

            thisCamera.fieldOfView += fovIncrement;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            cameraPosition.Distance += zoomMaxSpeed * delta;

            thisCamera.fieldOfView -= fovIncrement;
        }
    }

    void UpdateChangeTarget()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            arrayPos++;
            if (arrayPos > arrayLength)
            {
                arrayPos = 0;
            }
            target = arrayTargets[arrayPos];
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            arrayPos--;
            if (arrayPos < 0)
            {
                arrayPos = arrayLength;
            }
            target = arrayTargets[arrayPos];

        }
    }
    
    void UpdateCameraPosition(float dt)
    {
        GameObject nextGameObject = arrayTargets[arrayPos++].transform.GetChild(0).gameObject;
        float angularTarget = nextGameObject.transform.position.x;
        float angularOffset = angularTarget - angularVel;
        angularOffset = Mathf.Clamp(angularOffset, -angularAcceleration * dt, angularAcceleration * dt);
        angularVel += angularOffset;
        transform.eulerAngles += new Vector3(0.0f, angularVel * dt, 0.0f);
        Vector3 target = nextGameObject.transform.forward;
        Vector3 offset = target - velocity;
        offset = Vector3.ClampMagnitude(offset, acceleration * dt);
        velocity += offset;
        transform.position += velocity * dt;

    }

    #endregion
}