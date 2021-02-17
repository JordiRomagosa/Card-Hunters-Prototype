using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSelector : MonoBehaviour
{
    public CardTemplate cardTemplate;
    public CardForger cardForger;
    public EffectManager effectManager;

    public GameObject effectSelectorPrefab;
    
    private List<Dropdown> rarityDropdowns;
    private List<Dropdown> effectDropdowns;

    void Start()
    {
        rarityDropdowns = new List<Dropdown>();
        effectDropdowns = new List<Dropdown>();

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (var i = 0; i < cardTemplate.effectSlots.Count; i++)
        {
            Transform go = Instantiate(effectSelectorPrefab, transform).transform;

            rarityDropdowns.Add(go.Find("Rarity").GetComponent<Dropdown>());
            effectDropdowns.Add(go.Find("SelectedEffect").GetComponent<Dropdown>());
            go.GetComponent<EffectSelectorListener>().selector = this;
            RewriteRarityOptions(i);
            RewriteEffectOptions(i);
        }

        effectManager.LoadEffects();
    }

    public void RarityChanged()
    {
        for (var i = 0; i < cardTemplate.effectSlots.Count; i++)
        {
            Effect.EffectRarity rarity = GetRarityForEffectIndex(i);

            cardForger.ChangedSelectedRarity(i, rarity);
            if (cardTemplate.effectSlots[i].effect == null)
            {
                RewriteEffectOptions(i);
            }
        }
    }

    public void EffectSelected()
    {
        for (var i = 0; i < cardTemplate.effectSlots.Count; i++)
        {
            string stringEffect = effectDropdowns[i].transform.Find("EffectName").GetComponent<Text>().text;

            cardForger.ChangeSelectedEffect(GetRarityForEffectIndex(i), i, stringEffect);
        }
    }

    private void RewriteRarityOptions(int index)
    {
        rarityDropdowns[index].ClearOptions();

        if (cardTemplate.effectSlots[index].rarity >= Effect.EffectRarity.Common)
        {
            AddToRarityOptions(index, "Common");
        }
        if (cardTemplate.effectSlots[index].rarity >= Effect.EffectRarity.Advanced)
        {
            AddToRarityOptions(index, "Advanced");
        }
        if (cardTemplate.effectSlots[index].rarity >= Effect.EffectRarity.Special)
        {
            AddToRarityOptions(index, "Special");
        }
        if (cardTemplate.effectSlots[index].rarity >= Effect.EffectRarity.Definitive)
        {
            AddToRarityOptions(index, "Definitive");
        }
    }

    private void RewriteEffectOptions(int index)
    {
        effectDropdowns[index].ClearOptions();
        AddToEffectOptions(index, "None");

        Effect.EffectRarity rarity = GetRarityForEffectIndex(index);
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
            AddToEffectOptions(index, effect.effectName);
        }
    }
    
    private void AddToRarityOptions(int index, string text)
    {
        List<string> newOption = new List<string> { text };
        rarityDropdowns[index].AddOptions(newOption);
    }
    
    private void AddToEffectOptions(int index, string text)
    {
        List<string> newOption = new List<string> { text };
        effectDropdowns[index].AddOptions(newOption);
    }

    private Effect.EffectRarity GetRarityForEffectIndex(int index)
    {
        string s = rarityDropdowns[index].transform.Find("RarityText").GetComponent<Text>().text;
        return GetRarityForString(s);
    }

    private Effect.EffectRarity GetRarityForString(string s)
    {
        Effect.EffectRarity result = Effect.EffectRarity.Common;

        switch (s)
        {
            case "Common":
                result = Effect.EffectRarity.Common;
                break;
            case "Advanced":
                result = Effect.EffectRarity.Advanced;
                break;
            case "Special":
                result = Effect.EffectRarity.Special;
                break;
            case "Definitive":
                result = Effect.EffectRarity.Definitive;
                break;
        }

        return result;
    }
}
