using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverview : NetworkBehaviour
{
    PlayerSkills playerSkills;

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI companyNameText;
    [SerializeField] private Image playerColorImage;
    [SerializeField] private CanvasRenderer radarMesh;

    [SerializeField] private TextMeshProUGUI programmingText;
    [SerializeField] private TextMeshProUGUI designText;
    [SerializeField] private TextMeshProUGUI financeText;
    [SerializeField] private TextMeshProUGUI productManagementText;
    [SerializeField] private TextMeshProUGUI qualityAssuranceText;

    // [SerializeField] private TextMeshProUGUI playerValueText;
    // private int playerValue;

    private void Awake()
    {
        // playerSkills = GameObject.FindWithTag("Player").GetComponent<PlayerSkills>();
        playerSkills = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerSkills>();

        if (playerNameText)
        {
            playerNameText.text = playerSkills.playerName;
        }

        if (companyNameText)
        {
            companyNameText.text = playerSkills.companyName;
        }

        if (playerColorImage)
        {
            playerColorImage.color = playerSkills.playerColor;
        }
    }

    private void OnEnable()
    {
        SkillPointsSetup();
    }

    public void SkillPointsSetup()
    {
        programmingText.text = $"Programming ({playerSkills.skills["Programming"]})";
        designText.text = $"Design ({playerSkills.skills["Design"]})";
        financeText.text = $"Finance ({playerSkills.skills["Finance"]})";
        productManagementText.text = $"Product Management ({playerSkills.skills["Product Management"]})";
        qualityAssuranceText.text = $"Quality Assurance ({playerSkills.skills["Quality Assurance"]})";

        /*foreach (var skill in playerSkills.skills)
        {
            playerValue += skill.Value;
        }

        playerValueText.text = $"Player Value: {playerValue}";*/
    }

    public void ContinueButton()
    {
        radarMesh.Clear();
        GetComponent<Animator>().Play("FadeOut");
    }
}
