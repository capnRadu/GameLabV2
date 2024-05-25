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

    [SerializeField] private Button hintButton;
    [NonSerialized] public bool skillStatBonus = true;
    [NonSerialized] public int hints = 3;

    private void Start()
    {
        imagesShuffle = imagesDefault.OrderBy( x => UnityEngine.Random.value).ToList();
        imagesShuffle[currentImageIndex].gameObject.SetActive(true);

        if (skillStatBonus)
        {
            hintButton.gameObject.SetActive(true);
        }
    }

    public void NextRound()
    {
        if (currentImageIndex != imagesShuffle.Count - 1)
        {
            currentImageIndex++;
            imagesShuffle[currentImageIndex].gameObject.SetActive(true);
        }
        else
        {
            currentImageIndex = 0;
            imagesShuffle = imagesDefault.OrderBy(x => UnityEngine.Random.value).ToList();
            imagesShuffle[currentImageIndex].gameObject.SetActive(true);
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