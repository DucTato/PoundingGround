using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance;
    //UIController uicontrollerRef;

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
    
    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        players[localClientID].currentHealth--;
    //        Debug.Log(players[localClientID].currentHealth);
    //        Debug.Log(players[localClientID].currentArmor);
    //        Debug.Log(players[localClientID].playerName);
    //    }

    //}
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
        UpdateLocalUI(players[targetID].connection, players[targetID].playerObject, targetID);
        if (players[targetID].currentHealth <= 0)
        {
            PlayerSlain(attackerID, targetID);
        }
    }
    public void PlayerSlain(int attackerID, int targetID)
    {
        // Set text;
        print("Player " + players[attackerID].playerName + " killed " + players[targetID].playerName);
        players[attackerID].score++;
    }
   
    [TargetRpc]
    void UpdateLocalUI(NetworkConnection connection, GameObject player,int ID)
    {
        PlayerController script = player.GetComponent<PlayerController>();
        script.currentHealth = players[ID].currentHealth;
        script.currentArmor = players[ID].currentArmor;
        script.LocalUICall();
    }
}
