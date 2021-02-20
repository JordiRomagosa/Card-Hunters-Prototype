using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public CardManager cardManager;
    public string deckFileName = "Assets/deck.json";

    public Text deckCountDisplay;

    public List<Card> allDeckCards; //TODO: make private
    public List<Card> currentDeck;

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

    public Card DrawNextCard()
    {
        if (DeckIsEmpty())
        {
            return null;
        }

        Card drawnCard = currentDeck[0];
        currentDeck.RemoveAt(0);
        deckCountDisplay.text = currentDeck.Count.ToString();

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

        deckCountDisplay.text = currentDeck.Count.ToString();
    }
}
