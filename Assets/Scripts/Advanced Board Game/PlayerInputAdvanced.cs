using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputAdvanced : NetworkBehaviour
{
    [NonSerialized] public GamePiece gamePiece;

    [SerializeField] private GameObject upDirectionButton;
    [SerializeField] private GameObject downDirectionButton;
    [SerializeField] private GameObject leftDirectionButton;
    [SerializeField] private GameObject rightDirectionButton;

    public TextMeshProUGUI diceRollText;

    [NonSerialized] public int steps = 0;
    [NonSerialized] public bool rolledDice = false;

    [SerializeField] private GameObject cameraPov;
    [NonSerialized] public int defaultCameraFov = 60;
    private int bifurcationCameraFov = 80;

    [NonSerialized] public int coins = 0;
    [NonSerialized] public int employees = 0;
    [NonSerialized] public int maxEmployees = 3;
    [NonSerialized] public TextMeshProUGUI coinsText;
    [NonSerialized] public TextMeshProUGUI employeesText;

    public GameObject hireMenu;
    [NonSerialized] public bool isHireMenuActive = false;

    public GameObject officeMenu;
    [NonSerialized] public bool isOfficeMenuActive = false;

    private string[] risks = { "employee", "coin" };

    public bool hasVoted = false;

    public TextMeshProUGUI yourTurnText;

    private void Start()
    {
        gamePiece = GetComponent<GamePiece>();
    }

    private void Update()
    {
        // Keyboard movement (doesn't use the steps variable and the coroutine)
        // UpdateKeyboardMovement();

        // Dice movement (this is used without the separate dice roll script)
        // UpdateDiceMovement();

        if (PlayersManager.Instance.currentPlayer != null && this == PlayersManager.Instance.currentPlayer.GetComponent<PlayerInputAdvanced>())
        {
            yourTurnText.text = "Your Turn";
        }
        else
        {
            yourTurnText.text = "Not Your Turn";
        }
    }

    private void FixedUpdate()
    {
        // Update the camera POV to follow the current player
        UpdateCameraPov();
    }

    private void UpdateCameraPov()
    {
        if (PlayersManager.Instance.currentPlayer != null && this == PlayersManager.Instance.currentPlayer.GetComponent<PlayerInputAdvanced>() && Camera.main.transform.position != cameraPov.transform.position)
        {
            Camera.main.transform.SetParent(gameObject.transform);
            Camera.main.transform.rotation = cameraPov.transform.rotation;

            Vector3 desiredPosition = cameraPov.transform.position;
            Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, Time.deltaTime * 10f);
            Camera.main.transform.position = smoothedPosition;

            Debug.Log("Camera POV updated");
        }
    }

    public void Setup()
    {
        upDirectionButton = GameObject.FindWithTag("Up Button");
        downDirectionButton = GameObject.FindWithTag("Down Button");
        leftDirectionButton = GameObject.FindWithTag("Left Button");
        rightDirectionButton = GameObject.FindWithTag("Right Button");

        diceRollText = GameObject.FindWithTag("Text Dice Roll").GetComponent<TextMeshProUGUI>();
        coinsText = GameObject.FindWithTag("Text Player Coins").GetComponent<TextMeshProUGUI>();
        employeesText = GameObject.FindWithTag("Text Player Employees").GetComponent<TextMeshProUGUI>();
        yourTurnText = GameObject.FindWithTag("Text Player Turn").GetComponent<TextMeshProUGUI>();

        employeesText.text = $"{employees}/{maxEmployees}";

        hireMenu = GameObject.FindWithTag("Hire Menu");
        officeMenu = GameObject.FindWithTag("Office Menu");

        upDirectionButton.SetActive(false);
        downDirectionButton.SetActive(false);
        leftDirectionButton.SetActive(false);
        rightDirectionButton.SetActive(false);

        diceRollText.gameObject.SetActive(false);

        hireMenu.SetActive(false);
        officeMenu.SetActive(false);
    }

    public IEnumerator DiceMovement()
    {
        diceRollText.text = steps.ToString();

        if (diceRollText.gameObject.activeSelf == false)
        {
            diceRollText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        if (gamePiece.isMoving)
        {
            yield break;
        }

        while (steps > 0 && gamePiece.currentNode.NeighborsCount().Count == 1 && !isHireMenuActive && !isOfficeMenuActive)
        {
            MoveToNode(gamePiece.currentNode.NeighborsCount()[0]);
            diceRollText.text = steps.ToString();

            yield return new WaitForSeconds(0.5f);

            switch (gamePiece.currentNode.tag)
            {
                case "YellowTile":
                    hireMenu.SetActive(true);
                    isHireMenuActive = true;
                    break;
                case "GreenTile":
                    if (gamePiece.currentNode.GetComponent<Office>().owningPlayer.Length == 0 && steps == 0)
                    {
                        officeMenu.SetActive(true);
                        isOfficeMenuActive = true;
                    }
                    break;
                case "BlackTile":
                    if (steps == 0)
                    {
                        int randomRisk = UnityEngine.Random.Range(0, risks.Length);

                        switch (risks[randomRisk])
                        {
                            case "employee":
                                employees -= 2;
                                UpdatePlayerEmployeesServerRpc(employees, default);
                                employeesText.text = $"{employees}/{maxEmployees}";
                                Debug.Log("You lost 2 employees");
                                break;
                            case "coin":
                                coins = (int)(coins / 2);
                                UpdatePlayerCoinsServerRpc(coins, default);
                                coinsText.text = coins.ToString();
                                Debug.Log("You lost half of your coins");
                                break;
                        }
                    }
                    break;
            }
        }

        if (!isHireMenuActive && !isOfficeMenuActive)
        {
            if (steps != 0)
            {
                StartCoroutine(SmoothCameraFov(bifurcationCameraFov, 0.2f));

                if (gamePiece.currentNode.upNeighbor != null)
                {
                    upDirectionButton.SetActive(true);
                }
                if (gamePiece.currentNode.downNeighbor != null)
                {
                    downDirectionButton.SetActive(true);
                }
                if (gamePiece.currentNode.leftNeighbor != null)
                {
                    leftDirectionButton.SetActive(true);
                }
                if (gamePiece.currentNode.rightNeighbor != null)
                {
                    rightDirectionButton.SetActive(true);
                }
            }
            else
            {
                rolledDice = false;
                diceRollText.gameObject.SetActive(false);
                CheckTile();
                NextPlayerServerRpc();
            }
        }
    }

    public void MoveToNode(MapNode node)
    {
        if (node != null)
        {
            // gamePiece.currentNode = node;
            UpdateCurrentNodeServerRpc(node);
            steps--;
            Debug.Log($"Remaining steps: {steps}");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateCurrentNodeServerRpc(NetworkBehaviourReference mapNodeReference)
    {
        PlayersManager.Instance.UpdateCurrentNodeClientRpc(mapNodeReference);
    }

    public IEnumerator SmoothCameraFov(int targetFov, float duration)
    {
        float startFov = Camera.main.fieldOfView;
        float time = 0;

        while (time < duration)
        {
            Camera.main.fieldOfView = Mathf.Lerp(startFov, targetFov, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Camera.main.fieldOfView = targetFov;
    }

    private void CheckTile()
    {
        if (gamePiece.currentNode.tag == "BlueTile")
        {
            coins += 3;
        }
        else if (gamePiece.currentNode.tag == "RedTile")
        {
            coins -= 3;
        }

        coins = Mathf.Clamp(coins, 0, 1000000);
        UpdatePlayerCoinsServerRpc(coins, default);
        coinsText.text = coins.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerCoinsServerRpc(int amount, ServerRpcParams serverRpcParams)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;

        PlayersManager.Instance.UpdatePlayerCoinsClientRpc(amount, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerEmployeesServerRpc(int amount, ServerRpcParams serverRpcParams)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;

        PlayersManager.Instance.UpdatePlayerEmployeesClientRpc(amount, clientId);
    }

    [ServerRpc]
    public void NextPlayerServerRpc()
    {
        PlayersManager.Instance.NextPlayerClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void VoteMinigameServerRpc(string minigame, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;

        PlayersManager.Instance.VoteMinigameClientRpc(minigame, clientId);
    }

    // KEYBOARD INPUT - UNUSED
    private void UpdateKeyboardMovement()
    {
        if (!gamePiece.isMoving && gamePiece.currentNode != null)
        {
            if (gamePiece.currentNode.NeighborsCount().Count == 1)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    MoveToNode(gamePiece.currentNode.NeighborsCount()[0]);
                }
            }
            else
            {
                if (gamePiece.currentNode.upNeighbor != null)
                {
                    upDirectionButton.SetActive(true);
                }
                if (gamePiece.currentNode.downNeighbor != null)
                {
                    downDirectionButton.SetActive(true);
                }
                if (gamePiece.currentNode.leftNeighbor != null)
                {
                    leftDirectionButton.SetActive(true);
                }
                if (gamePiece.currentNode.rightNeighbor != null)
                {
                    rightDirectionButton.SetActive(true);
                }
            }
        }
    }

    private void UpdateDiceMovement()
    {
        if (Input.GetKeyDown(KeyCode.E) && !rolledDice && gamePiece.currentNode != null)
        {
            steps = UnityEngine.Random.Range(1, 7);
            rolledDice = true;
            Debug.Log($"Dice roll: {steps}");

            StartCoroutine(DiceMovement());
        }
    }
}