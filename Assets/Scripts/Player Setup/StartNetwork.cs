using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class StartNetwork : MonoBehaviour
{
    [SerializeField] private GameObject networkPanel;
    [SerializeField] private GameObject fadeImage;

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        BeginGame();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        BeginGame();
    }

    private void BeginGame()
    {
        networkPanel.SetActive(false);
        fadeImage.GetComponent<Animator>().enabled = true;
    }
}
