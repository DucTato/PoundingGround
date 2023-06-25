using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;


public class PlayerController : NetworkBehaviour
{
    UIController uiRef;
    public static PlayerController instance;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed, bodyRotation;
    [SerializeField] private Transform Head,Body,Legs;

    public float currentHealth, currentArmor;
    
    
   
    //
    public List<Guns> availGuns = new List<Guns>();
    public bool EPC = true;
    private Vector2 moveInput;
    private Quaternion rotationAngle;
    private int currentGun;

    private void Awake()
    {
        instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsServer)
        {
            PlayerManager.instance.players.Add(gameObject.GetInstanceID(), new PlayerManager.Player() { currentHealth = 100, currentArmor = 0, playerName = PlayerPrefs.GetString("PlayerName"), connection = GetComponent<NetworkObject>().Owner, playerObject = gameObject });
            
        }
        if (base.IsOwner)
        {
            CameraController.instance.target = transform;
            uiRef = UIController.instance;
            PlayerManager.instance.localClientID = gameObject.GetInstanceID();
            currentArmor = 0;
            currentHealth = 100;
            LocalUICall();
        }
        else
        {
            EPC = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if(EPC)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
            playerRB.velocity = moveInput * moveSpeed;

            // Looking at the Mouse's Position
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2 (mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * 57.29578f - 90f;
            Head.rotation = Quaternion.Euler(0,0,angle);

            rotationAngle = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Legs.rotation = Quaternion.Slerp(Legs.rotation, rotationAngle, bodyRotation * 0.9f * Time.deltaTime);
            Body.rotation = Quaternion.Slerp(Body.rotation, rotationAngle, bodyRotation  * Time.deltaTime);
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
            if (Input.GetKeyDown("q"))
            {
                if (availGuns.Count > 0)
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
                    SwitchWeapon();
                }
                else
                    Debug.LogError("Player has no gun :v");
            }
        }
    }

    public void SwitchWeapon()
    {
        foreach(Guns thegun in availGuns)
        {
            thegun.gameObject.SetActive(false);
        }
        availGuns[currentGun].gameObject.SetActive(true);
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
}
