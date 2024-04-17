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
    [SerializeField] private TextMeshProUGUI playerValueText;
    [SerializeField] private CanvasRenderer radarMesh;
    private int playerValue;

    private void Awake()
    {
        playerSkills = GameObject.Find("Player").GetComponent<PlayerSkills>();

        playerNameText.text = playerSkills.playerName;
        companyNameText.text = playerSkills.companyName;
        playerColorImage.color = playerSkills.playerColor;

        foreach (var skill in playerSkills.skills)
        {
            playerValue += skill.Value;
        }

        playerValueText.text = $"Player Value: {playerValue}";
    }

    public void ContinueButton()
    {
        radarMesh.Clear();
        GetComponent<Animator>().Play("FadeOut");
    }
}
