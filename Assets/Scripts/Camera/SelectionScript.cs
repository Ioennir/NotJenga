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
        // SUGGESTION (GABI): Camera.main every Update() is very expensive
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // SUGGESTION (GABI): Tag comparison with == comparator is not ideal, use transform.CompareTag(selectableTag) instead 
        if (Physics.Raycast(ray, out hit) && hit.transform.tag == selectableTag)
        {
            // SUGGESTION (GABI): Move this above so we only check for Raycast when Click
            if (Input.GetMouseButtonDown(0))
            {
                var selection = hit.transform;
                var selectionRenderer = selection.GetComponent<Renderer>();
                // SUGGESTION (GABI): previousMaterial != null is expensive, use ! operator instead
                if (previousMaterial != null)
                {
                    previousMaterial = selectionRenderer.material;
                }
                // SUGGESTION (GABI): Above 
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = mat;
                }
            }
            else
            {
                // SUGGESTION (GABI): Above
                if (previousMaterial != null)
                {
                    hit.transform.GetComponent<Renderer>().material = previousMaterial;
                }
            }

        }
    }
}
