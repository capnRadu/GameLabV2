using Unity.Netcode;
using UnityEngine;

public class PlayersManager : NetworkBehaviour
{
    public static PlayersManager Instance;

    public GameObject[] players;
    public GameObject currentPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    [ClientRpc]
    public void NextPlayerClientRpc()
    {
        currentPlayer = players[(System.Array.IndexOf(players, currentPlayer) + 1) % players.Length];
    }
}