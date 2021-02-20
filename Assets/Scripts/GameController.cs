using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Deck playerDeck;


    void Start()
    {

    }

    void Update()
    {

    }

    public void DrawCard()
    {
        if (!playerDeck.DeckIsEmpty())
        {
            Card drawnCard = playerDeck.DrawNextCard();
        }

        else
        {
            playerDeck.ShuffleDeck();
        }
    }
}
