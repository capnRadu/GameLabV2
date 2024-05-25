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

    private void Start()
    {
        imagesShuffle = imagesDefault.OrderBy( x => Random.value).ToList();
        imagesShuffle[currentImageIndex].gameObject.SetActive(true);
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
            imagesShuffle = imagesDefault.OrderBy(x => Random.value).ToList();
            imagesShuffle[currentImageIndex].gameObject.SetActive(true);
        }
    }
}
