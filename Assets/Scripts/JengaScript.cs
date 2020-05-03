using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    #region public
    public float mass;
    public float frictionStatic;
    public float frictionDynamic;
    public bool isSticky;
    public PhysicMaterial physicsMaterial;
    #endregion

    #region private
    private Collider coll;
    private Rigidbody rb;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        coll = this.GetComponent<Collider>();
        physicsMaterial.staticFriction = frictionStatic;
        physicsMaterial.dynamicFriction = frictionDynamic;

        coll.material = physicsMaterial;

        rb.mass = mass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
