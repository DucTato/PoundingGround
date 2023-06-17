using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController instance;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed, bodyRotation;
    [SerializeField] private Transform Head,Body,Legs;
    public bool EPC = true;
    private Vector2 moveInput;
    private Quaternion rotationAngle;

    private void Awake()
    {
        instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            CameraController.instance.target = transform;
        }
        else
        {
            EPC = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //EPC = true;
    }

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
        }
    }
}
