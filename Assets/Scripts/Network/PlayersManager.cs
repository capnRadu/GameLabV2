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
    public void SetPlayerNameClientRpc(string name, ulong clientId)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == clientId)
            {
                player.GetComponent<PlayerSkills>().playerName = name;
                Debug.Log("Player name is " + name);
            }
        }
    }

    [ClientRpc]
    public void NextPlayerClientRpc()
    {
        currentPlayer = players[(System.Array.IndexOf(players, currentPlayer) + 1) % players.Length];
    }

    [ClientRpc]
    public void UpdateCurrentNodeClientRpc(NetworkBehaviourReference mapNodeReference)
    {
        if (mapNodeReference.TryGet<MapNode>(out MapNode mapNode))
        {
            currentPlayer.GetComponent<GamePiece>().currentNode = mapNode;
        }
        else
        {
            Debug.LogError("Failed to get MapNode");
        }
    }

    [ClientRpc]
    public void UpdateOfficeOwnershipClientRpc()
    {
        currentPlayer.GetComponent<GamePiece>().currentNode.GetComponent<Office>().owningPlayer = currentPlayer.GetComponent<PlayerSkills>().playerName;
        Debug.Log("Office ownership updated");
    }
}