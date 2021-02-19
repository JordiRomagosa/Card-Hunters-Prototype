using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public CardManager cardManager;
    public string deckFileName = "Assets/deck.json";

    private List<Card> allDeckCards;
    private List<Card> currentDeck;

    void Start()
    {
        allDeckCards = cardManager.LoadDeck(deckFileName);
        ShuffleDeck();
    }

    public int RemainingCardsInDeck()
    {
        return currentDeck.Count;
    }

    public bool DeckIsEmpty()
    {
        return currentDeck.Count == 0;
    }

    public Card DrawCard()
    {
        if (DeckIsEmpty())
        {
            return null;
        }

        Card drawnCard = currentDeck[0];
        currentDeck.RemoveAt(0);

        return drawnCard;
    }

    public void ShuffleDeck()
    {
        currentDeck = new List<Card>(allDeckCards);

        for (int i = 0; i < currentDeck.Count; i++)
        {
            Card temp = currentDeck[i];
            int randomIndex = Random.Range(i, currentDeck.Count);
            currentDeck[i] = currentDeck[randomIndex];
            currentDeck[randomIndex] = temp;
        }
    }
}
