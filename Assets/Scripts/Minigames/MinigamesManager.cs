using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MinigamesManager : NetworkBehaviour
{
    PlayerInputAdvanced playerInputAdvanced;
    [NonSerialized] public PlayerSkills playerSkills;
    [NonSerialized] public List<string> playerPrimarySkills = new List<string>();

    public int financeVotes = 0;
    public int qaVotes = 0;
    public int programmingVotes = 0;
    public int productManagementVotes = 0;

    private void Start()
    {
        playerInputAdvanced = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>();
        playerSkills = playerInputAdvanced.GetComponent<PlayerSkills>();
        playerPrimarySkills = playerSkills.primarySkills;
    }

    public void VoteMinigame(string minigame)
    {
        playerInputAdvanced.VoteMinigameServerRpc(minigame);
    }
}