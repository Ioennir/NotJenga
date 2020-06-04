using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameActivator : MonoBehaviour
{
    public Camera menuCamera;
    public Camera gameCamera;

    public GameObject Tower;
    public Text textrows;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ResetTower()
    {
        int rows = 7;
        int parsedrows = int.Parse(textrows.text);
        if (parsedrows > 2 && parsedrows < 17)
        {
            rows = parsedrows;
        }
        //create a tower reset 
    }
}
