using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    private Rigidbody2D bulletRB; // Declaration
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10f;
        bulletRB = GetComponent<Rigidbody2D>(); // Value
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletRB.velocity = transform.right * moveSpeed;
    }
}
