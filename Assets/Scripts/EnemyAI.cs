using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public string deckFile = "Assets/enemy.json";

    public Deck enemyDeck;
    public HandEnemy enemyHand;

    public Character enemy;

    private List<Card> cardsPlayed;
    public Text discardCountDisplay;
    public List<Card> discardedCards;

    void Awake()
    {
        enemyDeck.deckFileName = deckFile;

        cardsPlayed = new List<Card>();
        discardedCards = new List<Card>();
    }

    public void StartFirstTurn(uint cardDraw, Character player)
    {
        for (int i = 0; i < cardDraw; i++)
        {
            DrawCard();
        }

        enemyHand.ShowHandCardsHidden();
        discardCountDisplay.text = discardedCards.Count.ToString();
    }
    

    public void StartNewTurn(Character player)
    {
        
    }


    private void DrawCard()
    {
        if (enemyHand.MaximumHandIsReached())
        {
            Debug.Log("Max hand reached enemy");
        }

        if (enemyDeck.DeckIsEmpty())
        {
            enemyDeck.ShuffleDiscardsExceptPlayed(enemyHand.cardsInHand, cardsPlayed);
            discardedCards.Clear();
            discardCountDisplay.text = discardedCards.Count.ToString();


        }

        Card drawnCard = enemyDeck.DrawNextCard();
        enemyHand.AddCardToHand(drawnCard);
        
    }

    private void ShowPlayedCards(bool hidden)
    {
        if (hidden)
        {
            enemyHand.ShowHandCardsHidden();
        }

        else
        {
            enemyHand.ShowHandCards();
        }
    }
}
