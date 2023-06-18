using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject customizePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void customPanelBackToMainButton()
    {
        customizePanel.gameObject.SetActive(false);
    }
    public void CustomizeButton() 
    {
        customizePanel.gameObject.SetActive(true);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}