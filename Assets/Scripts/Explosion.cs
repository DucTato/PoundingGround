using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damageToGive;
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // Damage player
    }

    private void OnExplosionEnd()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(gameObject);
        
    }    
}
