using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ManagerQA : MonoBehaviour
{
    [SerializeField] private List<Image> imagesDefault = new List<Image>();
    [SerializeField] private List<Image> imagesShuffle = new List<Image>();
    private int currentImageIndex = 0;
    private int rounds = 3;

    [SerializeField] private Button hintButton;
    [NonSerialized] public bool skillStatBonus = false;
    [NonSerialized] public int hints = 3;

    MinigamesManager minigamesManager;
    [SerializeField] public GameObject minigamesPanel;

    private void Awake()
    {
        imagesShuffle = imagesDefault.OrderBy(x => UnityEngine.Random.value).ToList();

        minigamesManager = minigamesPanel.GetComponent<MinigamesManager>();
        if (minigamesManager.playerPrimarySkills.Contains("Quality Assurance"))
        {
            skillStatBonus = true;
            hintButton.gameObject.SetActive(true);
        }
    }

    public void OnEnable()
    {
        imagesShuffle[currentImageIndex].gameObject.SetActive(true);
    }

    public void NextRound()
    {
        rounds--;

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
            }
            else
            {
                GetHint();
            }
        }
    }
}