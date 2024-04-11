using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionButtons : MonoBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;

    private void Start()
    {
        playerInputAdvanced = GameObject.FindWithTag("Player").GetComponent<PlayerInputAdvanced>();
    }

    public void GoLeft()
    {
        playerInputAdvanced.MoveToNode(playerInputAdvanced.gamePiece.currentNode.leftNeighbor);
        playerInputAdvanced.StartCoroutine(playerInputAdvanced.DiceMovement());
        gameObject.SetActive(false);
    }

    public void GoRight()
    {
        playerInputAdvanced.MoveToNode(playerInputAdvanced.gamePiece.currentNode.rightNeighbor);
        playerInputAdvanced.StartCoroutine(playerInputAdvanced.DiceMovement());
        gameObject.SetActive(false);
    }
}
