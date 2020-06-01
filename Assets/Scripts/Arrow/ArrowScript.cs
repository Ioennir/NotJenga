using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    #region Public Variables
    public float rotSpeed = 100.0f;
    public Transform cube;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        this.transform.Rotate(new Vector3(0.0f, horizontal, 0.0f) * Time.deltaTime * rotSpeed * 10f);
        if (cube != null)
        {
            cube.GetComponent<PieceShoot>().arrow = this.gameObject;
            var normalizeVaribale = Vector3.Normalize(transform.forward);
            cube.GetComponent<PieceShoot>().arrowDir = normalizeVaribale;
        }
    }
}
