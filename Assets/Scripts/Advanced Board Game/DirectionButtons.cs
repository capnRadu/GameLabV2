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

    public void Bifurcation(string direction)
    {
        switch (direction)
        {
            case "left":
                playerInputAdvanced.MoveToNode(playerInputAdvanced.gamePiece.currentNode.leftNeighbor);
                break;
            case "right":
                playerInputAdvanced.MoveToNode(playerInputAdvanced.gamePiece.currentNode.rightNeighbor);
                break;
        }

        playerInputAdvanced.StartCoroutine(playerInputAdvanced.DiceMovement());
        gameObject.SetActive(false);
    }
}
