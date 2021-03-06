﻿using System.Collections;
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
        Vampirism,
        DoubleDamage,
        DoubleShield,
        EnergyBoost,
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
    public bool vampirism;
    public bool doubleDamage;
    public bool doubleShield;
    public uint energyBoost;

    //public uint buffDuration;
    //public uint attackBuff;
    //public uint defenseBuff;
}
