using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotTheDifferences : MonoBehaviour
{
    ManagerQA minigameManager;

    [SerializeField] private Button[] differenceButtons;
    [NonSerialized] public int totalDifferences;
    [NonSerialized] public int foundDifferences = 0;

    private void Awake()
    {
        minigameManager = FindObjectOfType<ManagerQA>();
    }

    private void OnEnable()
    {
        foundDifferences = 0;
        totalDifferences = differenceButtons.Length / 2;

        foreach (Button difference in differenceButtons)
        {
            difference.onClick.AddListener(() => OnDifferenceFound(difference));
        }

        minigameManager.differences.text = $"{foundDifferences}/{totalDifferences}";
    }

    private void OnDifferenceFound(Button difference)
    {
        if (Input.touchCount != 2)
        {
            difference.interactable = false;
            difference.GetComponent<Animator>().enabled = true;

            if (difference.transform.parent.name == difference.name)
            {
                difference.transform.parent.GetComponent<Button>().interactable = false;
                difference.transform.parent.GetComponent<Animator>().enabled = true;
            }
            else if (difference.transform.GetChild(0).name == difference.name)
            {
                difference.transform.GetChild(0).GetComponent<Button>().interactable = false;
                difference.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            }

            foundDifferences++;
            Debug.Log("Difference found! " + foundDifferences + " out of " + totalDifferences + " found.");

            minigameManager.differences.text = $"{foundDifferences}/{totalDifferences}";

            minigameManager.obtainedPoints += 5;
            minigameManager.obtainedPointsText.text = "Points: " + minigameManager.obtainedPoints;

            if (foundDifferences == totalDifferences)
            {
                Debug.Log("All differences found!");
                StartCoroutine(StartNextRound());
            }
        }
    }

    private IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        minigameManager.NextRound();
    }
}