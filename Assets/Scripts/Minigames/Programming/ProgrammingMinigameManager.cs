using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;
using System;

public class ProgrammingMinigameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> questions = new List<GameObject>();
    [SerializeField] private List<GameObject> shuffledQuestions = new List<GameObject>();
    private int currentQuestionIndex = 0;
    private int rounds = 3;
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

    private void Awake()
    {
        shuffledQuestions = questions.OrderBy(x => UnityEngine.Random.value).ToList();

        minigamesManager = minigamesPanel.GetComponent<MinigamesManager>();
        if (minigamesManager.playerPrimarySkills.Contains("Programming"))
        {
            skillStatBonus = true;
            specialAbilityPanel.SetActive(true);
        }
    }

    private void OnEnable()
    {
        currentRound = 1;
        obtainedPoints = 0;

        roundsText.text = $"Round {currentRound}/{rounds}";
        pointsText.text = $"Points: {obtainedPoints}";

        infoPanel.SetActive(true);
        roundsText.gameObject.SetActive(false);
        pointsText.gameObject.SetActive(false);
        pointsWonPanel.SetActive(false);

        StartCoroutine(HideInfoPanel());
    }

    private IEnumerator HideInfoPanel()
    {
        yield return new WaitForSeconds(5f);

        infoPanel.SetActive(false);
        roundsText.gameObject.SetActive(true);
        pointsText.gameObject.SetActive(true);

        shuffledQuestions[currentQuestionIndex].SetActive(true);
        
        if (skillStatBonus && totalPoints == -1)
        {
            SkillStatBonus(shuffledQuestions[currentQuestionIndex]);
        }
    }

    public void ChooseAnswer(bool correctAnswer)
    {
        if (correctAnswer)
        {
            Debug.Log("Correct answer!");
            obtainedPoints += 15;
            pointsText.text = $"Points: {obtainedPoints}";
        }
        else
        {
            Debug.Log("Incorrect answer!");
        }

        shuffledQuestions[currentQuestionIndex].SetActive(false);

        if (currentQuestionIndex < shuffledQuestions.Count - 1)
        {
            currentQuestionIndex++;
            shuffledQuestions[currentQuestionIndex].SetActive(true);
        }
        else
        {
            currentQuestionIndex = 0;
            shuffledQuestions = questions.OrderBy(x => UnityEngine.Random.value).ToList();
            shuffledQuestions[currentQuestionIndex].SetActive(true);
        }

        if (skillStatBonus)
        {
            SkillStatBonus(shuffledQuestions[currentQuestionIndex]);
        }

        if (currentRound < rounds)
        {
            currentRound++;
            roundsText.text = $"Round {currentRound}/{rounds}";
        }
        else
        {
            Debug.Log("All questions answered!");
            shuffledQuestions[currentQuestionIndex].SetActive(false);

            roundsText.gameObject.SetActive(false);
            pointsText.gameObject.SetActive(false);
            specialAbilityPanel.SetActive(false);
            pointsWonPanel.SetActive(true);

            totalPoints = obtainedPoints + obtainedPoints * minigamesManager.playerSkills.skills["Programming"] / minigamesManager.playerSkills.GetTotalAttributePoints();
            totalPoints = Mathf.Round(totalPoints * 10.0f) * 0.1f;
            pointsWonText.text = $"{obtainedPoints} + {obtainedPoints} x {minigamesManager.playerSkills.skills["Programming"]} (Programming attribute points) / {minigamesManager.playerSkills.GetTotalAttributePoints()} (Total attribute points) = {totalPoints}";

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
        
        gameObject.SetActive(false);
    }

    private void SkillStatBonus(GameObject question)
    {
        foreach (Transform child in question.transform)
        {
            child.gameObject.SetActive(true);
        }

        List<AnswerButton> answerButtons = question.GetComponentsInChildren<AnswerButton>().Where(x => x.correctAnswer == false).ToList();

        int randomIndex = UnityEngine.Random.Range(0, answerButtons.Count);
        int randomIndex2 = randomIndex;

        do
        {
            randomIndex2 = UnityEngine.Random.Range(0, answerButtons.Count);
        }
        while (randomIndex2 == randomIndex);

        answerButtons[randomIndex].gameObject.SetActive(false);
        answerButtons[randomIndex2].gameObject.SetActive(false);
    }
}