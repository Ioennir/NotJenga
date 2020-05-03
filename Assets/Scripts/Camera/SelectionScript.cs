using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    #region Public Variables
    public Material mat;
    #endregion
    #region Private Variables
    private string selectableTag = "Selectable";
    private Material previousMaterial;
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
            if (Input.GetMouseButtonDown(0))
            {
                var selection = hit.transform;
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (previousMaterial != null)
                {
                    previousMaterial = selectionRenderer.material;
                }
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = mat;
                }
            }
            else
            {
                if (previousMaterial != null)
                {
                    hit.transform.GetComponent<Renderer>().material = previousMaterial;
                }
            }

        }
    }
}
