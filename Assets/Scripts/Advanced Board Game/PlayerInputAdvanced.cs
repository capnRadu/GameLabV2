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

        employeesText.text = $"{employees}/{maxEmployees}";

        hireMenu = GameObject.FindWithTag("Hire Menu");

        upDirectionButton.SetActive(false);
        downDirectionButton.SetActive(false);
        leftDirectionButton.SetActive(false);
        rightDirectionButton.SetActive(false);

        diceRollText.gameObject.SetActive(false);

        hireMenu.SetActive(false);
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

        while (steps > 0 && gamePiece.currentNode.NeighborsCount().Count == 1 && !isHireMenuActive)
        {
            MoveToNode(gamePiece.currentNode.NeighborsCount()[0]);
            diceRollText.text = steps.ToString();

            yield return new WaitForSeconds(0.5f);

            if (gamePiece.currentNode.tag == "YellowTile")
            {
                hireMenu.SetActive(true);
                isHireMenuActive = true;
            }
        }

        if (!isHireMenuActive)
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
            gamePiece.currentNode = node;
            steps--;
            Debug.Log($"Remaining steps: {steps}");
        }
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
        coinsText.text = coins.ToString();
    }

    [ServerRpc]
    public void NextPlayerServerRpc()
    {
        PlayersManager.Instance.NextPlayerClientRpc();
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