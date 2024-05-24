using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersManager : NetworkBehaviour
{
    public static PlayersManager Instance;

    public GameObject[] players;
    public GameObject currentPlayer;
    public int clientIndex = -1;
    private bool indexCheck = false;

    private int gameTurns = 5;
    private float maxPoints = 0;
    private string winner = null;

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
        if (players.Length != 4) 
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length > 0 && clientIndex == -1)
            {
                clientIndex = players.Length - 1;
                currentPlayer = players[0];
            }
        }
        else if (clientIndex == 2 && !indexCheck)
        {
            (players[clientIndex], players[clientIndex - 1]) = (players[clientIndex - 1], players[clientIndex]);
            indexCheck = true;
        }
        else if (clientIndex == 3 && !indexCheck)
        {
            (players[clientIndex], players[clientIndex - 2]) = (players[clientIndex - 2], players[clientIndex]);
            indexCheck = true;
        }
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

        if (System.Array.IndexOf(players, currentPlayer) % players.Length == 0 && gameTurns > 0)
        {
            gameTurns--;
            Debug.Log(gameTurns);
        }
        
        if (gameTurns == 0)
        {
            foreach (GameObject player in players)
            {
                PlayerSkills playerSkills = player.GetComponent<PlayerSkills>();
                PlayerInputAdvanced playerInputAdvanced = player.GetComponent<PlayerInputAdvanced>();

                float skillPoints = 0;

                foreach (string skill in playerSkills.skills.Keys)
                {
                    if (!playerSkills.primarySkills.Contains(skill))
                    {
                        skillPoints += playerSkills.skills[skill];
                    }
                }

                Debug.LogWarning($"{playerInputAdvanced.employees}  {playerInputAdvanced.coins}  {skillPoints}");

                float totalPoints = 0.5f * playerInputAdvanced.employees + 0.35f * skillPoints + 0.15f * playerInputAdvanced.coins;
                Debug.Log(playerSkills.playerName + " has " + totalPoints + " points");

                if (totalPoints > maxPoints)
                {
                    maxPoints = totalPoints;
                    winner = playerSkills.playerName;
                }
            }

            Debug.Log("The winner is " + winner);
        }
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

    [ClientRpc]
    public void UpdatePlayerCoinsClientRpc(int coins, ulong clientId)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == clientId)
            {
                player.GetComponent<PlayerInputAdvanced>().coins = coins;

                Debug.Log(player.GetComponent<PlayerSkills>().playerName + " has " + player.GetComponent<PlayerInputAdvanced>().coins + " coins");
            }
        }
    }

    [ClientRpc]
    public void UpdatePlayerEmployeesClientRpc(int employees, ulong clientId)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == clientId)
            {
                player.GetComponent<PlayerInputAdvanced>().employees = employees;

                Debug.Log(player.GetComponent<PlayerSkills>().playerName + " has " + player.GetComponent<PlayerInputAdvanced>().employees + " employees");
            }
        }
    }

    [ClientRpc]
    public void UpdatePlayerAttributesClientRpc(string playerSkill, int skillAttributes, ulong clientId)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == clientId)
            {
                player.GetComponent<PlayerSkills>().skills[playerSkill] = skillAttributes;

                Debug.Log(player + " has " + player.GetComponent<PlayerSkills>().skills[playerSkill] + " " + playerSkill);
            }
        }
    }

    [ClientRpc]
    public void UpdatePlayerPrimarySkillsClientRpc(string primarySkill, ulong clientId)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == clientId)
            {
                if (!player.GetComponent<PlayerSkills>().primarySkills.Contains(primarySkill))
                {
                    player.GetComponent<PlayerSkills>().primarySkills.Add(primarySkill);

                    Debug.Log(player + " has " + primarySkill + " as a primary skill");
                }
            }
        }
    }
}