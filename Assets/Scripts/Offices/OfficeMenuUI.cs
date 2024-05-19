using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class OfficeMenuUI : NetworkBehaviour
{
    Office office;
    PlayerInputAdvanced playerInputAdvanced;

    [SerializeField] private TextMeshProUGUI streetNameText;
    [SerializeField] private TextMeshProUGUI maxEmployeesIncreaseText;
    [SerializeField] private TextMeshProUGUI costText;

    private void OnEnable()
    {
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
        office = NetworkManager.LocalClient.PlayerObject.GetComponent<GamePiece>().currentNode.GetComponent<Office>();

        if (office != null)
        {
            streetNameText.text = office.streetName;
            maxEmployeesIncreaseText.text = $"+{office.maxEmployeesIncrease} maximum employees";
            costText.text = $"Buy ({office.cost} coins)";
        }
    }

    public void BuyOffice()
    {
        if (playerInputAdvanced.coins >= office.cost)
        {
            playerInputAdvanced.coins -= office.cost;
            playerInputAdvanced.UpdatePlayerCoinsServerRpc(playerInputAdvanced.coins, default);
            playerInputAdvanced.coinsText.text = playerInputAdvanced.coins.ToString();

            playerInputAdvanced.maxEmployees += office.maxEmployeesIncrease;
            playerInputAdvanced.employeesText.text = $"{playerInputAdvanced.employees}/{playerInputAdvanced.maxEmployees}";

            // office.owningPlayer = playerInputAdvanced.GetComponent<PlayerSkills>().playerName;
            office.UpdateOfficeOwnershipServerRpc();

            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        playerInputAdvanced.isOfficeMenuActive = false;

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
