using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class EmployeeSkills : NetworkBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;
    PlayerSkills playerSkills;

    public Dictionary<string, int> skills = new Dictionary<string, int>()
    {
        {"Programming", 1 },
        {"Design", 1 },
        {"Finance", 1 },
        {"Product Management", 1 },
        {"Quality Assurance", 1 }
    };
    [NonSerialized] public int maxAttributePoints = 0;

    [SerializeField] private TextMeshProUGUI employeeName;
    private string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack" };
    private string[] surnames = { "S.", "T.", "U.", "V.", "W.", "A.", "B." };

    private int hireCost;
    [SerializeField] private TextMeshProUGUI hireCostText;

    private void Awake()
    {
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
        playerSkills = playerInputAdvanced.GetComponent<PlayerSkills>();

        skills["Programming"] = UnityEngine.Random.Range(1, 4);
        skills["Design"] = UnityEngine.Random.Range(1, 4);
        skills["Finance"] = UnityEngine.Random.Range(1, 4);
        skills["Product Management"] = UnityEngine.Random.Range(1, 4);
        skills["Quality Assurance"] = UnityEngine.Random.Range(1, 4);

        foreach (var skill in skills)
        {
            if (skill.Value > maxAttributePoints)
            {
                maxAttributePoints = skill.Value;
            }
        }

        employeeName.text = names[UnityEngine.Random.Range(0, names.Length)] + " " + surnames[UnityEngine.Random.Range(0, surnames.Length)];

        int skillSum = 0;

        foreach (var skill in skills)
        {
            skillSum += skill.Value;
        }

        if (skillSum < 7)
        {
            hireCost = 3;
        }
        else if (skillSum < 12)
        {
            hireCost = 6;
        }
        else
        {
            hireCost = 8;
        }

        hireCostText.text = $"Hire ({hireCost} coins)";
    }

    public void HireEmployee()
    {
        if (playerInputAdvanced.coins >= hireCost && playerInputAdvanced.employees < playerInputAdvanced.maxEmployees)
        {
            playerInputAdvanced.coins -= hireCost;
            playerInputAdvanced.UpdatePlayerCoinsServerRpc(playerInputAdvanced.coins, default);
            playerInputAdvanced.coinsText.text = playerInputAdvanced.coins.ToString();

            playerInputAdvanced.employees++;
            playerInputAdvanced.UpdatePlayerEmployeesServerRpc(playerInputAdvanced.employees, default);
            playerInputAdvanced.employeesText.text = $"{playerInputAdvanced.employees}/{playerInputAdvanced.maxEmployees}";

            foreach (var skill in skills)
            {
                playerSkills.skills[skill.Key] += skill.Value;
                
                if (playerSkills.skills[skill.Key] > playerSkills.maxAttributePoints)
                {
                    playerSkills.maxAttributePoints = playerSkills.skills[skill.Key];
                }
            }

            foreach (var playerOverview in GameObject.FindObjectsOfType<PlayerOverview>())
            {
                playerOverview.SkillPointsSetup();
            }

            foreach (var radarChart in GameObject.FindObjectsOfType<RadarChartUI>())
            {
                radarChart.SetupMesh();
            }

            playerSkills.UpdatePlayerAttributes();

            Destroy(gameObject);
        }
    }

    public void CloseOverviews()
    {
        if (GameObject.Find("Employee Overview"))
        {
            GameObject.Find("Employee Overview").SetActive(false);
        }
    }
}
