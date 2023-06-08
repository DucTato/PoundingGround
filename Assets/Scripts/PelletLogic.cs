using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletLogic : MonoBehaviour
{
    [SerializeField] private float pelletSpread;
    [SerializeField] private GameObject bulletsToShoot;
    [SerializeField] private int numberOfBullets;
    // Start is called before the first frame update
    void Start()
    {
       
        Destroy(gameObject, 5f); // Automatically destroys this pellet after 5 seconds
        for (int i = 0; i < numberOfBullets; i++)
        {
            Debug.Log(transform.rotation);
            if (i % 2 != 0)
            {
                Instantiate(bulletsToShoot, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(bulletsToShoot, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, Random.Range(-pelletSpread, pelletSpread)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
