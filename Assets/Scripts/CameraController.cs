using System.Collections;
using System.Collections.Generic;
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
        // Following the player
        
        if (Input.GetMouseButton(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
             // Move the Camera
             transform.position = Vector3.Lerp(transform.position, new Vector3(hit.point.x, hit.point.y, -10f), Time.deltaTime * camSpeed);
            }
        }
        else if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10f);
        }
    }
}
