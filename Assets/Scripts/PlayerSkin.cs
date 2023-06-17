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
            
        }
        else
        {
            GetComponent<PlayerSkin>().enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        HeadSR.sprite = Heads[currHead];
        BodySR.sprite = Bodies[currBody];   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
