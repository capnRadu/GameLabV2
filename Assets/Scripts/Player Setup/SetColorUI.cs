using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColorUI : MonoBehaviour
{
    PlayerSkills playerSkills;

    private void Awake()
    {
        playerSkills = GameObject.FindWithTag("Player").GetComponent<PlayerSkills>();
    }

    private void Update()
    {
        if (GetComponent<Image>().color != playerSkills.playerColor)
        {
            GetComponent<Image>().color = playerSkills.playerColor;
            playerSkills.SetColor(GetComponent<Image>().color);
        }
    }

    public void ContinueButton()
    {
        if (playerSkills.playerName.Length != 0 && playerSkills.companyName.Length != 0)
        {
            GetComponentInParent<Animator>().Play("FadeOut");
        }
    }
}
