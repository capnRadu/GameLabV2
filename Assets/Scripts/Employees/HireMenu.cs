using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HireMenu : NetworkBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;

    [SerializeField] private GameObject employeePrefab;
    private float posY = 170f;
    private int spacing = 90;

    private void Start()
    {
        // playerInputAdvanced = GameObject.FindWithTag("Player").GetComponent<PlayerInputAdvanced>();
        // playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();

        InstantiateEmployees(employeePrefab);
    }

    private void InstantiateEmployees(GameObject employeePrefab)
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject employee = Instantiate(employeePrefab, new Vector3(0, posY, 0), Quaternion.identity);
            employee.transform.SetParent(gameObject.transform);
            employee.transform.localPosition = new Vector3(0, posY, 0);
            employee.transform.localScale = new Vector3(1, 1, 1);
            employee.transform.localRotation = Quaternion.identity;

            posY -= spacing;
        }
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
            playerInputAdvanced.NextPlayerServerRpc();
        }

        gameObject.SetActive(false);
    }
}
