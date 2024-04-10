using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputAdvanced : MonoBehaviour
{
    [NonSerialized] public GamePiece gamePiece;

    [SerializeField] private GameObject directionButtons;

    private void Start()
    {
        gamePiece = GetComponent<GamePiece>();
    }

    private void Update()
    {
        // Keyboard movement
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

        // Dice movement
    }

    public void MoveToNode(MapNode node)
    {
        if (node != null)
        {
            gamePiece.currentNode = node;
        }
    }
}