using FishNet;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Transporting.Tugboat;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanelController : MonoBehaviour
{
    [SerializeField] private string[] mapNames;
    [SerializeField] private InputField IPInputField, PortInputField;
    [SerializeField] private NetworkManager netManager;
    private string selectedMap, selectedIP, localIP;
    private ushort localPort, selectedPort;
    // Start is called before the first frame update
    private void Start()
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
        //netManager = InstanceFinder.NetworkManager;
        //localPort = netManager.GetComponent<Tugboat>().GetPort();
        Debug.Log("On Started " );
        SetDefaultState();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    private void OnEnable()
    {
        
        netManager = InstanceFinder.NetworkManager;
        localPort = netManager.GetComponent<Tugboat>().GetPort();
        selectedPort = localPort;
        Debug.Log("On Enabled " + localPort);
        
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
        //NetworkManager netManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        if (ip == "")
        {
            selectedIP = localIP;
        }
        else
        {
            selectedIP = ip;
        }
        netManager.GetComponent<Tugboat>().SetClientAddress(selectedIP);
    }
    public void enterPort(string portString)
    {
        if (portString == "")
        {
            selectedPort = localPort;
        }
        else
        {
            selectedPort = Convert.ToUInt16(portString);
        }
        netManager.GetComponent<Tugboat>().SetPort(selectedPort);
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
        //if (PlayerPrefs.HasKey("Port"))
        //{
        //    if (Convert.ToUInt16(PlayerPrefs.GetInt("Port")) != localPort)
        //    {
        //        PortInputField.text = PlayerPrefs.GetInt("Port").ToString();
        //        localPort = Convert.ToUInt16(PlayerPrefs.GetInt("Port"));
        //    }
        //    else
        //    {
        //        PortInputField.text = localPort.ToString();
        //    }
        //}
        PortInputField.text = localPort.ToString();
        Debug.Log("Default state created" + localPort);
        Debug.Log("PlayerPrefs: " + PlayerPrefs.GetInt("Port"));
    }
    public void StartButton()
    {
        if (selectedIP == localIP)
        {
            // if you press Start when the IP is your Local IP. It means you started playing as a Host(server)
            StartServer();
        }
        else
        {
            // if you press Start when the IP is not your Local IP. It means you started playing as a Client
            InstanceFinder.ClientManager.StartConnection();
        }
        
        

    }
    private void StartServer()
    {
        StartCoroutine(StartServerAndWait());
    }
    private IEnumerator StartServerAndWait()
    {
        InstanceFinder.ServerManager.StartConnection();
        yield return new WaitForSeconds(1f);
        netManager.GetComponent<Tugboat>().SetClientAddress(selectedIP);
        netManager.GetComponent<Tugboat>().SetPort(selectedPort);
        SceneLoadData sld = new SceneLoadData(selectedMap);
        sld.ReplaceScenes = ReplaceOption.All;
        PlayerPrefs.SetString("recentIP", selectedIP);
        //PlayerPrefs.SetInt("Port", Convert.ToInt32(selectedPort));
        PlayerPrefs.Save();
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }
}
