using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet;
using FishNet.Object.Synchronizing;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance;
    //UIController uicontrollerRef = UIController.instance;

    // Create Dictionary of all the players in the current Scene
    public Dictionary<int, Player> players = new Dictionary<int, Player>();
    public Transform[] spawnPoints;
    public GameObject randomPickups;
    public int localClientID;
    public BoxCollider2D[] spawners;
    private int maxScore = 0;
   
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
               
            }
            
            SpawnRandomPickups(Random.Range(0, 4));
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
        
        
        players[attackerID].score++;
        players[targetID].currentHealth = 100;
        UpdateLocalUI(players[targetID].connection, players[targetID].playerObject, 100, 0);
        RespawnPlayer(players[targetID].connection, players[targetID].playerObject, Random.Range(0, spawnPoints.Length));
        if (players[attackerID].score >= maxScore)
        {
            maxScore = players[attackerID].score;
        }
        UpdateKillFeedText(players[attackerID].playerName + "  fragged  " + players[targetID].playerName, maxScore);
    }
    [TargetRpc]
    public void AddAmmo(NetworkConnection connection, GameObject player)
    {
        PlayerController script = player.GetComponent<PlayerController>();
        script.currentUsedGun.ReplenishAmmo();
    }
    [TargetRpc]
    public void UpdateLocalUI(NetworkConnection connection, GameObject player, float Health, float Armor)
    {
        PlayerController script = player.GetComponent<PlayerController>();
        script.currentHealth = Health;
        script.currentArmor = Armor;
        script.LocalUICall();
    }
    [TargetRpc]
    public void RespawnPlayer(NetworkConnection connection, GameObject player, int spawn)
    {
        player.transform.position = spawnPoints[spawn].position;
    }
    [ObserversRpc(ExcludeOwner = false, ExcludeServer = false, BufferLast = true)]
    public void UpdateKillFeedText(string Feed, int Score)
    {
        PlayerController script = ClientManager.Connection.FirstObject.GetComponent<PlayerController>();
        Debug.Log(script);
        script.LocalKillFeedCall(Feed);
        script.LocalScoreUI(Score);
    }
    [ObserversRpc(ExcludeOwner = true, ExcludeServer = false, BufferLast = true)]
    public void SpawnRandomPickups(int rarity)
    {
        // Get the spawners' locations randomly
        int chosenSpawner = Random.Range(0, spawners.Length);
        // Inside the chosen spawner, spawn the pick ups randomly
        float randomX = Random.Range(spawners[chosenSpawner].bounds.min.x, spawners[chosenSpawner].bounds.max.x);
        float randomY = Random.Range(spawners[chosenSpawner].bounds.min.y, spawners[chosenSpawner].bounds.max.y);
        Vector2 randomPos = new Vector2(randomX, randomY);
        // Instantiate the pick up and then spawn it over the Network
        GameObject newPickUP = Instantiate(randomPickups, randomPos, Quaternion.identity);
        newPickUP.GetComponent<RandomPickUps>().rarityType = (RandomPickUps.Rarity)rarity;
        InstanceFinder.ServerManager.Spawn(newPickUP);
    }

    public void ReplenishPlayer(float healAmount, float armorPoint, int targetID)
    {
        players[targetID].currentHealth += healAmount;
        if (players[targetID].currentHealth > 100) 
        {
            players[targetID].currentHealth = 100;
        }
        players[targetID].currentArmor = armorPoint;
        UpdateLocalUI(players[targetID].connection, players[targetID].playerObject, players[targetID].currentHealth, players[targetID].currentArmor);
    }
    public void PlayerAddAmmo(int targetID)
    {
        AddAmmo(players[targetID].connection, players[targetID].playerObject);
    }
}
