using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    ManagerFinance managerFinance;

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Sprite circleSpriteWhite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;

    private bool graphSetup = false;

    private void Awake()
    {
        managerFinance = FindObjectOfType<ManagerFinance>();

        graphContainer = transform.Find("Graph Container").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("Label Template X").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("Label Template Y").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("Dash Template X").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("Dash Template Y").GetComponent<RectTransform>();
    }

    public void NewRound()
    {
        List<int> valueList = new List<int>();
        valueList.Add(50);

        for (int i = 1; i < 19; i++)
        {
            int newValue = valueList[i - 1] + UnityEngine.Random.Range(-30, 30);
            newValue = Mathf.Clamp(newValue, 0, 100);
            valueList.Add(newValue);
        }

        ShowGraph(valueList, (int _i) => "" + (_i + 1), (float _f) => "€" + Mathf.RoundToInt(_f));
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        
        return gameObject;
    }

    private void ShowGraph(List<int> valueList, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 50f;

        if (!graphSetup)
        {
            if (getAxisLabelX == null)
            {
                getAxisLabelX = delegate (int _i) { return _i.ToString(); };
            }

            if (getAxisLabelY == null)
            {
                getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
            }

            int separatorCountY = 10;
            for (int i = 0; i <= separatorCountY; i++)
            {
                RectTransform labelY = Instantiate(labelTemplateY);
                labelY.SetParent(graphContainer, false);
                labelY.gameObject.SetActive(true);
                float normalizedValue = i * 1f / separatorCountY;
                labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
                labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);

                RectTransform dashY = Instantiate(dashTemplateY);
                dashY.SetParent(graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            }

            int separatorCountX = valueList.Count;
            for (int i = 0; i < separatorCountX; i++)
            {
                float xPosition = xSize + i * xSize;

                RectTransform labelX = Instantiate(labelTemplateX);
                labelX.SetParent(graphContainer, false);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -7f);
                labelX.GetComponent<Text>().text = getAxisLabelX(i);

                RectTransform dashX = Instantiate(dashTemplateX);
                dashX.SetParent(graphContainer, false);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(xPosition, -7f);
            }

            graphSetup = true;
        }

        StartCoroutine(PlotGraph(valueList, xSize, yMaximum, graphHeight));
    }

    private IEnumerator PlotGraph(List<int> valueList, float xSize, float yMaximum, float graphHeight)
    {
        managerFinance.canTrade = true;

        GameObject lastCircleGameObject = null;

        for (int i = 0; i < valueList.Count; i++)
        {
            managerFinance.stockPrice = valueList[i];
            managerFinance.stockPriceText.text = "Stock price: €" + valueList[i];

            if (i != 0)
            {
                managerFinance.stockStatusColor.color = valueList[i] >= valueList[i - 1] ? managerFinance.greenColor : managerFinance.redColor;
            }

            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                lastCircleGameObject.GetComponent<Image>().sprite = circleSpriteWhite;
            }

            lastCircleGameObject = circleGameObject;

            yield return new WaitForSeconds(0.5f);
        }

        managerFinance.canTrade = false;

        yield return new WaitForSeconds(1f);

        foreach (Transform child in graphContainer)
        {
            if (child.name == "circle" || child.name == "dotConnection")
            {
                Destroy(child.gameObject);
            }
        }

        managerFinance.isRoundActive = false;
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin  = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    private float GetAngleFromVectorFloat(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}