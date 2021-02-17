using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUpdater : MonoBehaviour
{
    public Card card;

    public bool isSelected = false;

    public void UpdateCardValues()
    {
        transform.Find("EnergyCost/Cost").GetComponent<Text>().text = card.energyCost.ToString();
        transform.Find("Name").GetComponent<Text>().text = card.cardName;
        transform.Find("Effects/EffectDescription").GetComponent<Text>().text = card.cardDescription;
        transform.Find("AttackDefense/Attack").GetComponent<Text>().text = card.attack.ToString();
        transform.Find("AttackDefense/Defense").GetComponent<Text>().text = card.defense.ToString();
    }

    public void UpdateSelected()
    {
        if (isSelected)
        {
            transform.GetComponent<Image>().color = new Color32(194, 255, 146, 255);
        }

        else
        {
            transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }
}
