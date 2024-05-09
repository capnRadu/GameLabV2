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
        GetComponent<Image>().color = playerSkills.playerColor;
    }

    public void PreviousColor()
    {
        playerSkills.PreviousPlayerColor();
        GetComponent<Image>().color = playerSkills.playerColor;
    }
}
