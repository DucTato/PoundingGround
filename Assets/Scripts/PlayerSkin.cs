using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class PlayerSkin : NetworkBehaviour
{
    [SerializeField] private Sprite[] Heads, Bodies;
    [SerializeField] private SpriteRenderer HeadSR, BodySR;
    
    public int currHead, currBody;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            currHead = PlayerPrefs.GetInt("Head");
            currBody = PlayerPrefs.GetInt("Body");
            setPlayerSkin(currHead, currBody);
            //Debug.Log("Found PlayerPrefs parts");
        }
        else
        {
            GetComponent<PlayerSkin>().enabled = false;
            return;
        }
    }

    [ServerRpc]
    private void setPlayerSkin(int headPart, int bodyPart)
    {
        HeadSR.sprite = Heads[headPart];
        BodySR.sprite = Bodies[bodyPart];
        setPlayerSkinObservers(headPart, bodyPart);
    }
    [ObserversRpc(ExcludeOwner = false, ExcludeServer = true, BufferLast = true)]
    private void setPlayerSkinObservers(int headPart, int bodyPart)
    {
        HeadSR.sprite = Heads[headPart];
        BodySR.sprite = Bodies[bodyPart];
    }
}
