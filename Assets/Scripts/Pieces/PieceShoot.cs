using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceShoot : MonoBehaviour
{
    #region Public Variables
    public float force = 0.0f;
    public Vector3 shootAxis = new Vector3(10.0f, 0.0f, 0.0f);
    #endregion

    #region Private Variables
    Rigidbody rb;
    bool isShooting = true;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isShooting)
        {
            rb.AddForce(shootAxis);
            isShooting = false;
        }
        
    }
}
