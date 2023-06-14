using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damageToGive;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage player
    }

    private void OnExplosionEnd()
    {
        Destroy(gameObject);
    }    
}
