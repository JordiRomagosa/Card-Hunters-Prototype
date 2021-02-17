using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardForger : MonoBehaviour
{
    public EffectManager effectManager;
    public CardManager cardManager;
    public CardTemplate cardTemplate;
    public GameObject effectTemplatePrefab;

    public GameObject effectZoneTemplate;
    public Text cardNameTemplate;
    public Text costTextTemplate;
    public Text cardNameInputField;

    public GameObject finalForgedCard;
    public GameObject confirmationWindow;

    private bool hasChangedEffects = true;

    void Start()
    {
        HideForgedCard();
    }

    void Update()
    {
        cardNameTemplate.text = cardTemplate.cardName.ToString();

        if (hasChangedEffects)
        {
            UpdateTemplate();
            hasChangedEffects = false;
        }
    }

    public void SetCardTemplateName()
    {
        cardTemplate.cardName = cardNameInputField.text;
    }

    public void ChangedSelectedRarity(int effectIndex, Effect.EffectRarity rarity)
    {
        if (cardTemplate.effectSlots[effectIndex].effect != null)
        {
            if (cardTemplate.effectSlots[effectIndex].effect.rarity == rarity)
            {
                return;
            }
            cardTemplate.effectSlots[effectIndex].effect = null;
            hasChangedEffects = true;
        }
    }

    public void ChangeSelectedEffect(Effect.EffectRarity rarity, int effectIndex, string effectName)
    {
        if (effectName == "None")
        {
            cardTemplate.effectSlots[effectIndex].effect = null;
            return;
        }

        Effect effect = FindEffectWithNameAndRarity(effectName, rarity);
        if (effect == null)
        {
            return;
        }

        cardTemplate.effectSlots[effectIndex].effect = effect;
        hasChangedEffects = true;
    }

    public void ShowForgedCard()
    {
        cardTemplate.ForgeCard();

        finalForgedCard.transform.Find("EnergyCost/Cost").GetComponent<Text>().text = cardTemplate.energyCost.ToString();
        finalForgedCard.transform.Find("Name").GetComponent<Text>().text = cardTemplate.cardName;
        finalForgedCard.transform.Find("Effects/EffectDescription").GetComponent<Text>().text = cardTemplate.cardDescription;
        finalForgedCard.transform.Find("AttackDefense/Attack").GetComponent<Text>().text = cardTemplate.attack.ToString();
        finalForgedCard.transform.Find("AttackDefense/Defense").GetComponent<Text>().text = cardTemplate.defense.ToString();

        finalForgedCard.SetActive(true);
        confirmationWindow.SetActive(true);
    }

    public void HideForgedCard()
    {
        finalForgedCard.SetActive(false);
        confirmationWindow.SetActive(false);
    }

    public void ConfirmForgeCard()
    {
        Card card = new Card(cardTemplate);
        cardManager.AddCardToSaveFile(card);

        SceneManager.LoadScene("DeckShowcase");
    }

    public void ChangeTemplateSelected()
    {





    }

    public void LoadDeckShowcaseScreen()
    {
        SceneManager.LoadScene("DeckShowcase");
    }

    private void UpdateTemplate()
    {
        cardTemplate.CalculateFinalEnergyCost();
        costTextTemplate.text = cardTemplate.energyCost.ToString();
        RecreateEffectDescriptions();
    }

    private void RecreateEffectDescriptions()
    {
        foreach (Transform child in effectZoneTemplate.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < cardTemplate.GetEffectNumber(); i++)
        {
            if (cardTemplate.effectSlots[i].effect == null)
            {
                GameObject effect = Instantiate(effectTemplatePrefab, effectZoneTemplate.transform);
                effect.transform.Find("Rarity").GetComponent<Text>().text = "-";
                effect.transform.Find("Description").GetComponent<Text>().text = "Select Effect";
            }

            else
            {
                GameObject effect = Instantiate(effectTemplatePrefab, effectZoneTemplate.transform);

                string rarityText = "";
                switch (cardTemplate.effectSlots[i].effect.rarity)
                {
                    case Effect.EffectRarity.Common:
                        rarityText = "C";
                        break;
                    case Effect.EffectRarity.Advanced:
                        rarityText = "A";
                        break;
                    case Effect.EffectRarity.Special:
                        rarityText = "S";
                        break;
                    case Effect.EffectRarity.Definitive:
                        rarityText = "D";
                        break;
                }
                effect.transform.Find("Rarity").GetComponent<Text>().text = rarityText;
                effect.transform.Find("Description").GetComponent<Text>().text = cardTemplate.effectSlots[i].effect.effectName + " - " + cardTemplate.effectSlots[i].effect.description;
            }
        }
    }

    private Effect FindEffectWithNameAndRarity(string name, Effect.EffectRarity rarity)
    {
        List<Effect> listEffects = new List<Effect>();

        switch (rarity)
        {
            case Effect.EffectRarity.Common:
                listEffects = effectManager.effectsCommon;
                break;
            case Effect.EffectRarity.Advanced:
                listEffects = effectManager.effectsAdvanced;
                break;
            case Effect.EffectRarity.Special:
                listEffects = effectManager.effectsSpecial;
                break;
            case Effect.EffectRarity.Definitive:
                listEffects = effectManager.effectsDefinitive;
                break;
        }

        foreach (Effect effect in listEffects)
        {
            if (effect.effectName == name)
            {
                return effect;
            }
        }

        return null;
    }
}
