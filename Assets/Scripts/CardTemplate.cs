using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTemplate : MonoBehaviour
{
    public class EffectSlot
    {
        public Effect.EffectRarity rarity;
        public Effect effect;

        public EffectSlot(Effect.EffectRarity rarity, Effect effect)
        {
            this.rarity = rarity;
            this.effect = effect;
        }
    }

    public string cardName;
    public uint energyCost;
    public uint attack;
    public uint defense;
    public uint heal;
    public uint shieldBreak;
    public string cardDescription;

    public List<EffectSlot> effectSlots;

    public int GetEffectNumber() { return effectSlots.Count; }

    void Awake()
    {
        CreateEffectSlots(1, 1, 1, 0);
    }

    public void CreateEffectSlots(uint commonNum, uint advancedNum, uint specialNum, uint definitiveNum)
    {
        effectSlots = new List<EffectSlot>();

        for (var i = 0; i < commonNum; i++)
        {
            effectSlots.Add(new EffectSlot(Effect.EffectRarity.Common, null));
        }
        for (var i = 0; i < advancedNum; i++)
        {
            effectSlots.Add(new EffectSlot(Effect.EffectRarity.Advanced, null));
        }
        for (var i = 0; i < specialNum; i++)
        {
            effectSlots.Add(new EffectSlot(Effect.EffectRarity.Special, null));
        }
        for (var i = 0; i < definitiveNum; i++)
        {
            effectSlots.Add(new EffectSlot(Effect.EffectRarity.Definitive, null));
        }
    }

    public bool AssignEffectToSlot(int slotIndex, Effect effect)
    {
        if (effectSlots[slotIndex].rarity >= effect.rarity)
        {
            effectSlots[slotIndex] = new EffectSlot(effectSlots[slotIndex].rarity, effect);
            return true;
        }

        return false;
    }

    public void ForgeCard()
    {
        CalculateFinalEnergyCost();
        CalculateFinalAttack();
        CalculateFinalDefense();
        CalculateFinalHeal();
        CalculateFinalShieldBreak();
        //BuildCardEffects();
        WriteCardEffectDescription();
    }
    
    public void CalculateFinalEnergyCost()
    {
        energyCost = 0;
        for (var i = 0; i < effectSlots.Count; i++)
        {
            if (effectSlots[i].effect != null)
            {
                energyCost += effectSlots[i].effect.energyCost;
            }
        }
    }

    private void CalculateFinalAttack()
    {
        attack = 0;

        for (var i = 0; i < effectSlots.Count; i++)
        {
            if (effectSlots[i].effect != null)
            {
                attack += effectSlots[i].effect.attack;
            }
        }
    }

    private void CalculateFinalDefense()
    {
        defense = 0;
        for(var i = 0; i < effectSlots.Count; i++)
        {
            if (effectSlots[i].effect != null)
            {
                defense += effectSlots[i].effect.defense;
            }
        }
    }

    private void CalculateFinalHeal()
    {
        heal = 0;
        for (var i = 0; i < effectSlots.Count; i++)
        {
            if (effectSlots[i].effect != null)
            {
                heal += effectSlots[i].effect.heal;
            }
        }
    }
    
    private void CalculateFinalShieldBreak()
    {
        shieldBreak = 0;
        for (var i = 0; i < effectSlots.Count; i++)
        {
            if (effectSlots[i].effect != null)
            {
                shieldBreak += effectSlots[i].effect.shieldBreak;
            }
        }
    }

    private void WriteCardEffectDescription()
    {
        cardDescription = "";
        string s = "";

        if (shieldBreak > 0)
        {
            s = "Break " + shieldBreak + " shield points and then ";
            cardDescription += s;
        }

        if (attack > 0)
        {
            s = "Deal " + attack + " damage";
            if (cardDescription != "")
            {
                s = s.ToLower();
            }
            cardDescription += s;
        }

        if (defense > 0)
        {
            s = "Gain " + defense + " shield";
            if (cardDescription != "")
            {
                s = " and " + s.ToLower();
            }
            cardDescription += s;
        }

        if (heal > 0)
        {
            s = "Recover " + heal + " hit points";
            if (cardDescription != "")
            {
                s = " and " + s.ToLower();
            }
            cardDescription += s;
        }

        cardDescription += ".";
    }

}
