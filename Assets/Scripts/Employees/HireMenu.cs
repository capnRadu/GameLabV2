using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HireMenu : NetworkBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;

    [SerializeField] private GameObject employeePrefab;
    private GameObject[] employees = new GameObject[5];
    private float posY = 170f;
    private int spacing = 90;

    private void Awake()
    {
        // playerInputAdvanced = GameObject.FindWithTag("Player").GetComponent<PlayerInputAdvanced>();
        // playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
    }

    private void OnEnable()
    {
        InstantiateEmployees(employeePrefab);
    }

    private void InstantiateEmployees(GameObject employeePrefab)
    {
        /*for (int i = 0; i < 5; i++)
        {
            GameObject employee = Instantiate(employeePrefab, new Vector3(0, posY, 0), Quaternion.identity);
            employee.transform.SetParent(gameObject.transform);
            employee.transform.localPosition = new Vector3(0, posY, 0);
            employee.transform.localScale = new Vector3(1, 1, 1);
            employee.transform.localRotation = Quaternion.identity;

            posY -= spacing;
        }*/

        foreach (GameObject employee in employees)
        {
            if (employee == null)
            {
                GameObject employeeInstance = Instantiate(employeePrefab, new Vector3(0, posY - (spacing * Array.IndexOf(employees, employee)), 0), Quaternion.identity);
                employeeInstance.transform.SetParent(gameObject.transform);
                employeeInstance.transform.localPosition = new Vector3(0, posY - (spacing * Array.IndexOf(employees, employee)), 0);
                employeeInstance.transform.localScale = new Vector3(1, 1, 1);
                employeeInstance.transform.localRotation = Quaternion.identity;

                employees[Array.IndexOf(employees, employee)] = employeeInstance;
            }
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

        foreach (var radarMesh in GameObject.FindObjectsOfType<CanvasRenderer>())
        {
            if (radarMesh.name == "Radar Mesh")
            {
                radarMesh.Clear();
            }
        }


        if (GameObject.Find("Employee Overview"))
        {
            GameObject.Find("Employee Overview").SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
