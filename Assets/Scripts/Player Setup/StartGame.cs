using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject fadeImage;

    public void BeginGame()
    {
        GetComponent<Animator>().enabled = true;
        fadeImage.GetComponent<Animator>().enabled = true;
    }
}
