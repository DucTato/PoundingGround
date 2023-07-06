using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;
using FishNet.Component.Animating;

public class Guns : NetworkBehaviour
{
    CameraController cameraRef;
    //PlayerController playerRef;
    UIController uiControlRef;
    public int magAmmo, resAmmo; // Magazine ammo and Max ammo
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private GameObject alternateState, ammoCasing;
    [SerializeField] private Transform shootPoint; 
    [SerializeField] private float reloadSpeedMult, zoomInMax;
    [SerializeField] private bool hasAlternateState;
    [SerializeField] private bool hasCasing;
    [SerializeField] private float maxSpread, spreadMult, spreadRecovery;
    [SerializeField] private BulletLogic bulletToFire;
    public string weaponName;
    public Sprite weaponUI;
    public Animator animator;

    public NetworkAnimator netAnimator;
    private ParticleSystem muzzleFX;
    public bool isAutomatic;
    public int currentAmmo;
    private float shotCounter, currSpread;
    private bool isReady;
    private int currentUserID;

    private const float MAX_PASSED_TIME = 0.3f;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            Debug.Log("Weapon: " + weaponName + " started");
            muzzleFX = GetComponentInChildren<ParticleSystem>();
            isReady = true;
            uiControlRef = UIController.instance;
            cameraRef = CameraController.instance;

