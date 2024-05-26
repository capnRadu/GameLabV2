using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerFinance : MonoBehaviour
{
    WindowGraph windowGraph;
    [NonSerialized] public bool isRoundActive = false;
    [NonSerialized] public bool canTrade = false;
    [NonSerialized] public bool skillStatBonus = true;

    private int currentDay = 0;
    private int funds = 500;
    private int ownedStocks = 0;
    [NonSerialized] public int stockPrice = 50;
    public Color redColor;
    public Color greenColor;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TextMeshProUGUI ownedStocksText;
    public TextMeshProUGUI stockPriceText;
    public Image stockStatusColor;

    [SerializeField] private GameObject results;
    [SerializeField] private TextMeshProUGUI outcomeFundsText;

    private void Start()
    {
        windowGraph = FindObjectOfType<WindowGraph>();

        dayText.text = "Day " + currentDay;
        fundsText.text = "€" + funds;
        ownedStocksText.text = "Owned stocks: " + ownedStocks;
        stockPriceText.text = "Stock price: €" + stockPrice;
        stockStatusColor.color = greenColor;
    }

    private void Update()
    {
        if (!isRoundActive)
        {
            if (currentDay != 3)
            {
                isRoundActive = true;
                currentDay++;
                dayText.text = "Day " + currentDay;
                StartCoroutine(StartNewRound());
            }
            else
            {
                if (ownedStocks > 0)
                {
                    funds += ownedStocks * stockPrice;
                    fundsText.text = "€" + funds;
                    ownedStocks = 0;
                    ownedStocksText.text = "Owned stocks: " + ownedStocks;

                    outcomeFundsText.text = "€" + funds;
                }

                foreach (Transform child in transform)
                {
                    if (child.name != "Background Image" && child.name != "Results")
                    {
                        Destroy(child.gameObject);
                    }
                }

                results.SetActive(true);
            }
        }
    }

    private IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(0.5f);
        windowGraph.NewRound();
    }

    public void BuyStock()
    {
        if (funds >= stockPrice && canTrade)
        {
            funds -= stockPrice;
            ownedStocks++;
            fundsText.text = "€" + funds;
            ownedStocksText.text = "Owned stocks: " + ownedStocks;
        }
    }

    public void SellStock()
    {
        if (ownedStocks > 0 && canTrade)
        {
            if (skillStatBonus)
            {

               funds += stockPrice + (int) stockPrice / 2;
            }
            else
            {
                funds += stockPrice;
            }

            ownedStocks--;
            fundsText.text = "€" + funds;
            ownedStocksText.text = "Owned stocks: " + ownedStocks;
        }
    }
}