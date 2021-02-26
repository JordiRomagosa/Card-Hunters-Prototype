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

    public void UpdateStatus(Character character)
    {
        if (isPlayer)
        {
            healthText.text = "Health: " + character.healthCurrent + "/" + character.healthMax;
            energyText.text = "Energy:" + character.energyCurrent + "/" + character.energyMax;
            energyRecText.text = "Energy Rec: +" + character.energyRecovery;
            cardDrawText.text = "Card Drawn: " + character.cardDraw;
        }
        else
        {
            healthText.text = character.healthCurrent + "/" + character.healthMax + " :Health";
            energyText.text = character.energyCurrent + "/" + character.energyMax + " :Energy";
            energyRecText.text = "+" + character.energyRecovery + " :Energy Rec";
            cardDrawText.text = character.cardDraw + " :Card Drawn";
        }
    }
}
