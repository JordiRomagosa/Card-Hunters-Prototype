using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public uint healthMax;
    public uint energyMax;
    public uint energyStart;
    public uint energyRecovery;
    public uint cardDraw;

    public int healthCurrent;
    public uint energyCurrent;
    public int shieldCurrent;

    void Start()
    {
        healthCurrent = (int)healthMax;
        energyCurrent = energyStart;
        shieldCurrent = 0;
    }

    public void StartNextTurn()
    {
        energyCurrent += energyRecovery;
        if (energyCurrent > energyMax)
        {
            energyCurrent = energyMax;
        }

        shieldCurrent = 0;
    }

    public bool CanUseCard(Card card)
    {
        if (card == null)
        {
            return false;
        }

        return card.energyCost <= energyCurrent;
    }

    public bool PrepareCardToUse(Card playingCard)
    {
        if (playingCard == null)
        {
            return false;
        }

        energyCurrent -= playingCard.energyCost;
        shieldCurrent += (int)playingCard.defense;

        return true;
    }

    public bool RemoveCardToUse(Card playingCard)
    {
        if (playingCard == null)
        {
            return false;
        }

        energyCurrent += playingCard.energyCost;
        shieldCurrent -= (int)playingCard.defense;

        return true;
    }

    public uint RealizeCardActionsReturnDamage(Character target, Card playingCard)
    {
        uint damage = target.AttackCharacterReturnDamage(playingCard.attack, playingCard.shieldBreak);

        if (playingCard.vampirism > 0)
        {
            healthCurrent += (int) (playingCard.vampirism * damage);
        }

        if (playingCard.energyBoost > 0)
        {
            energyCurrent += playingCard.energyBoost;
        }

        healthCurrent += (int)playingCard.heal;
        if (healthCurrent > healthMax)
        {
            healthCurrent = (int)healthMax;
        }

        return damage;
    }

    public uint AttackCharacterReturnDamage(uint attack, uint shieldBreak)
    {
        if (shieldBreak > 0 && shieldCurrent > 0)
        {
            shieldCurrent -= (int)shieldBreak;

            if (shieldCurrent < 0)
            {
                shieldCurrent = 0;
            }
        }

        if (attack <= 0)
        {
            return 0;
        }

        uint assignedDamage = attack;

        if (shieldCurrent > 0)
        {
            if (assignedDamage <= shieldCurrent)
            {
                shieldCurrent -= (int)assignedDamage;
                return 0;
            }
            else
            {
                assignedDamage -= (uint)shieldCurrent;
                shieldCurrent = 0;
            }
        }

        healthCurrent -= (int)assignedDamage;

        return assignedDamage;
    }
}
