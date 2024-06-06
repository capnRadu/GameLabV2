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
    [SerializeField] private TextMeshProUGUI minigamesPointsText;

    [SerializeField] private TextMeshProUGUI programmingText;
    [SerializeField] private TextMeshProUGUI designText;
    [SerializeField] private TextMeshProUGUI financeText;
    [SerializeField] private TextMeshProUGUI productManagementText;
    [SerializeField] private TextMeshProUGUI qualityAssuranceText;

    [SerializeField] private GameObject infoPanel1;
    [SerializeField] private GameObject infoPanel2;
    [SerializeField] private GameObject infoPanel3;

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

        if (minigamesPointsText)
        {
            minigamesPointsText.text = $"Minigames Points: {playerSkills.minigamesPoints}";
        }
    }

    public void SkillPointsSetup()
    {
        programmingText.text = $"Programming ({playerSkills.skills["Programming"]})";
        if (playerSkills.primarySkills.Contains("Programming"))
        {
            programmingText.color = Color.red;
        }

        designText.text = $"Design ({playerSkills.skills["Design"]})";
        if (playerSkills.primarySkills.Contains("Design"))
        {
            designText.color = Color.red;
        }

        financeText.text = $"Finance ({playerSkills.skills["Finance"]})";
        if (playerSkills.primarySkills.Contains("Finance"))
        {
            financeText.color = Color.red;
        }

        productManagementText.text = $"Product Management ({playerSkills.skills["Product Management"]})";
        if (playerSkills.primarySkills.Contains("Product Management"))
        {
            productManagementText.color = Color.red;
        }

        qualityAssuranceText.text = $"Quality Assurance ({playerSkills.skills["Quality Assurance"]})";
        if (playerSkills.primarySkills.Contains("Quality Assurance"))
        {
            qualityAssuranceText.color = Color.red;
        }

        /*foreach (var skill in playerSkills.skills)
        {
            playerValue += skill.Value;
        }

        playerValueText.text = $"Player Value: {playerValue}";*/
    }

    public void ContinueButton()
    {
        radarMesh.Clear();
        infoPanel1.SetActive(false);
        infoPanel2.SetActive(false);
        infoPanel3.SetActive(false);
        GetComponent<Animator>().Play("FadeOut");
    }
}
