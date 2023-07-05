using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPickups : NetworkBehaviour
{
    public enum StatusType
    {
        Ammo = 0,
        Health = 1,
        Armor = 2
    }
    public StatusType Type;
    [SerializeField] float armorPoint, healAmount;
    private Animator anim;
    private PlayerController touchedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (InstanceFinder.IsServer)
        {
           touchedPlayer = collision.GetComponent<PlayerController>();
            if ((touchedPlayer != null && touchedPlayer.currentHealth < 100) || (touchedPlayer.currentArmor < 100 && touchedPlayer != null))
            {
                Debug.Log("Picked upper's ID is: " + touchedPlayer.gameObject.GetInstanceID()); 
            }
            anim.SetBool("isOpened", true);
        }
    }
    private void OnBeingOpened()
    {
        switch (Type)
        {
            case StatusType.Ammo:
                PlayerManager.instance.PlayerAddAmmo(touchedPlayer.gameObject.GetInstanceID());
            break;
            case StatusType.Health:
                PlayerManager.instance.ReplenishPlayer(25, 0, touchedPlayer.gameObject.GetInstanceID());
                break;
            case StatusType.Armor:
                PlayerManager.instance.ReplenishPlayer(0, 100, touchedPlayer.gameObject.GetInstanceID());
                break;
        }
        InstanceFinder.ServerManager.Despawn(gameObject);
    }
}
