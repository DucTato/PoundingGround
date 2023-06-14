using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] private Sprite[] Heads, Bodies;
    [SerializeField] private SpriteRenderer HeadSR, BodySR;
    public int currHead, currBody;
    // Start is called before the first frame update
    void Start()
    {
        HeadSR.sprite = Heads[currHead];
        BodySR.sprite = Bodies[currBody];   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
