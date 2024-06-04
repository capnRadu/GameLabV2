using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SkillAttribution : NetworkBehaviour
{
    PlayerSkills playerSkills;

    [SerializeField] private GameObject skillPrefab;

    private float yPos = 40;

    [NonSerialized] public int attributions = 4;
    [SerializeField] private TextMeshProUGUI attributionsText;

    [SerializeField] private GameObject infoPanel;

    private void Awake()
    {
        // playerSkills = GameObject.FindWithTag("Player").GetComponent<PlayerSkills>();
        playerSkills = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerSkills>();
    }

    private void Start()
    {
        InstantiateSkills(skillPrefab);
    }

    private void Update()
    {
        attributionsText.text = $"Allocate {attributions} attribute points to the chosen skills based on your proficiency.";
    }

    private void InstantiateSkills(GameObject skillPrefab)
    {
        foreach (var skill in playerSkills.skills)
        {
            if (skill.Value != 1)
            {
                GameObject skillInstance = Instantiate(skillPrefab, new Vector3(960, 540, 0), Quaternion.identity);
                skillInstance.transform.SetParent(transform);
                skillInstance.transform.localPosition = new Vector3(0, yPos, 0);
                skillInstance.GetComponent<SkillPrefabValues>().SetSkillValues(skill.Key, skill.Value);
                yPos -= 80;
            }
        }
    }

    public void ContinueButton()
    {
        if (attributions == 0)
        {
            foreach (var skill in playerSkills.skills)
            {
                if (skill.Value > playerSkills.maxAttributePoints)
                {
                    playerSkills.maxAttributePoints = skill.Value;
                }
            }

            Debug.Log("Max attribute points: " + playerSkills.maxAttributePoints);

            infoPanel.SetActive(false);

            GetComponent<Animator>().Play("FadeOut");

            playerSkills.UpdatePlayerAttributes();

            foreach (var attributePrefab in FindObjectsOfType<SkillPrefabValues>())
            {
                attributePrefab.GetComponent<Animator>().Play("FadeOut");
            }
        }
    }
}
