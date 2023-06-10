using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    
    public enum AmmoType
    {
        Pistol,
        Rifle,
        Shotgun,
        Energy,
        Rocket
    };

    public AmmoType type;
    public static ObjectPool instance;
    public int amountToPool;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
