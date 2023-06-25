using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;
using FishNet.Managing.Timing;

public class Guns : NetworkBehaviour
{
    CameraController cameraRef;
    PlayerController playerRef;
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

    private NetworkObject currentUser;
    private ParticleSystem muzzleFX;
    public bool isAutomatic;
    public int currentAmmo;
    private float shotCounter, currSpread;
    private bool isReady = true;
    private int currentUserID;

    private const float MAX_PASSED_TIME = 0.3f;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            muzzleFX = GetComponentInChildren<ParticleSystem>();
            cameraRef = CameraController.instance;
            playerRef = PlayerController.instance;
            currentUser = transform.root.gameObject.GetComponent<NetworkObject>();
            currentUserID = transform.root.gameObject.GetInstanceID();
        }
        else
        {
            //GetComponent<PlayerSkin>().enabled = false;
            return;
        }
    }
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if(playerRef.EPC)
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
                        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && currentAmmo > 0) // Hold-down or Click Mouse 1 to Fire
                        {
                            animator.SetTrigger("shotFired");
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
                        animator.SetTrigger("shotFired");
                    }
                    else
                    {
                        if (currSpread > 0)
                        {
                            currSpread -= spreadRecovery;
                        }
                    }
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
        // Muzzle FX
        if (Random.Range(0, 3) != 0)
        {
            muzzleFX.Play();
        } 
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
        //GameObject bullet = Instantiate(bulletToFire, shootPoint.position, shootPoint.rotation);
        //bullet.GetComponent<BulletLogic>().attackerID = currentUserID;
        if (!IsServer)
        {
           SpawnProjectile(0f);
        }
        ServerSpawnProjectile(base.TimeManager.Tick);
        currentAmmo--;
        updateUIAmmo();
        shotCounter = timeBetweenShots;
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
        shootPoint.rotation = Quaternion.Euler(0f, 0f, 90f);
        updateUIAmmo();
        isReady = true;
    }
    private void OnEnable()
    {
        isReady = false;
        uiControlRef = UIController.instance;
        setUI();
        animator = GetComponent<Animator>();
        animator.SetTrigger("unholster");
        animator.SetFloat("reloadSpeedMult", reloadSpeedMult);
    }

    private void setUI()
    {
        uiControlRef.currentWeapon.sprite = weaponUI;
        uiControlRef.weaponName.text = weaponName;
        updateUIAmmo();
    }

    private void updateUIAmmo()
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
    private void SpawnProjectile(float passedTime)
    {     
        BulletLogic pb = Instantiate(bulletToFire, shootPoint.position, shootPoint.rotation);
        pb.Initialize(transform.up, passedTime, currentUserID);
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
    [ObserversRpc(ExcludeOwner = true)]
    private void ObserversSpawnProjectile(uint Tick)
    {
        Debug.Log("Observer Fired");
        float passedTime = (float)base.TimeManager.TimePassed(Tick, false);
        passedTime = Mathf.Min(MAX_PASSED_TIME, passedTime);
        SpawnProjectile(passedTime);
    }
}
