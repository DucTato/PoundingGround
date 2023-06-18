using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomizedPanelController : MonoBehaviour
{
    [SerializeField] public Image head, body;
    [SerializeField] public Sprite[] imageHeadList;
    [SerializeField] public Sprite[] imageBodyList;
    [SerializeField] public Text headText, bodyText;
    public int currentHead, currentBody;


public void leftBodyButton() {
   
    currentBody --;
    if (currentBody < 0) {
        currentBody = imageBodyList.Length - 1;
    }
    body.sprite = imageBodyList[currentBody];
    
    bodyText.text = "Body :        " + currentBody.ToString();
}
public void rightBodyButton() {
   
    currentBody ++;
      if (currentBody > imageBodyList.Length){
        currentBody = 0;
    }
    body.sprite = imageBodyList[currentBody];
  
    bodyText.text = "Body :        " + currentBody.ToString();
}
public void leftHeadButton() {
   
    currentHead --;
    if (currentHead < 0) {
        currentHead = imageHeadList.Length -1;
    }
    head.sprite = imageHeadList[currentHead];
    
    headText.text = "Head :        " + currentHead.ToString();
}
public void rightHeadButton() {
   
    currentHead ++;
    if (currentHead > imageHeadList.Length){
        currentHead = 0;
    }
    head.sprite = imageHeadList[currentHead];
    
    headText.text = "Head :        " + currentHead.ToString();
}
    // Start is called before the first frame update
    void Start()
    {
        headText.text = "Head :        " + currentHead.ToString();
        bodyText.text = "Body :        " + currentBody.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
