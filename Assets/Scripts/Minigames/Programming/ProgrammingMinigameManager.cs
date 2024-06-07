using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ProgrammingMinigameManager : MonoBehaviour
{
    public GameObject[] canvases;
    private GameObject currentCanvas;

    [Header("Minigame 1")]
    [SerializeField] private Canvas canvas1;
    [SerializeField] private Canvas canvas2;
    [SerializeField] private Canvas canvas3;

    [Header("Minigame 2")]
    [SerializeField] private Canvas canvas4;
    [SerializeField] private Canvas canvas5;
    [SerializeField] private Canvas canvas6;

    [Header("Minigame 3")]
    [SerializeField] private Canvas canvas7;
    [SerializeField] private Canvas canvas8;
    [SerializeField] private Canvas canvas9;

    [Header("Minigame 4")]
    [SerializeField] private Canvas canvas10;
    [SerializeField] private Canvas canvas11;
    [SerializeField] private Canvas canvas12;
    
    void Start()
    {
        DeactivateAllCanvases();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            ActivateRandomCanvas();
        }
    }

    void ActivateRandomCanvas()
    {
        DeactivateCanvas(currentCanvas);

        if (canvases.Length > 0)
        {
            int randomIndex = Random.Range(0, canvases.Length);
            currentCanvas = canvases[randomIndex];
            RemoveCanvas(randomIndex);
            ActivateCanvas(currentCanvas);
        }
        else
        {
            Debug.Log("No canvas!");
        }
    }

    void ToggleCanvas(GameObject canvas)
    {

        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }

    void ActivateCanvas(GameObject canvas)
    {

        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }

    void DeactivateCanvas(GameObject canvas)
    {

        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    void DeactivateAllCanvases()
    {

        foreach (var canvas in canvases)
        {
            canvas.SetActive(false);
        }
    }

    void RemoveCanvas(int index)
    {

        if (index >= 0 && index < canvases.Length)
        {
            var tempList = new List<GameObject>(canvases);
            tempList.RemoveAt(index);
            canvases = tempList.ToArray();
        }
    }


    public void Canva1Button1()
    {
        Debug.Log("Wrong");
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);

    }

    public void Canva1Button2()
    {
        Debug.Log("Wrong");
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);
    }

    public void Canva1Button3()
    {
        Debug.Log("Correct");
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);
    }

    public void Canva1Button4()
    {
        Debug.Log("Wrong");
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(true);
    }

    public void Canva2Button1()
    {
        Debug.Log("Correct");
        canvas2.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(true);
    }

    public void Canva2Button2()
    {
        Debug.Log("Wrong");
        canvas2.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(true);
    }

    public void Canva2Button3()
    {
        Debug.Log("Wrong");
        canvas2.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(true);
    }

    public void Canva2Button4()
    {
        Debug.Log("Wrong");
        canvas2.gameObject.SetActive(false);
        canvas3.gameObject.SetActive(true);
    }

    public void Canva3Button1()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva3Button2()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva3Button3()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva3Button4()
    {
        Debug.Log("Correct");
        //finish mini game & deactivate canvas
    }

    public void Canva4Button1()
    {
        Debug.Log("Wrong");
        canvas4.gameObject.SetActive(false);
        canvas5.gameObject.SetActive(true);
    }

    public void Canva4Button2()
    {
        Debug.Log("Wrong");
        canvas4.gameObject.SetActive(false);
        canvas5.gameObject.SetActive(true);
    }

    public void Canva4Button3()
    {
        Debug.Log("Correct");
        canvas4.gameObject.SetActive(false);
        canvas5.gameObject.SetActive(true);
    }

    public void Canva4Button4()
    {
        Debug.Log("Wrong");
        canvas4.gameObject.SetActive(false);
        canvas5.gameObject.SetActive(true);
    }

    public void Canva5Button1()
    {
        Debug.Log("Correct");
        canvas5.gameObject.SetActive(false);
        canvas6.gameObject.SetActive(true);
    }

    public void Canva5Button2()
    {
        Debug.Log("Wrong");
        canvas5.gameObject.SetActive(false);
        canvas6.gameObject.SetActive(true);
    }
    public void Canva5Button3()
    {
        Debug.Log("Wrong");
        canvas5.gameObject.SetActive(false);
        canvas6.gameObject.SetActive(true);
    }
    public void Canva5Button4()
    {
        Debug.Log("Wrong");
        canvas5.gameObject.SetActive(false);
        canvas6.gameObject.SetActive(true);
    }

    public void Canva6Button1()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }
    public void Canva6Button2()
    {
        Debug.Log("Correct");
        //finish mini game & deactivate canvas
    }
    public void Canva6Button3()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }
    public void Canva6Button4()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }
    public void Canva7Button1()
    {
        Debug.Log("Wrong");
        canvas7.gameObject.SetActive(false);
        canvas8.gameObject.SetActive(true);

    }

    public void Canva7Button2()
    {
        Debug.Log("Wrong");
        canvas7.gameObject.SetActive(false);
        canvas8.gameObject.SetActive(true);
    }

    public void Canva7Button3()
    {
        Debug.Log("Wrong");
        canvas7.gameObject.SetActive(false);
        canvas8.gameObject.SetActive(true);
    }
    public void Canva7Button4()
    {
        Debug.Log("Correct");
        canvas7.gameObject.SetActive(false);
        canvas8.gameObject.SetActive(true);
    }
    public void Canva8Button1()
    {
        Debug.Log("Wrong");
        canvas8.gameObject.SetActive(false);
        canvas9.gameObject.SetActive(true);
    }

    public void Canva8Button2()
    {
        Debug.Log("Wrong");
        canvas8.gameObject.SetActive(false);
        canvas9.gameObject.SetActive(true);
    }

    public void Canva8Button3()
    {
        Debug.Log("Wrong");
        canvas8.gameObject.SetActive(false);
        canvas9.gameObject.SetActive(true);
    }
    
    public void Canva8Button4()
    {
        Debug.Log("Correct");
        canvas8.gameObject.SetActive(false);
        canvas9.gameObject.SetActive(true);
    }

    public void Canva9Button1()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva9Button2()
    {
        Debug.Log("Correct");
        //finish mini game & deactivate canvas
    }

    public void Canva9Button3()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva9Button4()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva10Button1()
    {
        Debug.Log("Wrong");
        canvas10.gameObject.SetActive(false);
        canvas11.gameObject.SetActive(true);
    }

    public void Canva10Button2()
    {
        Debug.Log("Wrong");
        canvas10.gameObject.SetActive(false);
        canvas11.gameObject.SetActive(true);
    }

    public void Canva10Button3()
    {
        Debug.Log("Wrong");
        canvas10.gameObject.SetActive(false);
        canvas11.gameObject.SetActive(true);
    }

    public void Canva10Button4()
    {
        Debug.Log("Correct");
        canvas10.gameObject.SetActive(false);
        canvas11.gameObject.SetActive(true);
    }

    public void Canva11Button1()
    {
        Debug.Log("Correct");
        canvas11.gameObject.SetActive(false);
        canvas12.gameObject.SetActive(true);
    }

    public void Canva11Button2()
    {
        Debug.Log("Wrong");
        canvas11.gameObject.SetActive(false);
        canvas12.gameObject.SetActive(true);
    }

    public void Canva11Button3()
    {
        Debug.Log("Wrong");
        canvas11.gameObject.SetActive(false);
        canvas12.gameObject.SetActive(true);
    }

    public void Canva11Button4()
    {
        Debug.Log("Wrong");
        canvas11.gameObject.SetActive(false);
        canvas12.gameObject.SetActive(true);
    }

    public void Canva12Button1()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva12Button2()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva12Button3()
    {
        Debug.Log("Wrong");
        //finish mini game & deactivate canvas
    }

    public void Canva12Button4()
    {
        Debug.Log("Correct");
        //finish mini game & deactivate canvas
    }
}







