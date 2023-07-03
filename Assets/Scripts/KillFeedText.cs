using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedText : MonoBehaviour
{
    KillFeedController feedController;
    public Text killFeedText;
    private float timerCount;
    
    // Start is called before the first frame update
    void Start()
    {
        killFeedText= GetComponent<Text>();
        timerCount = 6f;       
        feedController = UIController.instance.KillFeedComponent.GetComponent<KillFeedController>();
        Debug.Log(feedController);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (timer > 0)
    //    {
    //        timer -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        killFeedText.text = null;
    //        feedController.ReDrawKillFeed();
    //    }
    //}
    public void AddFeedText(string Feed)
    {
        // Stop the counting down before setting new value
        StopCoroutine(WaitAndGoAway());
        // Set kill feed text then wait 10 seconds then make it null
        Debug.Log("Add feed text");
        killFeedText.text = Feed;
        StartCoroutine(WaitAndGoAway());   
    }
    private IEnumerator WaitAndGoAway()
    {
        yield return new WaitForSeconds(timerCount);
        killFeedText.text = "";
        feedController.ReDrawKillFeed();
    }
}
