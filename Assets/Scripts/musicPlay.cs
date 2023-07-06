using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPlay : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                else
                {
                    audioSource.Play();
                }
            }
        }
    }
}
