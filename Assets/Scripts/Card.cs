using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;
    public uint energyCost;
    public uint attack;
    public uint defense;
    public uint heal;
    public uint shieldBreak;
    public string cardDescription;
    public List<Effect.EffectTag> tags;

    public Card(CardTemplate template)
    {
        this.cardName = template.cardName;
        this.energyCost = template.energyCost;
        this.attack = template.attack;
        this.defense = template.defense;
        this.heal = template.heal;
        this.shieldBreak = template.shieldBreak;
        this.cardDescription = template.cardDescription;

        HashSet<Effect.EffectTag> tags = new HashSet<Effect.EffectTag>();
        foreach (CardTemplate.EffectSlot effectSlot in template.effectSlots)
        {
            if (effectSlot.effect != null)
            {
                foreach (Effect.EffectTag tag in effectSlot.effect.tags)
                {
                   tags.Add(tag);
                }
            }
        }
        this.tags = new List<Effect.EffectTag>(tags);
    }
}
