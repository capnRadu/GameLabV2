using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputAdvanced : MonoBehaviour
{
    [NonSerialized] public GamePiece gamePiece;

    [SerializeField] private GameObject upDirectionButton;
    [SerializeField] private GameObject downDirectionButton;
    [SerializeField] private GameObject leftDirectionButton;
    [SerializeField] private GameObject rightDirectionButton;

    public TextMeshProUGUI diceRollText;

    [NonSerialized] public int steps = 0;
    [NonSerialized] public bool rolledDice = false;

    [NonSerialized] public int defaultCameraFov = 60;
    private int bifurcationCameraFov = 80;

    private int coins = 0;
    [SerializeField] private TextMeshProUGUI coinsText;

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