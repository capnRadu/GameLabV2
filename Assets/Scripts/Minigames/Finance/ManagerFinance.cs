using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerFinance : MonoBehaviour
{
    WindowGraph windowGraph;
    [SerializeField] private GameObject windowGraphObject;

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

    [SerializeField] private GameObject specialAbilityPanel;
    [SerializeField] private GameObject infoPanel;

    private void Awake()
    {
        windowGraph = windowGraphObject.GetComponent<WindowGraph>();

        minigamesManager = minigamesPanel.GetComponent<MinigamesManager>();
        if (minigamesManager.playerPrimarySkills.Contains("Finance"))
        {
            skillStatBonus = true;
            specialAbilityPanel.SetActive(true);
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

        dayText.text = $"Day {currentDay}/5";
        fundsText.text = "€" + funds;
        ownedStocksText.text = "Owned stocks: " + ownedStocks;
        stockPriceText.text = "Stock price: €" + stockPrice;
        stockStatusColor.color = greenColor;

        StartCoroutine(HideInfo());
    }

    private void Update()
    {
        if (!isRoundActive && !infoPanel.activeSelf)
        {
            if (currentDay != 5)
            {
                isRoundActive = true;
                currentDay++;
                dayText.text = $"Day {currentDay}/5";
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
                    if (child.name != "Background Image")
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

    private IEnumerator HideInfo()
    {
        yield return new WaitForSeconds(6f);

        foreach (Transform child in transform)
        {
            if (child.name != "Results" && child.name != specialAbilityPanel.name && child.name != infoPanel.name)
            {
                child.gameObject.SetActive(true);
            }
            else if (child.name == infoPanel.name)
            {
                child.gameObject.SetActive(false);
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
        infoPanel.SetActive(true);

        if (skillStatBonus)
        {
            specialAbilityPanel.SetActive(true);
        }

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