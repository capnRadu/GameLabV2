using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    LineScript activeLine;
  
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<LineScript>();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }

        if (activeLine != null)
        {
            var mousePos = Input.mousePosition;
            //mousePos.x = Screen.width - mousePos.x;
            //mousePos.y = Screen.height - mousePos.y;
            mousePos.z = -80;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            activeLine.UpdateLine(worldPos);
        }
    }
}
