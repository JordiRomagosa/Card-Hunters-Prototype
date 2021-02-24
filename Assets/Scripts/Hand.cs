using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public GameObject handCardArea;
    public GameObject cardPrefab;

    public uint maxHand;
    public float maxPadding;
    public List<Card> cardsInHand; //TODO: make private
    //private List<CardUpdater> cards;

    void Start()
    {
        cardsInHand = new List<Card>();
    }

    public bool MaximumHandIsReached()
    {
        return cardsInHand.Count == maxHand;
    }

    public bool AddCardToHand(Card cardToAdd)
    {
        if (MaximumHandIsReached())
        {
            return false;
        }

        cardsInHand.Add(cardToAdd);
        return true;
    }

    public Card RemoveCardFromHand(int cardIndex)
    {
        if (cardIndex > cardsInHand.Count)
        {
            return null;
        }

        Card removed = cardsInHand[cardIndex - 1];
        cardsInHand.RemoveAt(cardIndex - 1);
        return removed;
    }

    public void ShowHandCards()
    {
        foreach (Transform child in handCardArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        RectTransform rt = (RectTransform)handCardArea.transform;
        float areaHeight = rt.rect.height;
        float areaWidth = rt.rect.width;

        GameObject instantiatedCard = Instantiate(cardPrefab, handCardArea.transform);
        rt = (RectTransform)instantiatedCard.transform;
        float cardHeight = rt.rect.height;
        float cardWidth = rt.rect.width;

        if (cardHeight > areaHeight)
        {
            float scaleFactor = areaHeight / cardHeight;
            Vector3 scaleVec = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            instantiatedCard.transform.localScale = scaleVec;
            cardWidth *= scaleFactor;
        }

        float totalCardWidth = cardsInHand.Count * cardWidth;
        float cardPadding = maxPadding;
        
        if (totalCardWidth > areaWidth)
        {
            cardPadding = (areaWidth - totalCardWidth) / cardsInHand.Count * 1.1f;
        }
        else if (totalCardWidth + (cardPadding * cardsInHand.Count) > areaWidth)
        {
            cardPadding = (areaWidth - totalCardWidth) / cardsInHand.Count;
        }

        for (int i = 1; i < cardsInHand.Count + 1; i++)
        {
            GameObject cardInGame = Instantiate(instantiatedCard, handCardArea.transform);
            float displacement = GetCardPositionInHand(i, cardsInHand.Count, cardWidth, cardPadding);
            cardInGame.transform.position += new Vector3(displacement / 57.0f, 0, 0);

            CardUpdater newCard = cardInGame.GetComponent<CardUpdater>();
            newCard.card = cardsInHand[i - 1];
            newCard.UpdateCardValues();

            DragAndDrop cardDrop = cardInGame.GetComponent<DragAndDrop>();
            cardDrop.cardHandIndex = i;
        }

        GameObject.Destroy(instantiatedCard.gameObject);
    }

    private float GetCardPositionInHand(int cardNum, int totalCards, float cardWidth, float cardPadding)
    {
        float centralCard = (totalCards + 1.0f) / 2.0f;
        float currentCardRelative = cardNum - centralCard;

        float cardDisplacement = (cardPadding + cardWidth) * currentCardRelative;

        return cardDisplacement;
    }
}
