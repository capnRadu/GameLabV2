using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RiskMenu : NetworkBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;

    private void Start()
    {
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
    }

    public void CloseMenu()
    {
        if (playerInputAdvanced.steps != 0)
        {
            playerInputAdvanced.StartCoroutine(playerInputAdvanced.DiceMovement());
        }
        else
        {
            playerInputAdvanced.rolledDice = false;
            playerInputAdvanced.diceRollText.gameObject.SetActive(false);
            playerInputAdvanced.NextPlayerServerRpc();
        }

        gameObject.SetActive(false);
    }
}
