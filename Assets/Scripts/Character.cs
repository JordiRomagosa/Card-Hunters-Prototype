using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public uint startingHealthPoints = 20;
    public uint startingEnergyPoints = 0;
    public uint startingShieldPoints = 0;
    public uint energyPointsPerTurn = 2;

    private uint healthPoints;
    private uint energyPoints;
    private uint shieldPoints;

    private Card playingCard;

    void Start()
    {
        healthPoints = startingHealthPoints;
        energyPoints = startingEnergyPoints;
        shieldPoints = startingShieldPoints;
    }

    public void StartNextTurn()
    {
        energyPoints += energyPointsPerTurn;
        shieldPoints = 0;
    }

    public bool CanUseCard(Card card)
    {
        if (card == null)
        {
            return false;
        }

        return card.energyCost <= energyPoints;
    }

    public bool PrepareCardToUse(Card card)
    {
        if (card == null)
        {
            return false;
        }

        energyPoints -= card.energyCost;
        shieldPoints += card.defense;
        healthPoints += card.heal;

        playingCard = card;

        return true;
    }

    public uint RealizeAttackCardActions(Character target)
    {
        return target.AttackCharacterReturnDamage(playingCard.attack, playingCard.shieldBreak);
    }

    public uint AttackCharacterReturnDamage(uint attack, uint shieldBreak)
    {
        uint assignedDamage = attack;

        if (shieldPoints > 0)
        {
            if (shieldBreak > 0)
            {
                shieldPoints -= shieldBreak;

                if (shieldPoints < 0)
                {
                    shieldPoints = 0;
                }
            }

            if (assignedDamage <= shieldPoints)
            {
                shieldPoints -= assignedDamage;
                return 0;
            }
            else
            {
                assignedDamage -= shieldPoints;
                shieldPoints = 0;
            }
        }

        healthPoints -= assignedDamage;
        if (healthPoints < 0)
        {
            healthPoints = 0;
        }

        return assignedDamage;
    }
}
