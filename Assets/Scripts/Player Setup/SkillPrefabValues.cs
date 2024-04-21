using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SkillPrefabValues : NetworkBehaviour
{
    PlayerSkills playerSkills;
    SkillAttribution skillAttribution;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillPointsText;
    private int skillPoints;

    private void Awake()
    {
        // playerSkills = GameObject.FindWithTag("Player").GetComponent<PlayerSkills>();
        playerSkills = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerSkills>();
        skillAttribution = GameObject.FindObjectOfType<SkillAttribution>();
    }

    private void Update()
    {
        skillPointsText.text = skillPoints.ToString();
    }

    public void SetSkillValues(string name, int points)
    {
        skillName.text = name;
        skillPoints = points;
    }

    public void IncreaseAttribute()
    {
        if (skillAttribution.attributions > 0)
        {
            skillPoints++;
            skillAttribution.attributions--;
            playerSkills.skills[skillName.text]++;
            Debug.Log($"{skillName.text} skill level: " + playerSkills.skills[skillName.text]);
        }
    }

    public void DecreaseAttribute()
    {
        if (skillPoints > 2)
        {
            skillPoints--;
            skillAttribution.attributions++;
            playerSkills.skills[skillName.text]--;
            Debug.Log($"{skillName.text} skill level: " + playerSkills.skills[skillName.text]);
        }
    }
}
