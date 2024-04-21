using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerInfo : NetworkBehaviour
{
    PlayerSkills playerSkills;

    private void Awake()
    {
        // playerSkills = GameObject.FindWithTag("Player").GetComponent<PlayerSkills>();
        playerSkills = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerSkills>();
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

    public void SetPlayerName(string name)
    {
        playerSkills.SetPlayerName(name);
    }

    public void SetCompanyName(string name)
    {
        playerSkills.SetCompanyName(name);
    }

    public void NextColor()
    {
        playerSkills.NextPlayerColor();
    }

    public void PreviousColor()
    {
        playerSkills.PreviousPlayerColor();
    }
}
