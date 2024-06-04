using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

public class StartNetwork : MonoBehaviour
{
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private GameObject waitingText;
    [SerializeField] private TextMeshProUGUI ipAddressText;
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private GameObject hostButton;
    [SerializeField] private GameObject clientButton;
    [SerializeField] private GameObject infoPanel;

    private string ipAddress;
    private UnityTransport transport;

    private void Start()
    {
        ipAddress = "0.0.0.0";
        SetIpAddress();
    }

    private void Update()
    {
        if (PlayersManager.Instance.players.Length > 1)
        {
            PlayersManager.Instance.currentPlayer = PlayersManager.Instance.players[0];
            BeginGame();
        }
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        GetLocalIPAddress();
        waitingText.SetActive(true);
        ipAddressText.gameObject.SetActive(true);
        infoPanel.SetActive(true);
        hostButton.SetActive(false);
        clientButton.SetActive(false);
        ipInput.gameObject.SetActive(false);
    }

    public void StartClient()
    {
        ipAddress = ipInput.text;
        SetIpAddress();
        NetworkManager.Singleton.StartClient();
    }

    private void BeginGame()
    {
        fadeImage.GetComponent<Animator>().enabled = true;
        waitingText.SetActive(false);
        ipAddressText.gameObject.SetActive(false);
        ipInput.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressText.text = ip.ToString();
                ipAddress = ip.ToString();
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    private void SetIpAddress()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
    }
}
