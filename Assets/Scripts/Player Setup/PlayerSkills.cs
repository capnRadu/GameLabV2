using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSkills : NetworkBehaviour
{
    [NonSerialized]
    public Dictionary<string, int> skills = new Dictionary<string, int>()
    {
        {"Programming", 1 },
        {"Design", 1 },
        {"Finance", 1 },
        // {"Marketing", 1 },
        // {"Data Analysis", 1 },
        // {"Human Resources", 1 },
        {"Product Management", 1 },
        {"Quality Assurance", 1 }
    };
    [NonSerialized] public List<string> primarySkills = new List<string>();
    [NonSerialized] public int maxAttributePoints = 0;

    [NonSerialized] public string playerName = "";
    [NonSerialized] public string companyName = "";
    [NonSerialized] public Color playerColor = Color.white;

    private List<Color> possibleColors = new List<Color>()
    {
        Color.white,
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta,
        Color.gray,
        Color.black
    };
    private int colorIndex = 0;

    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log("Player name is " + playerName);
    }

    public void SetCompanyName(string name)
    {
        companyName = name;
        Debug.Log("Company name is " + companyName);
    }

    public void NextPlayerColor()
    {
        if (colorIndex == possibleColors.Count - 1)
        {
            colorIndex = 0;
        }
        else
        {
            colorIndex++;
        }

        playerColor = possibleColors[colorIndex];
        Debug.Log("Player color is " + playerColor);

        SetColorServerRpc(playerColor);
    }

    public void PreviousPlayerColor()
    {
        if (colorIndex == 0)
        {
            colorIndex = possibleColors.Count - 1;
        }
        else
        {
            colorIndex--;
        }

        playerColor = possibleColors[colorIndex];
        Debug.Log("Player color is " + playerColor);

        SetColorServerRpc(playerColor);
    }

    [ServerRpc]
    private void SetColorServerRpc(Color color)
    {
        GetComponentInChildren<MeshRenderer>().material.color = color;
        UpdateColorClientRpc(color);
    }

    [ClientRpc]
    private void UpdateColorClientRpc(Color color)
    {
        GetComponentInChildren<MeshRenderer>().material.color = color;
    }
}