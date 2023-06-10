using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    public int magAmmo, resAmmo; // Magazine ammo and Max ammo
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private GameObject bulletToFire;
    [SerializeField] private Transform shootPoint; 
    [SerializeField] private float reloadSpeedMult;
    private ParticleSystem muzzleFX;
    private Animator animator;
    public int currentAmmo;
    public bool isAutomatic;
    private float shotCounter;
    private bool isReady = true;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magAmmo;
        animator = GetComponent<Animator>();
        muzzleFX = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {
                if (isAutomatic)
                {
                    if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && currentAmmo > 0) // hoac la click chuot, hoac la giu chuot = Fire
                    {
                        // Muzzle FX
                        if (Random.Range(0, 3) != 0)
                        {
                            muzzleFX.Play();
                        }
                        // spawn vien dan 
                        Instantiate(bulletToFire, shootPoint.position, shootPoint.rotation);
                        animator.SetTrigger("shotFired");
                        currentAmmo--;
                        shotCounter = timeBetweenShots;
                    }
                }
                else if (Input.GetMouseButtonDown(0) && currentAmmo > 0) // click chuot = Fire
                {
                    // Muzzle FX
                    if (Random.Range(0, 3) != 0)
                    {
                        muzzleFX.Play();
                    }
                    // spawn vien dan 
                    Instantiate(bulletToFire, shootPoint.position, shootPoint.rotation);
                    animator.SetTrigger("shotFired");
                    currentAmmo--;
                    shotCounter = timeBetweenShots;
                }
            }


            if (currentAmmo < magAmmo)
            {
                if (Input.GetKeyDown("r") && resAmmo != 0)
                {
                    animator.SetTrigger("startReload");
                    animator.SetFloat("reloadSpeedMult", reloadSpeedMult);
                }
            }
        }     

        
    }
    private void onReloadStart()
    {
        isReady = false;
    }
    private void onReloadEnd()
    {
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
        isReady = true;
    }
}
