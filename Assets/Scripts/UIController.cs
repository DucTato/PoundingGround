
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using SceneManager = UnityEngine.SceneManagement.SceneManager;
using FishNet;
using FishNet.Managing;
using FishNet.Transporting.Tugboat;


public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Image currentWeapon, isEmpty, hasAmmo;
    public Text weaponName, currentAmmo, healthText, armorText, progressText, pausedTextMessage, endOfMatchText;
    public Slider healthBar, armorBar, progressBar;
    public GameObject KillFeedComponent, pausePanel, endOfMatchPanel, matchOver;
    private KillFeedController killFeed;
    public PlayerController playerScript;
    private bool isPaused = false;
    private string localIP, playerWon;
    private NetworkManager netManager;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //netManager = FindObjectOfType<NetworkManager>();
        Debug.Log(netManager);
        
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
    //private void OnEnable()
    //{
    //    netManager = InstanceFinder.NetworkManager;
    //    Debug.Log("Erhm.. " + netManager + " ?");
    //    netManager.SceneManager.OnClientLoadedStartScenes += SceneManager_OnClientLoadedStartScenes;
    //}
    //private void OnDisable()
    //{
    //   netManager.SceneManager.OnClientLoadedStartScenes -= SceneManager_OnClientLoadedStartScenes;
    //}
    //private void SceneManager_OnClientLoadedStartScenes(FishNet.Connection.NetworkConnection arg1, bool arg2)
    //{
    //    InstanceFinder.ClientManager.StartConnection();
    //    //throw new System.NotImplementedException();
    //}
    private void OnEnable()
    {
        netManager = InstanceFinder.NetworkManager;
        netManager.SceneManager.OnQueueEnd += SceneManager_OnQueueEnd;
    }
    private void OnDisable()
    {
        netManager = InstanceFinder.NetworkManager;
        netManager.SceneManager.OnQueueEnd -= SceneManager_OnQueueEnd;
    }
    private void SceneManager_OnQueueEnd()
    {
        InstanceFinder.ClientManager.StartConnection();
        //throw new System.NotImplementedException();
    }




    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (playerScript != null)
                {
                    playerScript.enabled = true;
                }
                isPaused = false;
                pausePanel.SetActive(false);
            }
            else
            {
                if (playerScript != null)
                {
                    playerScript.enabled = false;
                }
                isPaused = true;
                ushort port = netManager.GetComponent<Tugboat>().GetPort();
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    localIP = GetLocalIPAddress();
                    pausedTextMessage.text = "Your local IP: " + localIP + ":" + port;
                }
                else
                    throw new System.Exception("No Internet Connection");
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
    public void QuitMenuButton()
    {
        //SceneLoadData sld = new SceneLoadData("Main Menu");
        //sld.ReplaceScenes = ReplaceOption.All;
        //InstanceFinder.SceneManager.LoadGlobalScenes(sld);
        InstanceFinder.ClientManager.StopConnection();
        InstanceFinder.ServerManager.StopConnection(true);
        SceneManager.LoadScene("Main Menu");
        
    }

    public void EndOfMatch(string message)
    {
        matchOver.SetActive(true);
        playerWon = message;
        StartCoroutine(WaitThenDisplayMessage());
    }
    private IEnumerator WaitThenDisplayMessage()
    {
        yield return new WaitForSeconds(2f);
        endOfMatchPanel.SetActive(true);
        endOfMatchText.text = playerWon;
        StartCoroutine(WaitThenEndLevel());
    }
    private IEnumerator WaitThenEndLevel()
    {
        yield return new WaitForSeconds(5f);
        InstanceFinder.ClientManager.StopConnection();
        InstanceFinder.ServerManager.StopConnection(true);
        SceneManager.LoadScene("Main Menu");
    }
}
