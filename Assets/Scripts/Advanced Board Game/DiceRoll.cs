using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : NetworkBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;

    [SerializeField] private Sprite[] diceSides = new Sprite[6];
    private Image diceImage;

    private void Start()
    {
        // playerInputAdvanced = GameObject.FindWithTag("Player").GetComponent<PlayerInputAdvanced>();
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
        diceImage = GetComponent<Image>();
    }

    public void RollDice()
    {
        if (!playerInputAdvanced.rolledDice && playerInputAdvanced.gamePiece.currentNode != null && playerInputAdvanced == PlayersManager.Instance.currentPlayer.GetComponent<PlayerInputAdvanced>())
        {
            StartCoroutine(RollTheDice());
        }
        else
        {
            Debug.Log("Not your turn");
        }
    }

    IEnumerator RollTheDice()
    {
        playerInputAdvanced.rolledDice = true;

        int randomDiceSide = 0;
        int finalSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            diceImage.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        finalSide = randomDiceSide + 1;
        Debug.Log($"Dice roll: {finalSide}");

        playerInputAdvanced.steps = finalSide;
        StartCoroutine(playerInputAdvanced.DiceMovement());
    }
}
