using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    public Text healthText;
    public Text energyText;
    public Text energyRecText;
    public Text cardDrawText;

    public bool isPlayer = true;

    public int healthMax;
    public int energyMax;
    public int energyStart;
    public int energyRecovery;
    public int cardDraw;

    private int healthCurrent;
    private int energyCurrent;

    void Start()
    {
        healthCurrent = healthMax;
        energyCurrent = energyStart;
    }

    public void UpdateStatus()
    {
        if (isPlayer)
        {
            healthText.text = "Health: " + healthCurrent + "/" + healthMax;
            energyText.text = "Energy:" + energyCurrent + "/" + energyMax;
            energyRecText.text = "Energy Rec: +" + energyRecovery;
            cardDrawText.text = "Card Drawn: " + cardDraw;
        }
        else
        {
            healthText.text = healthCurrent + "/" + healthMax + " :Health";
            energyText.text = energyCurrent + "/" + energyMax + " :Energy";
            energyRecText.text = "+" + energyRecovery + " :Energy Rec";
            cardDrawText.text = cardDraw + " :Card Drawn";
        }
    }
}
