using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Image currentWeapon, isEmpty, hasAmmo;
    public Text weaponName, currentAmmo, healthText, armorText, progressText, pausedTextMessage;
    public Slider healthBar, armorBar, progressBar;
    public GameObject KillFeedComponent, pausePanel; 
    private KillFeedController killFeed;
    public PlayerController playerScript;
    private bool isPaused = false;
    private string localIP;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = 100f;
        armorBar.maxValue = 100f;
        progressBar.maxValue = 5;
        killFeed = KillFeedComponent.GetComponent<KillFeedController>();
        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        {
            localIP = GetLocalIPAddress();
            pausedTextMessage.text = "Your local IP: " + localIP;
        }
        else
            throw new System.Exception("No Internet Connection");
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if(playerScript!= null)
                {
                    playerScript.enabled = true;
                }
                isPaused = false;
                pausePanel.SetActive(false);
            }
            else
            {
                if (playerScript!= null)
                {
                    playerScript.enabled = false;
                }
                isPaused = true;
                pausePanel.SetActive(true);
            }
        }
    }
    public void NewKill(string Feed)
    {
        killFeed.NewKillFeed(Feed);
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
}
