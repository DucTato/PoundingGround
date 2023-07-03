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
    [SerializeField] private GameObject[] commons, uncommons, exotics, legendaries;
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
            anim.SetBool("isOpened", true);
        }
    }
    private void BeingOpened()
    {
        GameObject drop = null;
        switch (rarityType)
        {
            case Rarity.Common:
                drop = Instantiate(commons[Random.Range(0, commons.Length)], transform.position, transform.rotation);
                break;
            case Rarity.Uncommon:
                drop = Instantiate(uncommons[Random.Range(0, uncommons.Length)], transform.position, transform.rotation);
                break;
            case Rarity.Exotic:
                drop = Instantiate(exotics[Random.Range(0, exotics.Length)], transform.position, transform.rotation);
                break;
            case Rarity.Legendary:
                drop = Instantiate(legendaries[Random.Range(0, legendaries.Length)], transform.position, transform.rotation);
                break;
        }
        InstanceFinder.ServerManager.Spawn(drop);
        InstanceFinder.ServerManager.Despawn(gameObject);
    }
}

