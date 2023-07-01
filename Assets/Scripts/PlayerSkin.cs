using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class PlayerSkin : NetworkBehaviour
{
    [SerializeField] private Sprite[] Heads, Bodies;
    [SerializeField] private SpriteRenderer HeadSR, BodySR;
    [SerializeField] private GameObject Marker;
    public string currPlayerName;
    
    public int currHead, currBody;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            currPlayerName = PlayerPrefs.GetString("PlayerName");
        }
        else
            currPlayerName = "Player";
        currHead = PlayerPrefs.GetInt("Head");
        currBody = PlayerPrefs.GetInt("Body");
        if (PlayerPrefs.HasKey("HasMarker"))
        {
            Marker.gameObject.SetActive(PlayerPrefs.GetInt("HasMarker") == 1);
        }
        else
        {
            Marker.gameObject.SetActive(false);
        }
        if (base.IsOwner)
        {
            setPlayerSkin(currHead, currBody, currPlayerName);
            //Debug.Log("Found PlayerPrefs parts");
        }
        else
        {
            //GetComponent<PlayerSkin>().enabled = false;
            return;
        }
        //if (base.IsServer)
        //{
        //    foreach (KeyValuePair<int, PlayerManager.Player> player in PlayerManager.instance.players)
        //    {
        //        player.Value.playerName = currPlayerName;
        //    }
        //}
        UpdatePlayerName(currPlayerName);
    }

    [ServerRpc]
    private void setPlayerSkin(int headPart, int bodyPart, string Name)
    {
        HeadSR.sprite = Heads[headPart];
        BodySR.sprite = Bodies[bodyPart];
        currPlayerName = Name;
        setPlayerSkinObservers(headPart, bodyPart, Name);
    }
    [ObserversRpc(ExcludeOwner = false, ExcludeServer = true, BufferLast = true)]
    private void setPlayerSkinObservers(int headPart, int bodyPart, string Name)
    {
        HeadSR.sprite = Heads[headPart];
        BodySR.sprite = Bodies[bodyPart];
        currPlayerName = Name;
    }
    [ServerRpc]
    private void UpdatePlayerName(string Name)
    {
        PlayerManager.instance.players[transform.root.gameObject.GetInstanceID()].playerName = Name;
    }
}
