using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HireMenu : MonoBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;

    private void Start()
    {
        playerInputAdvanced = GameObject.FindWithTag("Player").GetComponent<PlayerInputAdvanced>();
    }

    public void CloseMenu()
    {
        playerInputAdvanced.isHireMenuActive = false;

        if (playerInputAdvanced.steps != 0)
        {
            playerInputAdvanced.StartCoroutine(playerInputAdvanced.DiceMovement());
        }
        else
        {
            playerInputAdvanced.rolledDice = false;
            playerInputAdvanced.diceRollText.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
