using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    public int magAmmo, resAmmo; // Magazine ammo and Max ammo
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private GameObject bulletToFire;
    [SerializeField] private GameObject spentCasings, muzzleFX;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Animator animator;
    public int currentAmmo;
    public bool isAutomatic;
    private float shotCounter;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutomatic)
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) && currentAmmo > 0) // hoac la click chuot, hoac la giu chuot = Fire
                {
                    // spawn vien dan 
                    if (Random.Range(0, 3) == 1)
                    {
                        Instantiate(muzzleFX, shootPoint.position, shootPoint.rotation);
                    }
                    Instantiate(bulletToFire, shootPoint.position, shootPoint.rotation);
                    Instantiate(spentCasings, transform.position, transform.rotation);
                    currentAmmo--;
                    shotCounter = timeBetweenShots;
                }
            }
        }
        else
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && currentAmmo > 0) // click chuot = Fire
                {
                    // spawn vien dan 
                    if (Random.Range(0, 3) == 1)
                    {
                        Instantiate(muzzleFX, shootPoint.position, shootPoint.rotation);
                    }
                    Instantiate(bulletToFire, shootPoint.position, shootPoint.rotation);
                    Instantiate(spentCasings, transform.position, transform.rotation);
                    currentAmmo--;
                    shotCounter = timeBetweenShots;
                }
            }
        }
        if (currentAmmo < magAmmo)
        {
            if (Input.GetKeyDown("r") && resAmmo != 0)
            {
                //animator.SetBool();
                if (magAmmo < resAmmo + currentAmmo)
                {
                    
                    resAmmo = resAmmo - (magAmmo - currentAmmo);
                    currentAmmo = magAmmo;
                }
                else
                {
                    currentAmmo = currentAmmo + resAmmo;
                    resAmmo = 0;
                }
            }
        }
    }
}
