using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomizedPanelController : MonoBehaviour
{
    [SerializeField] private Image head, body;
    [SerializeField] private Sprite[] imageHeadList;
    [SerializeField] private Sprite[] imageBodyList;
    [SerializeField] private Text headText, bodyText;
    [SerializeField] private InputField enterNameText;
    private int currentHead, currentBody;
    public string userInput;


    public void leftBodyButton() {
   
        currentBody --;
        if (currentBody < 0) {
            currentBody = imageBodyList.Length - 1;
        }
        body.sprite = imageBodyList[currentBody];
        //Display current Head & Body part
        bodyText.text = "Body :        " + (currentBody + 1).ToString();
    }
    public void rightBodyButton() {
   
        currentBody ++;
          if (currentBody > imageBodyList.Length-1){
            currentBody = 0;
        }
        body.sprite = imageBodyList[currentBody];
        //Display current Head & Body part
        bodyText.text = "Body :        " + (currentBody + 1).ToString();
    }
    public void leftHeadButton() {
   
        currentHead --;
        if (currentHead < 0) {
            currentHead = imageHeadList.Length -1;
        }
        head.sprite = imageHeadList[currentHead];
        //Display current Head & Body part
        headText.text = "Head :        " + (currentHead + 1).ToString();
    }
    public void rightHeadButton() {
   
        currentHead ++;
        if (currentHead > imageHeadList.Length-1){
            currentHead = 0;
        }
        head.sprite = imageHeadList[currentHead];
        //Display current Head & Body part
        headText.text = "Head :        " + (currentHead + 1).ToString();
    }
    public void randomButton() {
        currentHead = Random.Range(0, imageHeadList.Length);
        currentBody = Random.Range(0, imageBodyList.Length);
        headText.text = "Head :        " + (currentHead + 1).ToString();
        bodyText.text = "Body :        " + (currentBody + 1).ToString();
        head.sprite = imageHeadList[currentHead];
        body.sprite = imageBodyList[currentBody];
    }
    public void userNameInput(string userName) {
        userInput = userName;
        //Debug.Log(userInput);
    }
    
    public void saveButton() {
        PlayerPrefs.SetInt("Head", currentHead);
        PlayerPrefs.SetInt("Body", currentBody);
        PlayerPrefs.SetString("PlayerName", userInput);
        PlayerPrefs.Save();
        //Debug.Log("Saved with UserName: " + userInput);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Reads from saved values in PlayerPrefs class
        if (PlayerPrefs.HasKey("Head")) {
            currentHead = PlayerPrefs.GetInt("Head");
        }
        else
            currentHead = 0;
        if (PlayerPrefs.HasKey("Body")) {
            currentBody = PlayerPrefs.GetInt("Body");
        }
        else
            currentBody = 0;
        if (PlayerPrefs.HasKey("PlayerName")) {
            userInput = PlayerPrefs.GetString("PlayerName");
            enterNameText.text = userInput;
            //Debug.Log("Loaded UserName: " + userInput);
        }
        else
            userInput = "Player";
        

        //Displays default value
        head.sprite = imageHeadList[currentHead];
        headText.text = "Head :        " + (currentHead + 1).ToString();
        body.sprite = imageBodyList[currentBody];
        bodyText.text = "Body :        " + (currentBody + 1).ToString();
    }
}
