using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    #region Public Variables
    public Material mat;
    public GameObject arrow;
    public int mode; //tell other scripts which mode are we in
    #endregion
    #region Private Variables
    private string selectableTag = "Selectable";
    private Material previousMaterial;
    private bool alreadySelected = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.tag == selectableTag)
        {
            if (Input.GetMouseButtonDown(0) && !alreadySelected) // Shoot jenga
            { 
                var selection = hit.transform;
                var selectionRenderer = selection.GetComponent<Renderer>();
                var scriptShoot = selection.GetComponent<PieceShoot>();
                var arrowInst = Instantiate(arrow);
                scriptShoot.shootMode = true;
              
                arrowInst.transform.position = hit.transform.position;
                arrowInst.GetComponent<ArrowScript>().cube = hit.transform;
                if (previousMaterial != null)
                {
                    previousMaterial = selectionRenderer.material;
                }
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = mat;
                }
            }
                if (previousMaterial != null)
                {
                    hit.transform.GetComponent<Renderer>().material = previousMaterial;
                }
            if (Input.GetMouseButtonDown(1) && !alreadySelected) //Drag and drop jenga
            {
                mode = 1;
                var selection = hit.transform;
                var selectionRenderer = selection.GetComponent<Renderer>();
                var scriptMove = selection.GetComponent<PieceDragAndDropScript>();
                scriptMove.dragMode = true;
                if (previousMaterial != null)
                {
                    previousMaterial = selectionRenderer.material;
                }
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = mat;
                }
            }
            if (previousMaterial != null)
            {
                hit.transform.GetComponent<Renderer>().material = previousMaterial;
            }
        }
    }
}
    

