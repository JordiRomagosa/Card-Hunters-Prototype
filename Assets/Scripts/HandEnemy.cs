using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandEnemy : Hand
{
    public GameObject cardReverse;
    public Text handCountDisplay;

    public bool HasCards()
    {
        return cardsInHand.Count > 0;
    }

    public void ShowHandCardsHidden()
    {
        foreach (Transform child in handCardArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        RectTransform rt = (RectTransform)handCardArea.transform;
        float areaHeight = rt.rect.height;
        float areaWidth = rt.rect.width;

        GameObject instantiatedCard = Instantiate(cardReverse, handCardArea.transform);
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
            cardInGame.transform.localPosition = new Vector3(displacement, 0.1f, 0);
        }

        GameObject.Destroy(instantiatedCard.gameObject);
        handCountDisplay.text = cardsInHand.Count.ToString();
    }
}
