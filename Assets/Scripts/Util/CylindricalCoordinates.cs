using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

/// <summary>
/// Spherical coordinates to define a point in space
/// </summary>
public class CylindricCoordinates
{
    #region Private Attributes

    private Vector3 _c;
    private float _r;
    private float _a;
    private float _h;

    #endregion

    #region Constructors

    public CylindricCoordinates()
    {
        _c = Vector3.zero;
        _r = 0f;
        _a = 0f;
        _h = 0f;
    }

    public CylindricCoordinates(Vector3 center, float radius, float angle, float height)
    {
        _c = center;
        _r = radius;
        _a = angle;
        _h = height;
    }

    public CylindricCoordinates(float radius, float angle, float height)
    {
        _c = Vector3.zero;
        _r = radius;
        _a = angle;
        _h = height;
    }
    
    #endregion

    #region Properties

    /// <summary>
    /// Center of the virtual cylinder.
    /// </summary>
    public Vector3 Center
    {
        get { return _c; }
        set { _c = value; }
    }

    /// <summary>
    /// Distance to the center of the virtual cylinder.
    /// </summary>
    public float Radius
    {
        get { return _r; }
        set { _r = value; }
    }

    /// <summary>
    /// Angle offset from the center of the base of the cylinder.
    /// </summary>
    public float Angle
    {
        get { return _a; }
        set { _a = value; }
    }


    /// <summary>
    /// Height from the base of the virtual cylinder 
    /// </summary>
    public float Height
    {
        get { return _h; }
        set { _h = value; }
    }

    /// <summary>
    /// Distance to the center of the base of the cylinder
    /// </summary>
    public float DistanceToCenter
    {
        get { return Mathf.Sqrt((_h * _h) + (_r * _r)); }
    }
    
    #endregion

    #region Methods

    public Vector3 toCarthesian()
    {
        float x = _r * Mathf.Cos(_a);
        float y = _h;
        float z = _r * Mathf.Sin(_a);
        Vector3 v = new Vector3(x, y, z);

        return v;
    }

    #endregion
    
}
