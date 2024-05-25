using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour
{
    private float zoomSens = 20f;

    private void Update()
    {
        if (Input.touchCount != 2)
        {
            return;
        }
        else
        {
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0Prev - touch1Prev).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            GameObject currentImage = GameObject.FindGameObjectWithTag("Image QA");

            if (deltaMagnitudeDiff > 0.7f)
            {
                if (currentImage && currentImage.transform.localScale.x > 0.8f && currentImage.transform.localScale.y > 0.8f && currentImage.transform.localScale.z > 0.8f)
                {
                    currentImage.transform.localScale -= Time.deltaTime * zoomSens * new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
            else if (deltaMagnitudeDiff < -0.7f)
            {
                if (currentImage && currentImage.transform.localScale.x < 2.5f && currentImage.transform.localScale.y < 2.5f && currentImage.transform.localScale.z < 2.5f)
                {
                    currentImage.transform.localScale += Time.deltaTime * zoomSens * new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }
    }
}