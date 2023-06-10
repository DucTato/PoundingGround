using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;   
    public static CameraController instance;
    public Transform target;
    [SerializeField] private float camSpeed;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetMouseButton(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                // Move the Camera
                //Debug.Log("Hit the ground");
                transform.position = Vector3.Lerp(transform.position, new Vector3(hit.point.x, hit.point.y, -10f), Time.deltaTime * camSpeed);
            }
        }
        else if (target != null)
        {
            // Following the player
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, -10f), 2f * camSpeed * Time.deltaTime);
        }
    }
}