            animator = GetComponent<Animator>();
            netAnimator = GetComponent<NetworkAnimator>();
            netAnimator.SetTrigger("unholster");
            animator.SetFloat("reloadSpeedMult", reloadSpeedMult);
            setUI();
            SetCurrentUserID();
        }
        else
        {
            muzzleFX = GetComponentInChildren<ParticleSystem>();
            currentUserID = transform.root.gameObject.GetInstanceID();
            //GetComponent<Guns>().enabled = false;
            return;
        }
    }
    private void Start()
    {
        Debug.Log("Start() called");
        animator = GetComponent<Animator>();
        netAnimator = GetComponent<NetworkAnimator>();
        netAnimator.SetTrigger("unholster");
        animator.SetFloat("reloadSpeedMult", reloadSpeedMult);
        setUI();
        //SetCurrentUserID();

    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerRef);

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
                    if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && currentAmmo > 0) // Hold-down or Click Mouse 1 to Fire
                    {
                        netAnimator.SetTrigger("shotFired");
                    }
                    else
                    {
                        if (currSpread > 0)
                        {
                            currSpread -= spreadRecovery;
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && currentAmmo > 0) // click chuot = Fire
                {
                    netAnimator.SetTrigger("shotFired");
                }
                else
                {
                    if (currSpread > 0)
                    {
                        currSpread -= spreadRecovery;
                    }
                }
            }

            if (currentAmmo < magAmmo)
            {
                if (Input.GetKeyDown("r") && resAmmo != 0)
                {
                    netAnimator.SetTrigger("startReload");
                    animator.SetFloat("reloadSpeedMult", reloadSpeedMult);
                }
            }
        }
        if (cameraRef!= null)
        {
            if (Input.GetMouseButton(1))
            {
                float currentZoom = cameraRef.mainCamera.orthographicSize;
                cameraRef.mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, zoomInMax, Time.deltaTime * 10);
            }
            else
            {
                float currentZoom = cameraRef.mainCamera.orthographicSize;
                cameraRef.mainCamera.orthographicSize = Mathf.MoveTowards(currentZoom, 13f, Time.deltaTime * 10);
            }
        }
            
        
        // Update the visual state of the current gun
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
    }
    private void onShotFired()
    {
        if (IsOwner)
        { 
            if (currSpread < maxSpread)
            {
                currSpread += spreadMult;
            }
            // Weapon S p r e a d  :>
            if (currentAmmo % 2 != 0)
            {
                shootPoint.rotation = transform.rotation * Quaternion.Euler(0f, 0f, Random.Range(0f, currSpread) + 90f);
            }
            else
            {
                shootPoint.rotation = transform.rotation * Quaternion.Euler(0f, 0f, Random.Range(0f, -currSpread) + 90f);
            }
            // Spawn bullets 
            if (!IsServer)
            {
                SpawnProjectile(0f); // Spawns a local Projectile
            }
            ServerSpawnProjectile(base.TimeManager.Tick);
            currentAmmo--;
            if (currentAmmo < 0) {currentAmmo = 0;}
            updateUIAmmo();
            shotCounter = timeBetweenShots;
        }
        // Muzzle FX
        if (Random.Range(0, 3) != 0)
        {
            muzzleFX.Play();
        }
        // Shell Ejection Effects
        if (hasCasing)
        {
            Instantiate(ammoCasing, transform.position, transform.rotation);
        }
    }
    private void onReloadStart()
    {
        isReady = false;
    }
    private void onUnholsterEnd()
    {
        isReady = true;
        animator.Rebind();
        animator.Update(0f);
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
        currSpread = 0;
        //shootPoint.rotation = Quaternion.Euler(0f, 0f, 90f);
        updateUIAmmo();
        isReady = true;
    }
    private void OnEnable()
    {
        isReady = false;
        setUI();
        animator = GetComponent<Animator>();
        netAnimator = GetComponent<NetworkAnimator>();
        netAnimator.SetTrigger("unholster");
        animator.SetFloat("reloadSpeedMult", reloadSpeedMult);
        Debug.Log(netAnimator);
    }

    private void setUI()
    {
        if (uiControlRef != null)
        {
            uiControlRef.currentWeapon.sprite = weaponUI;
            uiControlRef.weaponName.text = weaponName;
            updateUIAmmo();
        }
    }

    private void updateUIAmmo()
    {
        if (uiControlRef != null)
        {
            uiControlRef.currentAmmo.text = currentAmmo + " / " + resAmmo;
            if (currentAmmo == 0)
            {
                uiControlRef.isEmpty.gameObject.SetActive(true);
            }
            else
                uiControlRef.isEmpty.gameObject.SetActive(false);
            if (uiControlRef.currentAmmo.text != null)
            {
                uiControlRef.hasAmmo.gameObject.SetActive(true);
            }
        }
    }
    private void SpawnProjectile(float passedTime)
    {     
        BulletLogic pb = Instantiate(bulletToFire, shootPoint.position, shootPoint.rotation);
        pb.Initialize(shootPoint.right, passedTime, currentUserID);
    }
    [ServerRpc]
    private void ServerSpawnProjectile(uint Tick)
    {
        Debug.Log(Tick);
        
        float passedTime = (float)base.TimeManager.TimePassed(Tick, false);
        passedTime = Mathf.Min(MAX_PASSED_TIME / 2f, passedTime);
        Debug.Log(passedTime);
        SpawnProjectile(passedTime);
        ObserversSpawnProjectile(Tick);
    }
    [ObserversRpc(ExcludeOwner = true, ExcludeServer = true)]
    private void ObserversSpawnProjectile(uint Tick)
    {
        Debug.Log("Observer Fired");
        float passedTime = (float)base.TimeManager.TimePassed(Tick, false);
        passedTime = Mathf.Min(MAX_PASSED_TIME, passedTime);
        SpawnProjectile(passedTime);
    }
    public void ReplenishAmmo()
    {
        resAmmo += 2 * magAmmo;
        updateUIAmmo();
    }
    [ServerRpc]
    public void SetCurrentUserID()
    {
        currentUserID = transform.root.gameObject.GetInstanceID();
        ObserverSetCurrentUserId(currentUserID);
    }
    [ObserversRpc(ExcludeOwner = false, ExcludeServer = false, BufferLast = true)]
    public void ObserverSetCurrentUserId(int UserID)
    {
        currentUserID = UserID;
    }
}
