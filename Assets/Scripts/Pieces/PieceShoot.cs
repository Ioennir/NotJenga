using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceShoot : MonoBehaviour
{
    #region Public Variables
    public float force = 0.0f;
    public Vector3 arrowDir;
    public bool shootMode;
    public Vector3 shootAxis;
    public GameObject arrow;
    public Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);
    #endregion

    #region Private Variables
    private Vector3 maxScale = new Vector3(2.0f, 2.0f, 2.0f);
    private Vector3 minScale = new Vector3(0.01f, 0.01f, 0.01f);
    private float forceProduct = 0.0f;
    private Rigidbody rb;
    private bool isShooting = true;
    
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        arrowDir = new Vector3(0, 0, 0);
        rb = this.GetComponent<Rigidbody>();
        shootMode = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shootMode)
        {
            DragJenga();
        }
    }

    public void DragJenga()
    {
        if (arrowDir != null)
        {
            shootAxis = arrowDir;
        }
        if (Input.GetKey("space"))
        {
            Vector3 vec = new Vector3((Mathf.Sin(Time.time) + 1) / 4, (Mathf.Sin(Time.time) + 1) / 4, (Mathf.Sin(Time.time) + 1) / 4);
            arrow.transform.localScale = vec;
            forceProduct = (Mathf.Sin(Time.time) + 3);

            /*if (arrow.transform.localScale.x >= maxScale.x)
            {
                arrow.transform.localScale -= scaleChange *Time.deltaTime;
            }
            if(transform.localScale.x <= minScale.x)
            {
                arrow.transform.localScale += scaleChange * Time.deltaTime;
            }*/
            Debug.Log(arrow.transform.localScale);
        }
        if (Input.GetKeyUp("space"))
        {
            Destroy(arrow);
            var direct = arrowDir * forceProduct;
            Debug.Log("Meto golpe");
            Debug.Log(direct);
            rb.AddForce(direct, ForceMode.Impulse);
            isShooting = false;
        }
    }
}
