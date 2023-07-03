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
    private Animator anim;
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
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                Debug.Log("Picked upper's ID is: " + pc.gameObject.GetInstanceID());
                //PlayerManager.instance.DamagePlayer(attackerID, damageToGive, ignoreArmorMult, pc.gameObject.GetInstanceID());
            }
            anim.SetBool("isOpened", true);
        }
    }
    private void OnBeingOpened()
    {
        switch (Type)
        {
            case StatusType.Ammo:
            break;
            case StatusType.Health:
            break;
            case StatusType.Armor:
            break;
        }
        InstanceFinder.ServerManager.Despawn(gameObject);
    }
}
