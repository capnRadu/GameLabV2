using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerQA : MonoBehaviour
{
    [SerializeField] private List<Image> imagesDefault = new List<Image>();
    [SerializeField] private List<Image> imagesShuffle = new List<Image>();
    private int currentImageIndex = 0;
    private int rounds = 3;
    private int currentRound = 1;

    [SerializeField] private Button hintButton;
    [NonSerialized] public bool skillStatBonus = false;
    [NonSerialized] public int hints = 3;

    MinigamesManager minigamesManager;
    [SerializeField] public GameObject minigamesPanel;

    [SerializeField] private GameObject specialAbilityPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject roundDifferencesPanel;
    [SerializeField] private TextMeshProUGUI round;
    public TextMeshProUGUI differences;

    [SerializeField] private GameObject skipButton;

    private void Awake()
    {
        imagesShuffle = imagesDefault.OrderBy(x => UnityEngine.Random.value).ToList();

        minigamesManager = minigamesPanel.GetComponent<MinigamesManager>();
        if (minigamesManager.playerPrimarySkills.Contains("Quality Assurance"))
        {
            skillStatBonus = true;
            specialAbilityPanel.SetActive(true);
        }
    }

    public void OnEnable()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Tap(Clone)")
            {
                Destroy(child.gameObject);
            }
        }

        hints = 3;

        currentRound = 1;
        round.text = $"Round {currentRound}/{rounds}";

        infoPanel.SetActive(true);
        hintButton.gameObject.SetActive(false);
        skipButton.SetActive(false);
        roundDifferencesPanel.SetActive(false);

        StartCoroutine(HideInfo());
    }

    private IEnumerator HideInfo()
    {
        yield return new WaitForSeconds(5f);

        if (skillStatBonus)
        {
            hintButton.gameObject.SetActive(true);
            hintButton.GetComponentInChildren<TextMeshProUGUI>().text = $"HINT ({hints})";
        }

        infoPanel.SetActive(false);
        skipButton.SetActive(true);
        roundDifferencesPanel.SetActive(true);

        imagesShuffle[currentImageIndex].gameObject.SetActive(true);
    }

    public void NextRound()
    {
        rounds--;
        currentRound++;

        round.text = $"Round {currentRound}/3";

        if (currentImageIndex != imagesShuffle.Count - 1)
        {
            currentImageIndex++;

            if (rounds > 0)
            {
                imagesShuffle[currentImageIndex].gameObject.SetActive(true);
            }
        }
        else
        {
            currentImageIndex = 0;

            if  (rounds > 0)
            {
                imagesShuffle = imagesDefault.OrderBy(x => UnityEngine.Random.value).ToList();
                imagesShuffle[currentImageIndex].gameObject.SetActive(true);
            }
        }

        if (rounds == 0)
        {
            rounds = 3;
            gameObject.SetActive(false);
        }
    }

    public void GetHint()
    {
        if (hints > 0)
        {
            var randomButton = imagesShuffle[currentImageIndex].transform.GetChild(UnityEngine.Random.Range(0, imagesShuffle[currentImageIndex].transform.childCount)).GetComponent<Button>();

            if (randomButton.interactable)
            {
                randomButton.onClick.Invoke();
                hints--;
                hintButton.GetComponentInChildren<TextMeshProUGUI>().text = $"HINT ({hints})";
            }
            else
            {
                GetHint();
            }
        }
    }

    public void SkipImage()
    {
       imagesShuffle[currentImageIndex].gameObject.SetActive(false);
       NextRound();
    }
}