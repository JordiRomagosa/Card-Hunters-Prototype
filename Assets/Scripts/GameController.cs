using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Deck playerDeck;
    public Hand playerHand;

    public GameObject canvas;
    public GameObject cardDropZone;
    public GameObject cardPrefab;

    private List<Card> cardsPlayed;

    void Start()
    {
        cardsPlayed = new List<Card>();
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
        RectTransform rt = (RectTransform)cardDropZone.transform;

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

        return false;
    }

    private void ShowPlayedCards()
    {
        foreach (Transform child in cardDropZone.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Card card in cardsPlayed)
        {
            GameObject cardPlayed = Instantiate(cardPrefab, cardDropZone.transform);
            CardUpdater newCard = cardPlayed.GetComponent<CardUpdater>();
            newCard.card = card;
            newCard.UpdateCardValues();

            cardPlayed.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
