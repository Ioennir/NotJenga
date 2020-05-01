using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceFriction : MonoBehaviour
{
    #region Public Variables
    public float mass;
    public float frictionStatic;
    public float frictionDynamic;
    public bool isSticky;
    public PhysicMaterial physicsMaterial;
    #endregion

    #region Private Variables
    private Collider coll;
    private Rigidbody rb;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        physicsMaterial = new PhysicMaterial();
        physicsMaterial.staticFriction = frictionStatic;
        physicsMaterial.dynamicFriction = frictionDynamic;

        coll.material = physicsMaterial;

        rb.mass = mass;


    }
    
}
