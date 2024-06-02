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
    [NonSerialized] public bool skillStatBonus = false;

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

    MinigamesManager minigamesManager;
    [SerializeField] public GameObject minigamesPanel;

    private void Awake()
    {
        windowGraph = FindObjectOfType<WindowGraph>();

        minigamesManager = minigamesPanel.GetComponent<MinigamesManager>();
        if (minigamesManager.playerPrimarySkills.Contains("Finance"))
        {
            skillStatBonus = true;
        }
    }

    private void OnEnable()
    {
        isRoundActive = false;
        canTrade = false;

        currentDay = 0;
        funds = 500;
        ownedStocks = 0;
        stockPrice = 50;

        dayText.text = "Day " + currentDay;
        fundsText.text = "€" + funds;
        ownedStocksText.text = "Owned stocks: " + ownedStocks;
        stockPriceText.text = "Stock price: €" + stockPrice;
        stockStatusColor.color = greenColor;

        foreach (Transform child in transform)
        {
            if (child.name != "Background Image" && child.name != "Results")
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (!isRoundActive)
        {
            if (currentDay != 2)
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
                }

                foreach (Transform child in transform)
                {
                    if (child.name != "Background Image" && child.name != "Results")
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                outcomeFundsText.text = "€" + funds;

                results.SetActive(true);
                StartCoroutine(EndGame());
            }
        }
    }

    private IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(0.5f);
        windowGraph.NewRound();
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3f);
        results.SetActive(false);
        gameObject.SetActive(false);
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