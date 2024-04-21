using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DirectionButtons : NetworkBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;

    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;

    private void Start()
    {
        // playerInputAdvanced = GameObject.FindWithTag("Player").GetComponent<PlayerInputAdvanced>();
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
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
            case "up":
                playerInputAdvanced.MoveToNode(playerInputAdvanced.gamePiece.currentNode.upNeighbor);
                break;
            case "down":
                playerInputAdvanced.MoveToNode(playerInputAdvanced.gamePiece.currentNode.downNeighbor);
                break;
        }

        leftButton.SetActive(false);
        rightButton.SetActive(false);
        upButton.SetActive(false);
        downButton.SetActive(false);

        if (playerInputAdvanced.gamePiece.currentNode.tag == "YellowTile")
        {
            playerInputAdvanced.hireMenu.SetActive(true);
            playerInputAdvanced.isHireMenuActive = true;
        }

        playerInputAdvanced.StartCoroutine(playerInputAdvanced.SmoothCameraFov(playerInputAdvanced.defaultCameraFov, 0.2f));
        playerInputAdvanced.StartCoroutine(playerInputAdvanced.DiceMovement());
    }
}
