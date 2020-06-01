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
        /*
        // SUGGESTION (GABI): Camera.main every Update() is very expensive
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // SUGGESTION (GABI): Tag comparison with == comparator is not ideal, use transform.CompareTag(selectableTag) instead 
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
                if (!previousMaterial)
                {
                    previousMaterial = selectionRenderer.material;
                }
                // SUGGESTION (GABI): Above 
                if (!selectionRenderer)
                {
                    selectionRenderer.material = mat;
                }
            }
            if (!previousMaterial)
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
                if (!previousMaterial)
                {
                    previousMaterial = selectionRenderer.material;
                }
                if (!selectionRenderer)
                {
                    selectionRenderer.material = mat;
                }
            }
            if (!previousMaterial)
            {
                hit.transform.GetComponent<Renderer>().material = previousMaterial;
            }
        }*/
    }

    public GameObject Tick(GameObject chosenFromStateMachine = null)
    {
        if (chosenFromStateMachine)
        {
            var selection = chosenFromStateMachine.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();
            var scriptShoot = selection.GetComponent<PieceShoot>();
            var arrowInst = Instantiate(arrow);
            scriptShoot.shootMode = true;
              
            arrowInst.transform.position = chosenFromStateMachine.transform.position;
            arrowInst.GetComponent<ArrowScript>().cube = chosenFromStateMachine.transform;
            if (!previousMaterial)
            {
                previousMaterial = selectionRenderer.material;
            }
            // SUGGESTION (GABI): Above 
            if (!selectionRenderer)
            {
                selectionRenderer.material = mat;
            }
            return selection.gameObject;
        }
        
         // SUGGESTION (GABI): Camera.main every Update() is very expensive
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // SUGGESTION (GABI): Tag comparison with == comparator is not ideal, use transform.CompareTag(selectableTag) instead 
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
                if (!previousMaterial)
                {
                    previousMaterial = selectionRenderer.material;
                }
                // SUGGESTION (GABI): Above 
                if (!selectionRenderer)
                {
                    selectionRenderer.material = mat;
                }
                return selection.gameObject;
            }
            if (!previousMaterial)
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
                if (!previousMaterial)
                {
                    previousMaterial = selectionRenderer.material;
                }
                if (!selectionRenderer)
                {
                    selectionRenderer.material = mat;
                }

                return selection.gameObject;
            }
            if (!previousMaterial)
            {
                hit.transform.GetComponent<Renderer>().material = previousMaterial;
            }

          
        }

        return null;
    }
}
    

