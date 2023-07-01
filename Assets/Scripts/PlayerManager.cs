using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;
using FishNet;
using static UnityEngine.GraphicsBuffer;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance;
    //UIController uicontrollerRef = UIController.instance;

    // Create Dictionary of all the players in the current Scene
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    public int localClientID;
    public class Player
    {
        public NetworkConnection connection;
        public GameObject playerObject;
        public float currentHealth = 0;
        public float currentArmor = 0;
        public string playerName;
        public int score;
    }

    private void Awake()
    {
        instance = this;
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log(players.Count);
            foreach (int keys in players.Keys)
            {
                Debug.Log(keys + ": " + players[keys].playerName);
                UpdateKillFeedText("I killed you, biatch");
            }
        }

    }
    public void DamagePlayer(int attackerID, float damage, float armorMult, int targetID)
    {
        if (!base.IsServer)
        {
            return;
        }
        if (players[targetID].currentArmor > 0)
        {
            players[targetID].currentHealth -= damage - players[targetID].currentArmor * armorMult;
            players[targetID].currentArmor -= damage;
        }
        else
        {
            players[targetID].currentHealth -= damage;
        }

        UpdateLocalUI(players[targetID].connection, players[targetID].playerObject, players[targetID].currentHealth, players[targetID].currentArmor);
        if (players[targetID].currentHealth <= 0)
        {
            PlayerSlain(attackerID, targetID);
        }
    }
    public void PlayerSlain(int attackerID, int targetID)
    {
        Debug.Log("Attacker ID: " + attackerID + "/ TargetID: " + targetID);
        // Set text;
        
        players[attackerID].score++;
        players[targetID].currentHealth = 100;
        UpdateLocalUI(players[targetID].connection, players[targetID].playerObject, 100, 0);
        //print("Player " + players[attackerID].playerName + " killed " + players[targetID].playerName);
        UpdateKillFeedText("Player " + players[attackerID].playerName + " killed " + players[targetID].playerName);
    }
   
    [TargetRpc]
    public void UpdateLocalUI(NetworkConnection connection, GameObject player, float Health, float Armor)
    {
        PlayerController script = player.GetComponent<PlayerController>();
        script.currentHealth = Health;
        script.currentArmor = Armor;
        script.LocalUICall();
    }
    [ObserversRpc(ExcludeOwner = false, ExcludeServer = false, BufferLast = false)]
    public void UpdateKillFeedText(string Feed)
    {
        
        PlayerController script = ClientManager.Connection.FirstObject.GetComponent<PlayerController>();
        Debug.Log(script);
        script.LocalKillFeedCall(Feed);
        
    }
}
