using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductManagementManager : MonoBehaviour
{
    private int rounds = 2;
    private int currentRound = 1;

    MinigamesManager minigamesManager;
    [SerializeField] public GameObject minigamesPanel;

    [NonSerialized] public bool skillStatBonus = false;
    [SerializeField] private GameObject specialAbilityPanel;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI roundsText;
    [SerializeField] private TextMeshProUGUI pointsText;
    private float obtainedPoints = 0;
    private float totalPoints = -1;

    [SerializeField] private GameObject pointsWonPanel;
    [SerializeField] private TextMeshProUGUI pointsWonText;

    [SerializeField] private GameObject productManagementMinigameBackground;
    [SerializeField] private GameObject minigameTiles;

    [SerializeField] private GameObject skipButton;

    private Camera minigameCamera;
    private Camera mainCamera;
    [SerializeField] private GameObject hudCanvas;

    BlockPuzzleScript blockPuzzleScript;

    private void Awake()
    {
        minigamesManager = minigamesPanel.GetComponent<MinigamesManager>();
        if (minigamesManager.playerPrimarySkills.Contains("Product Management"))
        {
            skillStatBonus = true;
            specialAbilityPanel.SetActive(true);
        }

        blockPuzzleScript = GetComponent<BlockPuzzleScript>();
        minigameCamera = blockPuzzleScript._camera;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(EndGame());
        }
    }

    private void OnEnable()
    {
        mainCamera.gameObject.SetActive(false);
        minigameCamera.enabled = true;
        hudCanvas.SetActive(false);

        currentRound = 1;
        obtainedPoints = 0;

        roundsText.text = $"Round {currentRound}/{rounds}";
        pointsText.text = $"Points: {obtainedPoints}";

        infoPanel.SetActive(true);
        pointsWonPanel.SetActive(false);

        StartCoroutine(HideInfoPanel());
    }

    private IEnumerator HideInfoPanel()
    {
        yield return new WaitForSeconds(5f);

        infoPanel.SetActive(false);
        roundsText.gameObject.SetActive(true);
        pointsText.gameObject.SetActive(true);
        skipButton.SetActive(true);
        productManagementMinigameBackground.SetActive(true);
        minigameTiles.SetActive(true);
        blockPuzzleScript.Shuffle();
    }

    public void NextRound()
    {
        if (currentRound < rounds)
        {
            currentRound++;
            roundsText.text = $"Round {currentRound}/{rounds}";
            blockPuzzleScript.Shuffle();
        }
        else
        {
            productManagementMinigameBackground.SetActive(false);
            minigameTiles.SetActive(false);
            roundsText.gameObject.SetActive(false);
            pointsText.gameObject.SetActive(false);
            specialAbilityPanel.SetActive(false);
            skipButton.SetActive(false);

            pointsWonPanel.SetActive(true);

            totalPoints = obtainedPoints + obtainedPoints * minigamesManager.playerSkills.skills["Product Management"] / minigamesManager.playerSkills.GetTotalAttributePoints();
            totalPoints = Mathf.Round(totalPoints * 10.0f) * 0.1f;
            pointsWonText.text = $"{obtainedPoints} + {obtainedPoints} x {minigamesManager.playerSkills.skills["Product Management"]} (Product Management attribute points) / {minigamesManager.playerSkills.GetTotalAttributePoints()} (Total attribute points) = {totalPoints}";

            minigamesManager.playerSkills.UpdateMinigamesPointsServerRpc(totalPoints);

            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5f);

        if (skillStatBonus)
        {
            specialAbilityPanel.SetActive(true);
        }

        minigameCamera.enabled = false;
        mainCamera.gameObject.SetActive(true);
        hudCanvas.SetActive(true);

        gameObject.SetActive(false);
    }

    public void AddPoints()
    {
        if (skillStatBonus)
        {
            obtainedPoints += 10;
        }
        else
        {
            obtainedPoints += 5;
        }

        pointsText.text = $"Points: {obtainedPoints}";
    }
}
