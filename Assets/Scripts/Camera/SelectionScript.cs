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
    private Transform selection;
    private Tower _tower;
    private List<GameObject> _topPieces;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _tower = FindObjectOfType<Tower>();
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
        if (_topPieces == null)
        {
            _topPieces = _tower.GetTopPieces();
        }
        if (chosenFromStateMachine)
        {
            HandleSelectionFromStateMachine(chosenFromStateMachine);
            return selection.gameObject;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag(selectableTag))
        {
           return HandleHit(hit);
        }
        Renderer selectionRendererSelection = selection.GetComponent<Renderer>();
        selectionRendererSelection.material = previousMaterial;
        selection = null;
        return null;
    }

    private GameObject HandleHit(RaycastHit hit)
    {
        if (_topPieces.Contains(hit.transform.gameObject))
        {
            return null;
        }
        if (selection && previousMaterial && hit.transform != selection.transform)
        {
            Renderer selectionRenderer = selection.GetComponent<Renderer>();
            selectionRenderer.material = previousMaterial;
            previousMaterial = null;
            selection = hit.transform;
            return null;
        }
        if (Input.GetMouseButtonDown(0) && !alreadySelected) // Shoot jenga
        { 
            selection = hit.transform;
            PieceShoot scriptShoot = selection.GetComponent<PieceShoot>();
            GameObject arrowInst = Instantiate(arrow);
            scriptShoot.shootMode = true;
            arrowInst.transform.position = hit.transform.position;
            arrowInst.GetComponent<ArrowScript>().cube = hit.transform;
            return selection.gameObject;
        }
        if (Input.GetMouseButtonDown(1) && !alreadySelected) //Drag and drop jenga
        {
            mode = 1;
            selection = hit.transform;
            var scriptMove = selection.GetComponent<PieceDragAndDropScript>();
            scriptMove.dragMode = true;
            return selection.gameObject;
        }
        selection = hit.transform;
        Renderer selectionRendererEnd = selection.GetComponent<Renderer>();
        if (!previousMaterial) 
            previousMaterial = selectionRendererEnd.material;
        selectionRendererEnd.material = mat;
        return null;
    }

    private void HandleSelectionFromStateMachine(GameObject chosenFromStateMachine)
    {
        selection = chosenFromStateMachine.transform;
        Renderer selectionRenderer = selection.GetComponent<Renderer>();
        PieceShoot scriptShoot = selection.GetComponent<PieceShoot>();
        GameObject arrowInst = Instantiate(arrow);
        scriptShoot.shootMode = true;
        arrowInst.transform.position = chosenFromStateMachine.transform.position;
        arrowInst.GetComponent<ArrowScript>().cube = chosenFromStateMachine.transform;
        if (!previousMaterial)
        {
            previousMaterial = selectionRenderer.material;
        }
        if (!selectionRenderer)
        {
            selectionRenderer.material = mat;
        }
    }
}
    

