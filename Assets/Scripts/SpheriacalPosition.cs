using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SphericalPosition
{
    #region Attributess

    float theta;
    float phi;
    float distance;

    #endregion

    #region Constructor

    public SphericalPosition(float _theta, float _phi, float _distance)
    {
        theta = _theta;
        phi = _phi;
        distance = _distance;
    }

    #endregion

    #region Properties

    public float Phi
    {
        get { return phi; }
        set { phi = value % 360.0f; }
    }
    public float Theta
    {
        get { return theta; }
        set { theta = value % 360.0f; }
    }
    public float Distance
    {
        get { return distance; }
        set { distance = value; }
    }

    #endregion

    #region Methods

    public Vector3 ToCarthesian()
    {
        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);
        float sinPhi = Mathf.Sin(Phi);
        float cosPhi = Mathf.Cos(Phi);
        Vector3 ret = new Vector3(sinTheta * cosPhi, cosTheta, sinTheta * sinPhi) * distance;
        return ret;
    }

    #endregion
}