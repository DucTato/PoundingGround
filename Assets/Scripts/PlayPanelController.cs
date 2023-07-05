using FishNet;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Transporting.Tugboat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanelController : MonoBehaviour
{
    [SerializeField] private string[] mapNames;
    [SerializeField] private InputField IPInputField;
    [SerializeField] private Tugboat setupIP;
    private string selectedMap, selectedIP, localIP;
    
    // Start is called before the first frame update
    void Start()
    {
        selectedMap = mapNames[0];
        if(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        {
            localIP = GetLocalIPAddress();
            selectedIP = localIP;
        }
        else
        {
            throw new System.Exception("No Internet Connection");
        }
        SetDefaultState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartButton()
    {
        setupIP.SetClientAddress(selectedIP);
        SceneLoadData sld = new SceneLoadData(selectedMap);
        sld.ReplaceScenes = ReplaceOption.All;
        PlayerPrefs.SetString("recentIP", selectedIP);
        PlayerPrefs.Save();
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        
    }
    public void DropDownMapSelect(int option)
    {
        switch (option)
        {
            case 0:
                selectedMap = mapNames[0];
                break;
        }
    }
    public static string GetLocalIPAddress()
    {
        // Finding the local IP address:
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No IPv4 address for you");
    }
    public void enterIP(string ip)
    {
        if(ip == "")
        {
            selectedIP = localIP;
        }
        else
        {
            selectedIP = ip;
        }
    }
    public void SetDefaultState()
    {
        if (PlayerPrefs.HasKey("recentIP"))
        {
            if (PlayerPrefs.GetString("recentIP") != localIP)
            {
                IPInputField.text = PlayerPrefs.GetString("recentIP");
            }
            else
            {
                IPInputField.text = "";
            }
        }
    }
}
