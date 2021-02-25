using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Deck playerDeck;
    public Hand playerHand;

    public GameObject canvas;
    public GameObject cardPlayZone;
    public GameObject cardDiscardZone;
    public GameObject cardPrefab;

    private List<Card> cardsPlayed;
    public Text discardCountDisplay;
    public List<Card> discardedCards; //TODO: make private?

    void Start()
    {
        cardsPlayed = new List<Card>();
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
            playerDeck.ShuffleDiscardsExceptPlayed(playerHand.cardsInHand, cardsPlayed);
            discardedCards.Clear();
            discardCountDisplay.text = discardedCards.Count.ToString();
        }
    }

    public Vector2 GetCanvasMousePosition()
    {
        Vector2 localPoint;
        RectTransform canvasRect = (RectTransform)canvas.transform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, canvas.GetComponent<Canvas>().worldCamera, out localPoint);

        return localPoint / 57.0f;
    }

    public bool DropCard(int cardHandIndex)
    {
        Vector2 mousePos = GetCanvasMousePosition();

        //Drop on play zone
        RectTransform rt = (RectTransform)cardPlayZone.transform;
        Vector3[] v = new Vector3[4];
        rt.GetWorldCorners(v);

        if (mousePos.x > v[0].x && mousePos.x < v[2].x && mousePos.y > v[0].y && mousePos.y < v[2].y)
        {
            Card removedCard = playerHand.RemoveCardFromHand(cardHandIndex);
            cardsPlayed.Add(removedCard);

            playerHand.ShowHandCards();
            ShowPlayedCards();
            return true;
        }

        //Drop on discard zone
        rt = (RectTransform)cardDiscardZone.transform;
        rt.GetWorldCorners(v);

        if (mousePos.x > v[0].x && mousePos.x < v[2].x && mousePos.y > v[0].y && mousePos.y < v[2].y)
        {
            Card removedCard = playerHand.RemoveCardFromHand(cardHandIndex);
            discardedCards.Add(removedCard);

            playerHand.ShowHandCards();
            discardCountDisplay.text = discardedCards.Count.ToString();
            return true;
        }

        return false;
    }

    private void ShowPlayedCards()
    {
        foreach (Transform child in cardPlayZone.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Card card in cardsPlayed)
        {
            GameObject cardPlayed = Instantiate(cardPrefab, cardPlayZone.transform);
            CardUpdater newCard = cardPlayed.GetComponent<CardUpdater>();
            newCard.card = card;
            newCard.UpdateCardValues();

            cardPlayed.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
