using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Deck playerDeck;
    public Hand playerHand;

    void Start()
    {

    }

    void Update()
    {

    }

    public void DrawCard()
    {
        if (playerHand.MaximumHandIsReached())
        {
            Debug.Log("Max hand reached");
            return;
        }

        if (!playerDeck.DeckIsEmpty())
        {
            Card drawnCard = playerDeck.DrawNextCard();
            playerHand.AddCardToHand(drawnCard);
            playerHand.ShowHandCards();
        }

        else
        {
            playerDeck.ShuffleDeck();
        }
    }
}
