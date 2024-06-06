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
    private float totalPoints = 0;
    [SerializeField] private TextMeshProUGUI totalPointsText;

    MinigamesManager minigamesManager;
    [SerializeField] public GameObject minigamesPanel;

    [SerializeField] private GameObject specialAbilityPanel;
    [SerializeField] private GameObject infoPanel;

    private float obtainedPoints = 0;
    [SerializeField] private TextMeshProUGUI obtainedPointsText;
    private bool updatePoints = false;

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
        obtainedPoints = 0;
        updatePoints = false;

        dayText.text = $"Day {currentDay}/5";
        fundsText.text = "€" + funds;
        ownedStocksText.text = "Owned stocks: " + ownedStocks;
        stockPriceText.text = "Stock price: €" + stockPrice;
        stockStatusColor.color = greenColor;
        obtainedPointsText.text = "Points: " + obtainedPoints;

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

                if (!updatePoints)
                {
                    updatePoints = true;

                    outcomeFundsText.text = "€" + funds;

                    totalPoints = obtainedPoints + obtainedPoints * minigamesManager.playerSkills.skills["Finance"] / minigamesManager.playerSkills.GetTotalAttributePoints();
                    totalPoints = Mathf.Round(totalPoints * 10.0f) * 0.1f;
                    totalPointsText.text = $"Points Won\r\n{obtainedPoints} + {obtainedPoints} x {minigamesManager.playerSkills.skills["Finance"]} (Finance attribute points) / {minigamesManager.playerSkills.GetTotalAttributePoints()} (Total attribute points) = {totalPoints}";

                    minigamesManager.playerSkills.UpdateMinigamesPointsServerRpc(totalPoints);
                }

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
        yield return new WaitForSeconds(5f);
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

            if (stockStatusColor.color == greenColor)
            {
                obtainedPoints += 5;
                obtainedPointsText.text = "Points: " + obtainedPoints;
            }

            ownedStocks--;
            fundsText.text = "€" + funds;
            ownedStocksText.text = "Owned stocks: " + ownedStocks;
        }
    }
}