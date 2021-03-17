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
    public uint shieldCurrent;

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
        shieldCurrent += playingCard.defense;

        return true;
    }

    public bool RemoveCardToUse(Card playingCard)
    {
        if (playingCard == null)
        {
            return false;
        }

        energyCurrent += playingCard.energyCost;
        shieldCurrent -= playingCard.defense;

        return true;
    }

    public uint RealizeCardActionsReturnDamage(Character target, Card playingCard)
    {
        healthCurrent += (int)playingCard.heal;
        if (healthCurrent > healthMax)
        {
            healthCurrent = (int)healthMax;
        }
        return target.AttackCharacterReturnDamage(playingCard.attack, playingCard.shieldBreak);
    }

    public uint AttackCharacterReturnDamage(uint attack, uint shieldBreak)
    {
        uint assignedDamage = attack;

        if (shieldCurrent > 0)
        {
            if (shieldBreak > 0)
            {
                shieldCurrent -= shieldBreak;

                if (shieldCurrent < 0)
                {
                    shieldCurrent = 0;
                }
            }

            if (assignedDamage <= shieldCurrent)
            {
                shieldCurrent -= assignedDamage;
                return 0;
            }
            else
            {
                assignedDamage -= shieldCurrent;
                shieldCurrent = 0;
            }
        }

        healthCurrent -= (int)assignedDamage;
        if (healthCurrent < 0)
        {
            healthCurrent = 0;
        }

        return assignedDamage;
    }
}
