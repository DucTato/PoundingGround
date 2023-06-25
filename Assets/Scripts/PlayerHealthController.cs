using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    UIController uiControlRef;
    [SerializeField] private float currentHP, currentAP;
    // Start is called before the first frame update
    void Start()
    {
        uiControlRef = UIController.instance;
        currentHP = 100f;
        currentAP = 0f;
        UpdateStatusUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamagePlayer(float damage, float armorMult)
    {
        if (currentHP <= 0) 
        {
            //Dead :D
        }
        if (currentAP > 0) 
        {
            currentAP -= damage;
            currentHP -= (damage - currentAP * armorMult);
        }
        else
        {
            currentHP -= damage;
        }
        UpdateStatusUI();
    }
    public void UpdateStatusUI()
    {
        uiControlRef.healthBar.maxValue = 100f;
        uiControlRef.healthBar.value = currentHP;
        uiControlRef.healthText.text = currentHP.ToString();
        uiControlRef.armorBar.maxValue = 100f;
        uiControlRef.armorBar.value = currentAP;
        uiControlRef.armorText.text = currentAP.ToString();
        if (currentAP <= 0)
        {
            uiControlRef.armorBar.gameObject.SetActive(false);
        }
        else
            uiControlRef.armorBar.gameObject.SetActive(true);
    }
}
