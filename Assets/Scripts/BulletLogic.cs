using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    private Rigidbody2D bulletRB; // Declaration
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isExplosive = false;
    [SerializeField] private GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        
        bulletRB = GetComponent<Rigidbody2D>(); // Value
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletRB.velocity = transform.right * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExplosive)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
