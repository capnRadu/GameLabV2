using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverview : MonoBehaviour
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
        playerSkills = GameObject.FindWithTag("Player").GetComponent<PlayerSkills>();

        playerNameText.text = playerSkills.playerName;
        companyNameText.text = playerSkills.companyName;
        playerColorImage.color = playerSkills.playerColor;

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
