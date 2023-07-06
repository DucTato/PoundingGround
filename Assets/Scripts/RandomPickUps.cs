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
    private float timeToPickup = 0.5f;

    [SyncVar]
    public Rarity rarityType;
    [field: SyncVar]
    public int itemToDrop { get; [ServerRpc(RequireOwnership = false, RunLocally = true)] set; }
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
        if (timeToPickup > 0)
        {
            timeToPickup -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (InstanceFinder.IsServer)
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null && timeToPickup < 0)
            anim.SetBool("isOpened", true);
        }
    }
    private void BeingOpened()
    {
        GameObject drop = null;
        switch (rarityType)
        {
            case Rarity.Common:
                itemToDrop = Random.Range(0, commons.Length);
                drop = Instantiate(commons[itemToDrop], transform.position, transform.rotation);
                break;
            case Rarity.Uncommon:
                itemToDrop = Random.Range(0, uncommons.Length);
                drop = Instantiate(uncommons[itemToDrop], transform.position, transform.rotation);
                break;
            case Rarity.Exotic:
                itemToDrop = Random.Range(0, exotics.Length);
                drop = Instantiate(exotics[itemToDrop], transform.position, transform.rotation);
                break;
            case Rarity.Legendary:
                itemToDrop = Random.Range(0, legendaries.Length);
                drop = Instantiate(legendaries[itemToDrop], transform.position, transform.rotation);
                break;
        }
        InstanceFinder.ServerManager.Spawn(drop);
        InstanceFinder.ServerManager.Despawn(gameObject);
    }
}

