using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    CameraController cameraRef;
    public int magAmmo, resAmmo; // Magazine ammo and Max ammo
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private GameObject bulletToFire, alternateState, ammoCasing;
    [SerializeField] private Transform shootPoint; 
    [SerializeField] private float reloadSpeedMult, zoomInMax;
    [SerializeField] private bool hasAlternateState;
    [SerializeField] private bool hasCasing;
    
    private ParticleSystem muzzleFX;
    private Animator animator;
    public bool isAutomatic;
    public int currentAmmo;
    private float shotCounter;
    private bool isReady = true;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magAmmo;
        animator = GetComponent<Animator>();
        muzzleFX = GetComponentInChildren<ParticleSystem>();
        cameraRef = CameraController.instance;
       
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
        if (hasAlternateState)
        {
            if (currentAmmo == 0)
            {
                alternateState.SetActive(false);
            }
            else
            {
                alternateState.SetActive(true);
            }
        }
        if (Input.GetMouseButton(1))
        {
            float currentZoom = cameraRef.mainCamera.orthographicSize;
            cameraRef.mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, zoomInMax, Time.deltaTime * 10);
        }
        else
        {
            float currentZoom = cameraRef.mainCamera.orthographicSize;
            cameraRef.mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, 7f, Time.deltaTime * 10);

        }
    }
    private void onShotFired()
    {
        if (hasCasing)
        {
            Instantiate(ammoCasing, transform.position, transform.rotation);
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
