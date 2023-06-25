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
    public Text weaponName, currentAmmo, healthText, armorText;
    public Slider healthBar, armorBar;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = 100f;
        armorBar.maxValue = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
