using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet;


public class PlayerController : NetworkBehaviour
{
    UIController uiRef;
    public static PlayerController instance;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed, bodyRotation;
    [SerializeField] private Transform Head, Body, Legs;

    public GameObject spawnedGun;
    public float currentHealth, currentArmor;
    public Transform gunPoint;

    //[SyncObject]
    //public readonly SyncList<Guns> availGuns = new SyncList<Guns>();
    public List<Guns> availGuns = new List<Guns>();
    public Guns currentUsedGun;
    public bool EPC = true;
    private Vector2 moveInput;
    private Quaternion rotationAngle;
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public int currentGun { get; [ServerRpc(RunLocally = true)] set; }
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public bool pickingUpWeapon { get; [ServerRpc(RunLocally = true)] set; }
    private void Awake()
    {
        instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            currentGun = 0;
            //availGuns.Add(gameObject.GetComponentInChildren<Guns>());
            currentUsedGun = availGuns[currentGun].GetComponent<Guns>();
            SetOwnerShipGun();
            CameraController.instance.target = transform;
            uiRef = UIController.instance;
            uiRef.playerScript = GetComponent<PlayerController>();
            PlayerManager.instance.localClientID = gameObject.GetInstanceID();
            currentArmor = 0;
            currentHealth = 100;
            LocalUICall();
            LocalScoreUI(0);
        }
        else
        {
            EPC = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        if (base.IsServer)
        {
            PlayerManager.instance.players.Add(gameObject.GetInstanceID(), new PlayerManager.Player() { currentHealth = 100, currentArmor = 0, playerName = GetComponent<PlayerSkin>().currPlayerName, connection = GetComponent<NetworkObject>().Owner, playerObject = gameObject });
            Debug.Log("Added a Player to Dictionary");

        }

    }
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (EPC)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
            playerRB.velocity = moveInput * moveSpeed;

            // Looking at the Mouse's Position
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * 57.29578f - 90f;
            Head.rotation = Quaternion.Euler(0, 0, angle);

            rotationAngle = Quaternion.AngleAxis(angle, Vector3.forward);

            Legs.rotation = Quaternion.Slerp(Legs.rotation, rotationAngle, bodyRotation * 0.9f * Time.deltaTime);
            Body.rotation = Quaternion.Slerp(Body.rotation, rotationAngle, bodyRotation * Time.deltaTime);
            // Animating Player's Movement
            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);

            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            // Press Q to switch between current weapons (if more than 1 is available)
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (availGuns.Count > 1)
                {
                    // Rebind the animator state so that it doesn't get stuck (in case it happens to be playing another animation)
                    availGuns[currentGun].animator.Rebind();
                    availGuns[currentGun].animator.Update(0f);
                    // Switch gun
                    currentGun++;
                    if (currentGun >= availGuns.Count)
                    {
                        currentGun = 0;
                    }
                    ServerSwitchWeapon();
                }
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                pickingUpWeapon = true;
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                pickingUpWeapon = false;
            }
        }
    }
   
    
    [ServerRpc]
    public void ServerSwitchWeapon()
    {
        ObserversSwitchWeapon();
    }
    [ObserversRpc(ExcludeOwner = false, ExcludeServer = false, BufferLast = true)]
    private void ObserversSwitchWeapon()
    {
        foreach (Guns thegun in availGuns)
        {
            thegun.gameObject.SetActive(false);
        }
        availGuns[currentGun].gameObject.SetActive(true);
        currentUsedGun = availGuns[currentGun].GetComponent<Guns>();
    }
    public void LocalUICall()
    {

        uiRef.healthBar.value = currentHealth;
        uiRef.healthText.text = currentHealth.ToString();

        uiRef.armorBar.value = currentArmor;
        uiRef.armorText.text = currentArmor.ToString();
        if (currentArmor <= 0)
        {
            uiRef.armorBar.gameObject.SetActive(false);
        }
        else
            uiRef.armorBar.gameObject.SetActive(true);
    }
    public void LocalKillFeedCall(string Feed)
    {
        Debug.Log("Local Kill Feed called: " + Feed);
        uiRef.NewKill(Feed);
    }

    public void LocalScoreUI(int Score)
    {
        uiRef.progressBar.value = Score;
        uiRef.progressText.text = Score.ToString();
    }
    [ServerRpc]
    private void SetOwnerShipGun()
    {
        availGuns[currentGun].gameObject.GetComponent<NetworkObject>().GiveOwnership(base.Owner);
        //availGuns.Dirty(currentGun);
    }
    public void LocalAddWeapon(GameObject weapon)
    {
        //GameObject gunClone = Instantiate(weapon);
        //InstanceFinder.ServerManager.Spawn(gunClone, base.Owner);
        //gunClone.transform.parent = gunPoint;
        //gunClone.transform.position = gunPoint.position;
        //gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //Guns newGun = gunClone.GetComponent<Guns>();
        //availGuns.Add(newGun);
        //currentGun = availGuns.Count - 1;
        //SwitchWeapon();
        Debug.Log("Local weapon:" + weapon);
        SpawnGun(weapon);


    }
    [ServerRpc]
    public void SpawnGun(GameObject weapon)
    {
        Debug.Log("ServerRpc called 1");
        GameObject gunClone = Instantiate(weapon);
        base.Spawn(gunClone, base.Owner);
        ObserverSpawnGun(gunClone);
    }
    

    [ObserversRpc(ExcludeOwner = false, ExcludeServer = false, BufferLast = true)]
    public void ObserverSpawnGun(GameObject spawned)
    {
        spawned.transform.parent = gunPoint;
        spawned.transform.position = gunPoint.position;
        spawned.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Guns newGun = spawned.GetComponent<Guns>();
        availGuns.Add(newGun);
        currentGun = availGuns.Count - 1;
        ServerSwitchWeapon();
    }
}
