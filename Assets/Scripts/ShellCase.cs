using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCase : MonoBehaviour
{
    [SerializeField] private float ejectForce, ejectTorque;
    private Rigidbody2D RB;
    
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        RB.AddForce(transform.right * ejectForce, ForceMode2D.Impulse);
        RB.AddTorque(ejectTorque * Random.value, ForceMode2D.Impulse);
        Destroy(gameObject, 5f);
    }
}
