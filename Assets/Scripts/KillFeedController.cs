using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedController : MonoBehaviour
{
    //[SerializeField] private GameObject[] KillTextPrefabs;
    //public List<GameObject> KillTextPrefabs = new List<GameObject>();
    public KillFeedText[] killFeedText;
    
    // Start is called before the first frame update
    void Start()
    {

        //for (int i = 0; i < KillTextPrefabs.Count; i++)
        //{
        //    Debug.Log(KillTextPrefabs[i] + "i = " + i);
        //    killFeedText[i] = KillTextPrefabs[i].GetComponent<KillFeedText>();
        //}

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    public void NewKillFeed(string Feed)
    {
        Debug.Log("New Kill Feed edited in");
        for (int i = 0; i < killFeedText.Length; i++)
        {
            if (killFeedText[i].killFeedText.text == "")
            {
                killFeedText[i].AddFeedText(Feed);
                return;
            }
        }
        // If all 4 lines are occupied, move all lines up 1 by 1 then set text on the latest one
        ReOrderKillFeed();
        killFeedText[3].AddFeedText(Feed);
    }
    public void ReDrawKillFeed()
    {
        for (int i = 0; i < killFeedText.Length - 1; i++)
        {
            if (killFeedText[i].killFeedText.text == "" && killFeedText[i + 1].killFeedText.text != "")
            {
                killFeedText[i].AddFeedText(killFeedText[i + 1].killFeedText.text);
                killFeedText[i + 1].AddFeedText("");
            }
            else
                return;
        }
    }
    private void ReOrderKillFeed()
    {
        for (int i = 0; i < killFeedText.Length - 1; i++)
        {
            killFeedText[i].AddFeedText(killFeedText[i + 1].killFeedText.text);
        }
    }
}
