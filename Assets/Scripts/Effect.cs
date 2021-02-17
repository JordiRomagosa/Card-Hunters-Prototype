using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public enum EffectRarity
    {
        Common,
        Advanced,
        Special,
        Definitive,
    }
    public enum EffectTag
    {
        Attack,
        Defense,
        Heal,
        ShieldBreak,
        //AttackBuff,
        //DeffenseBuff,
    }

    public string effectName;
    public string description;
    public EffectRarity rarity;
    public EffectTag[] tags = { };

    public uint energyCost;
    public uint attack;
    public uint defense;
    public uint heal;
    public uint shieldBreak;

    //public uint buffDuration;
    //public uint attackBuff;
    //public uint defenseBuff;
}
