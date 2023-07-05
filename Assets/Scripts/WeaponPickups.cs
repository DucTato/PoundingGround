using FishNet;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickups : NetworkBehaviour
{
    [SerializeField] private float ejectForce, ejectTorque;
    [SerializeField] private GameObject weapon;
    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private bool canPickUp;
    private PlayerController touchedPlayer;
    private float timeToPickup = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        switch (Random.Range (0, 4))
        {
            case 0:
                RB.AddForce(transform.right * ejectForce, ForceMode2D.Impulse);
                break;
            case 1:
                RB.AddForce(transform.right * -ejectForce, ForceMode2D.Impulse);
                break;
            case 2:
                RB.AddForce(transform.up * ejectForce, ForceMode2D.Impulse);
                break;
            case 3:
                RB.AddForce(transform.up * -ejectForce, ForceMode2D.Impulse);
                break;
        }
        RB.AddTorque(ejectTorque * Random.value, ForceMode2D.Impulse);
        if (Random.Range(0,2) == 1) 
        {
            SR.flipY = true;
        }
        else
        {
            SR.flipY = false;
        }
        if (Random.Range(0, 2) == 1)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (canPickUp)
        {
            if (timeToPickup > 0) 
            {
                timeToPickup -= Time.deltaTime;
            }
            else
            {
                if (touchedPlayer != null && touchedPlayer.pickingUpWeapon)
                { 
                    Debug.Log("Player: " + touchedPlayer.gameObject.GetInstanceID() + " picked up a weapon");
                    PlayerManager.instance.PlayerAddWeapon(touchedPlayer.gameObject.GetInstanceID(), weapon);
                    InstanceFinder.ServerManager.Despawn(gameObject);
                    timeToPickup = 0.5f;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (InstanceFinder.IsServer)
        {
            touchedPlayer = collision.GetComponent<PlayerController>();
            if (touchedPlayer != null)
            {
                
                canPickUp = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (InstanceFinder.IsServer)
        { 
            if (touchedPlayer != null)
            {
                canPickUp = false;
                //touchedPlayer = null;
            }
        }
    }
}
