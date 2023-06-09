using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;

public class BulletLogic : MonoBehaviour
{
    //private Rigidbody2D bulletRB; // Declaration
    [SerializeField] private float moveSpeed, damageToGive;
    [SerializeField]
    [Tooltip("Armor Multiplier 0 = Ignore armor, 1 = armor blocks off damage as much as possible")]
    private float ignoreArmorMult;
    [SerializeField] private bool isExplosive = false;
    [SerializeField] private GameObject Explosion;
    [SerializeField] private int targetID;

    private Vector3 _direction;
    private float _passedTime = 0f;
    public int attackerID;
    // Start is called before the first frame update
    void Start()
    {
        
       //bulletRB = GetComponent<Rigidbody2D>(); // Value
        
    }

    // Update is called once per frame
    void Update()
    {
        //bulletRB.velocity = transform.right * moveSpeed;
        Move();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExplosive)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
        }
        
        if (InstanceFinder.IsServer)
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                Debug.Log("Target's ID is: " + pc.gameObject.GetInstanceID());
                PlayerManager.instance.DamagePlayer(attackerID, damageToGive, ignoreArmorMult, pc.gameObject.GetInstanceID());
            }
        }
        //if (collision.tag == "Player")
        //{
        //    HitPlayer(collision.transform.gameObject);
        //}
        Destroy(gameObject);
        
    }
    //[ServerRpc(RequireOwnership = false)]
    //public void HitPlayer(GameObject playerHit)
    //{
    //    PlayerManager.instance.DamagePlayer(attackerID, damageToGive, ignoreArmorMult, playerHit.GetInstanceID());
    //}
    public void Initialize(Vector3 direction, float passedTime, int shooterID)
    {
        _direction = direction;
        _passedTime = passedTime;
        attackerID = shooterID;
    }
    private void Move()
    {
        float delta = Time.deltaTime;
        float passedTimeDelta = 0f;
        if (_passedTime > 0f)
        {
            float step = (_passedTime * 0.08f);
            _passedTime -= step;
            if (_passedTime <= (delta / 2f))
            {
                step += _passedTime;
                _passedTime = 0f;
            }
            passedTimeDelta = step;
        }
        transform.position += _direction * (moveSpeed * (delta + passedTimeDelta));
    }
}
