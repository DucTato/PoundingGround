using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickUps : NetworkBehaviour
{
    public enum Rarity
    {
        Common = 0,
        Uncommon = 1,
        Exotic = 2,
        Legendary = 3
    }
    [SerializeField] private Sprite[] raritySprite;
    private Animator anim;
    private SpriteRenderer currentSR;
    [SyncVar]
    public Rarity rarityType;
    // Start is called before the first frame update
    void Start()
    {
        currentSR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentSR.sprite = raritySprite[(int)rarityType];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (InstanceFinder.IsServer)
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                Debug.Log("Picked upper's ID is: " + pc.gameObject.GetInstanceID());
                //PlayerManager.instance.DamagePlayer(attackerID, damageToGive, ignoreArmorMult, pc.gameObject.GetInstanceID());
            }
            anim.SetBool("isOpened", true);
        }
    }
    private void BeingOpened()
    {
        InstanceFinder.ServerManager.Despawn(gameObject);
    }
}

