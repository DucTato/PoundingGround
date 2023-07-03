using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Image currentWeapon, isEmpty, hasAmmo;
    public Text weaponName, currentAmmo, healthText, armorText, progressText;
    public Slider healthBar, armorBar, progressBar;
    public GameObject KillFeedComponent;
    private KillFeedController killFeed;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = 100f;
        armorBar.maxValue = 100f;
        progressBar.maxValue = 5;
        killFeed = KillFeedComponent.GetComponent<KillFeedController>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
    public void NewKill(string Feed)
    {
        killFeed.NewKillFeed(Feed);
    }
}
