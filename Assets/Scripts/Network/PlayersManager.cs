using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayersManager : NetworkBehaviour
{
    public static PlayersManager Instance;

    public GameObject[] players;
    public GameObject currentPlayer;
    public int clientIndex = -1;
    private bool indexCheck = false;
    [SerializeField] private int numberOfPlayers = 4;
    public List<MapNode> startingNodes = new List<MapNode>();

    private int gameTurns = 15;
    public TextMeshProUGUI roundsLeftText;

    MinigamesManager minigamesManager;

    public GameObject minigameVotingPanel;
    public TextMeshProUGUI financeTotalVotesText;
    public TextMeshProUGUI qaTotalVotesText;
    public TextMeshProUGUI programmingTotalVotesText;
    public TextMeshProUGUI productManagementTotalVotesText;

    public GameObject financeMinigameCanvas;
    public GameObject qaMinigameCanvas;
    public GameObject programmingMinigameCanvas;
    public GameObject productManagementMinigameObject;

    public GameObject leaderboardPanel;
    public GameObject playerPanelPrefab;
    private float posY = 253f;
    private int spacing = 170;

    private struct PlayerStats
    {
        public string playerName;
        public float minigamesPoints;
        public float nonPrimaryAttributePoints;
        public float totalPoints;
    }

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

    private void Start()
    {
        minigamesManager = minigameVotingPanel.GetComponent<MinigamesManager>();

        roundsLeftText.text = "Rounds Left: " + gameTurns;
    }

    private void Update()
    {
        if (players.Length != numberOfPlayers) 
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length > 0 && clientIndex == -1)
            {
                clientIndex = players.Length - 1;
                currentPlayer = players[0];
                players[clientIndex].GetComponent<GamePiece>().currentNode = startingNodes[clientIndex];
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
            roundsLeftText.text = "Rounds Left: " + gameTurns;
            minigameVotingPanel.SetActive(true);
            Debug.Log(gameTurns);
        }
        
        if (gameTurns == 0)
        {
            leaderboardPanel.SetActive(true);

            List<PlayerStats> playerStats = new List<PlayerStats>();

            foreach (GameObject player in players)
            {
                PlayerSkills playerSkills = player.GetComponent<PlayerSkills>();

                float skillPoints = 0;

                foreach (string skill in playerSkills.skills.Keys)
                {
                    if (!playerSkills.primarySkills.Contains(skill))
                    {
                        skillPoints += playerSkills.skills[skill];
                    }
                }

                float totalPoints = 0.5f * skillPoints + 0.5f * playerSkills.minigamesPoints;
                totalPoints = Mathf.Round(totalPoints * 10.0f) * 0.1f;

                PlayerStats stats = new PlayerStats
                {
                    playerName = playerSkills.playerName,
                    minigamesPoints = playerSkills.minigamesPoints,
                    nonPrimaryAttributePoints = skillPoints,
                    totalPoints = totalPoints
                };

                playerStats.Add(stats);
            }

            playerStats = playerStats.OrderByDescending(player => player.totalPoints).ToList();

            foreach (PlayerStats player in playerStats)
            {
                GameObject playerPanel = Instantiate(playerPanelPrefab, new Vector3(0, posY, 0), Quaternion.identity);
                playerPanel.transform.SetParent(leaderboardPanel.transform);
                playerPanel.transform.localPosition = new Vector3(0, posY, 0);
                playerPanel.transform.localScale = new Vector3(1, 1, 1);
                playerPanel.transform.localRotation = Quaternion.identity;

                int ranking = playerStats.IndexOf(player) + 1;

                playerPanel.transform.Find("Ranking Text").GetComponent<TextMeshProUGUI>().text = $"{ranking}) {player.playerName} : {player.totalPoints} final points";
                playerPanel.transform.Find("Minigames Points Text").GetComponent<TextMeshProUGUI>().text = $"Minigames points (50%): {player.minigamesPoints}";
                playerPanel.transform.Find("Attribute Points Text").GetComponent<TextMeshProUGUI>().text = $"Total non-primary attribute points (50%): {player.nonPrimaryAttributePoints}";

                posY -= spacing;
            }
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

    [ClientRpc]
    public void VoteMinigameClientRpc(string minigame, ulong clientId)
    {
        GameObject invokingPlayer = null;

        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == clientId)
            {
                invokingPlayer = player;
            }
        }

        if (minigamesManager.financeVotes + minigamesManager.qaVotes + minigamesManager.programmingVotes + minigamesManager.productManagementVotes != players.Length && !invokingPlayer.GetComponent<PlayerInputAdvanced>().hasVoted)
        {
            switch (minigame)
            {
                case "finance":
                    minigamesManager.financeVotes++;
                    financeTotalVotesText.text = $"Vote ({minigamesManager.financeVotes} votes)";
                    Debug.Log(invokingPlayer.GetComponent<PlayerSkills>().playerName + " voted for Finance minigame");
                    break;
                case "qa":
                    minigamesManager.qaVotes++;
                    qaTotalVotesText.text = $"Vote ({minigamesManager.qaVotes} votes)";
                    Debug.Log(invokingPlayer.GetComponent<PlayerSkills>().playerName + " voted for QA minigame");
                    break;
                case "programming":
                    minigamesManager.programmingVotes++;
                    programmingTotalVotesText.text = $"Vote ({minigamesManager.programmingVotes} votes)";
                    Debug.Log(invokingPlayer.GetComponent<PlayerSkills>().playerName + " voted for Programming minigame");
                    break;
                case "product management":
                    minigamesManager.productManagementVotes++;
                    productManagementTotalVotesText.text = $"Vote ({minigamesManager.productManagementVotes} votes)";
                    Debug.Log(invokingPlayer.GetComponent<PlayerSkills>().playerName + " voted for Product Management minigame");
                    break;
            }

            invokingPlayer.GetComponent<PlayerInputAdvanced>().hasVoted = true;
        }

        if (minigamesManager.financeVotes + minigamesManager.qaVotes + minigamesManager.programmingVotes + minigamesManager.productManagementVotes == players.Length && invokingPlayer.GetComponent<PlayerInputAdvanced>().hasVoted)
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerInputAdvanced>().hasVoted = false;
            }

            int selectedMinigameIndex = DetermineMinigameIndex();
            StartMinigameClientRpc(selectedMinigameIndex);
        }
    }

    private int DetermineMinigameIndex()
    {
        int[] votes = { minigamesManager.financeVotes, minigamesManager.qaVotes, minigamesManager.programmingVotes, minigamesManager.productManagementVotes };
        int maxVotes = votes.Max();

        List<int> indicesWithMaxVotes = new List<int>();

        for (int i = 0; i < votes.Length; i++)
        {
            if (votes[i] == maxVotes)
            {
                indicesWithMaxVotes.Add(i);
            }
        }

        if (indicesWithMaxVotes.Count > 1)
        {
            return indicesWithMaxVotes[Random.Range(0, indicesWithMaxVotes.Count)];
        }

        return indicesWithMaxVotes[0];
    }

    [ClientRpc]
    private void StartMinigameClientRpc(int minigameIndex)
    {
        switch (minigameIndex)
        {
            case 0:
                StartCoroutine(StartMinigame(financeMinigameCanvas));
                break;
            case 1:
                StartCoroutine(StartMinigame(qaMinigameCanvas));
                break;
            case 2:
                StartCoroutine(StartMinigame(programmingMinigameCanvas));
                break;
            case 3:
                StartCoroutine(StartMinigame(productManagementMinigameObject));
                break;
        }
    }

    private IEnumerator StartMinigame(GameObject minigameCanvas)
    {
        yield return new WaitForSeconds(2);

        minigameCanvas.SetActive(true);

        financeTotalVotesText.text = "Vote (0 votes)";
        qaTotalVotesText.text = "Vote (0 votes)";
        programmingTotalVotesText.text = "Vote (0 votes)";
        productManagementTotalVotesText.text = "Vote (0 votes)";

        minigamesManager.financeVotes = 0;
        minigamesManager.qaVotes = 0;
        minigamesManager.programmingVotes = 0;
        minigamesManager.productManagementVotes = 0;

        minigameVotingPanel.SetActive(false);
    }

    [ClientRpc]
    public void UpdateMinigamesPointsClientRpc(float points, ulong clientId)
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == clientId)
            {
                player.GetComponent<PlayerSkills>().minigamesPoints += points;

                Debug.Log(player.GetComponent<PlayerSkills>().playerName + " has " + player.GetComponent<PlayerSkills>().minigamesPoints + " points");
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}