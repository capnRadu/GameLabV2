using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmployeeSkills : MonoBehaviour
{
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

    private void Awake()
    {
        skills["Programming"] = UnityEngine.Random.Range(1, 11);
        skills["Design"] = UnityEngine.Random.Range(1, 11);
        skills["Finance"] = UnityEngine.Random.Range(1, 11);
        skills["Product Management"] = UnityEngine.Random.Range(1, 11);
        skills["Quality Assurance"] = UnityEngine.Random.Range(1, 11);

        foreach (var skill in skills)
        {
            if (skill.Value > maxAttributePoints)
            {
                maxAttributePoints = skill.Value;
            }
        }

        employeeName.text = names[UnityEngine.Random.Range(0, names.Length)];
    }
}
