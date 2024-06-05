using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AnimationEvents : NetworkBehaviour
{
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject downButton;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private GameObject hireMenu;
    [SerializeField] private GameObject officeMenu;
    [SerializeField] private GameObject riskMenu;
    [SerializeField] private GameObject diceRollText;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject skillSelectPanel;
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private GameObject skillAttributionPanel;
    [SerializeField] private GameObject namesPanel;
    [SerializeField] private GameObject playerOverviewPanel;

    public void SkillSelectPanelOn()
    {
        skillSelectPanel.SetActive(true);
        startButton.SetActive(false);
        hudCanvas.SetActive(true);

        upButton.SetActive(true);
        downButton.SetActive(true);
        leftButton.SetActive(true);
        rightButton.SetActive(true);
        diceRollText.SetActive(true);
        hireMenu.SetActive(true);
        officeMenu.SetActive(true);
        riskMenu.SetActive(true);

        NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerInputAdvanced>().Setup();
    }

    public void SkillAttributionPanelOn()
    {
        skillAttributionPanel.SetActive(true);
        skillSelectPanel.SetActive(false);
    }

    public void NamesPanelOn()
    {
        namesPanel.SetActive(true);
        skillAttributionPanel.SetActive(false);
    }

    public void PlayerOverviewPanelOn()
    {
        playerOverviewPanel.SetActive(true);
        namesPanel.SetActive(false);
    }

    public void PlayerOverviewPanelOff()
    {
        playerOverviewPanel.SetActive(false);
        fadeImage.SetActive(false);
    }

}
