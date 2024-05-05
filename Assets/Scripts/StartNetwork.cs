using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class StartNetwork : MonoBehaviour
{
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private GameObject waitingText;

    private void Update()
    {
        if (PlayersManager.Instance.players.Length > 1)
        {
            PlayersManager.Instance.currentPlayer = PlayersManager.Instance.players[0];
            BeginGame();
        }
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        waitingText.SetActive(true);
    }

    private void BeginGame()
    {
        fadeImage.GetComponent<Animator>().enabled = true;
        waitingText.SetActive(false);
        gameObject.SetActive(false);
    }
}
