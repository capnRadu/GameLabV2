using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputAdvanced : MonoBehaviour
{
    [NonSerialized] public GamePiece gamePiece;

    [SerializeField] private GameObject directionButtons;

    private int steps = 0;
    private bool rolledDice = false;

    private void Start()
    {
        gamePiece = GetComponent<GamePiece>();
    }

    private void Update()
    {
        // Keyboard movement (doesn't use the steps variable and the coroutine)
        // UpdateKeyboardMovement();

        // Dice movement
        UpdateDiceMovement();
    }

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
                directionButtons.SetActive(true);
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

    public IEnumerator DiceMovement()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (gamePiece.isMoving)
        {
            yield break;
        }

        while (steps > 0 && gamePiece.currentNode.NeighborsCount().Count == 1)
        {
            MoveToNode(gamePiece.currentNode.NeighborsCount()[0]);

            yield return new WaitForSeconds(0.5f);
        }

        if (steps != 0)
        {
            directionButtons.SetActive(true);
        }
        else
        {
            rolledDice = false;
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
}