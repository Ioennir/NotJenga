using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDragAndDropScript : MonoBehaviour
{
    #region Public Variables
    public float speed = 0.00000005f;
    public string dragButton = "space";
    public string leftMoveButton = "left";
    public string rightMoveButton = "right";
    //public GameObject jengaPiece;
    public bool dragMode = false;
    #endregion
    #region Private Variables
    private Rigidbody rb;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        dragMode = false;
        rb = this.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dragMode)
        {
            DragJenga();
        }
        
        
    }
    public void DragJenga()
    {
        if (Input.GetKey(dragButton))
        {
            //rb.isKinematic = false;
            rb.useGravity = false;
            MoveJenga();
        }

        if (Input.GetKeyUp(dragButton))
        {
            //rb.isKinematic = true;
            rb.useGravity = true;
            dragMode = false;
        }
    }
    public void MoveJenga()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        this.transform.position += new Vector3(horizontal * speed, 0.0f, vertical * speed);

    }
}
