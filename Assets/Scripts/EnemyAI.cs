using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public const uint DISCARD_ENERGY_BONUS = 1;
    public const uint INITIAL_CARD_DRAW = 5;

    public string deckFile = "Assets/enemy.json";
    public GameObject enemyPlayZone;

    public Deck enemyDeck;
    public HandEnemy enemyHand;

    public Character enemy;

    public List<Card> cardsPlayed;
    public Text discardCountDisplay;
    public List<Card> discardedCards;

    void Awake()
    {
        enemyDeck.deckFileName = deckFile;

        cardsPlayed = new List<Card>();
        discardedCards = new List<Card>();
    }

    public void ShowCardEffects()
    {
        ShowPlayedCards(false);
    }

    public void StartFirstTurn(Character player)
    {
        for (int i = 0; i < INITIAL_CARD_DRAW; i++)
        {
            DrawCard();
        }

        PlayCardsForTurn(player);
        enemyHand.ShowHandCardsHidden();
        ShowPlayedCards(true);
        discardCountDisplay.text = discardedCards.Count.ToString();
    }

    public void StartNewTurn(Character player)
    {
        for (int i = 0; i < enemy.cardDraw; i++)
        {
            DrawCard();
        }

        PlayCardsForTurn(player);
        enemyHand.ShowHandCardsHidden();
        ShowPlayedCards(true);
        discardCountDisplay.text = discardedCards.Count.ToString();
    }

    private void PlayCardsForTurn(Character player)
    {
        while (true)
        {
            CheckTryDiscard();

            Card cardToPlay = SelectCardToPlay(player);
            if (cardToPlay == null)
            {
                break;
            }

            enemyHand.RemoveCardFromHand(cardToPlay.cardID);
            enemy.PrepareCardToUse(cardToPlay);
            cardsPlayed.Add(cardToPlay);
        }
    }

    private Card SelectCardToPlay(Character player)
    {
        if (!enemyHand.HasCards()) {
            return null;
        }

        Card toPlayCard = null;
        uint currentEnergyCost = 0;
        int currentIndex = 1;

        while (true)
        {
            Card currentCard = enemyHand.GetCardAtIndex(currentIndex);
            if (currentCard == null)
            {
                break;
            }

            if (enemy.CanUseCard(currentCard) && currentCard.energyCost >= currentEnergyCost)
            {
                if (toPlayCard == null || currentCard.energyCost > currentEnergyCost)
                {
                    toPlayCard = currentCard;
                    currentEnergyCost = currentCard.energyCost;
                }
                else
                {
                    toPlayCard = ChoosePreferedCardToPlay(player, toPlayCard, currentCard);
                }
            }

            currentIndex++;
        }

        return toPlayCard;
    }

    private Card ChoosePreferedCardToPlay(Character player, Card card1, Card card2)
    {
        //If card can kill the player, choose the one with highest attack
        if (player.healthCurrent <= card1.attack && card1.attack >= card2.attack)
        {
            return card1;
        }
        else if (player.healthCurrent <= card2.attack)
        {
            return card2;
        }

        //Otherwise, choose card at random
        int cardChoosen = Random.Range(1, 3);
        if (cardChoosen == 1)
        {
            return card1;
        }
        else if (cardChoosen == 2)
        {
            return card2;
        }

        Debug.Log("Something went wrong and didn't choose any card for enemy.");
        return null;
    }

    private void CheckTryDiscard()
    {
        if (!enemyHand.HasCards())
        {
            return;
        }

        uint maxEnergyCost = 0;
        foreach (Card c in enemyHand.cardsInHand)
        {
            if (c.energyCost > maxEnergyCost)
            {
                maxEnergyCost = c.energyCost;
            }
        }

        if (maxEnergyCost <= enemy.energyCurrent)
        {
            return;
        }

        while (maxEnergyCost > enemy.energyCurrent)
        {
            Card cardToDiscard = FindCardToDiscard(maxEnergyCost);
            if (cardToDiscard == null)
            {
                break;
            }

            enemy.energyCurrent += DISCARD_ENERGY_BONUS;
            enemyHand.RemoveCardFromHand(cardToDiscard.cardID);
            discardedCards.Add(cardToDiscard);
        }
    }

    private Card FindCardToDiscard(uint wantCardCost)
    {
        foreach (Card c in enemyHand.cardsInHand)
        {
            if (c.energyCost < wantCardCost || enemyHand.cardsInHand.Count >= enemyHand.maxHand + enemy.cardDraw)
            {
                return c;
            }
        }

        return null;
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

    public void ShowPlayedCards(bool hidden)
    {
        foreach (Transform child in enemyPlayZone.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (hidden)
        {
            SetScaleAndStyleToPlayedCard(enemyHand.cardReverse, hidden);
        }

        else
        {
            SetScaleAndStyleToPlayedCard(enemyHand.cardPrefab, hidden);
        }
    }

    private void SetScaleAndStyleToPlayedCard(GameObject prefab, bool hiddenValues)
    {
        RectTransform rt = (RectTransform)enemyPlayZone.transform;
        float areaHeight = rt.rect.height;
        float areaWidth = rt.rect.width;

        GameObject instantiatedCard = Instantiate(prefab, enemyPlayZone.transform);
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

        float totalCardWidth = cardsPlayed.Count * cardWidth;
        float cardPadding = 5.0f;

        if (totalCardWidth > areaWidth)
        {
            cardPadding = (areaWidth - totalCardWidth) / cardsPlayed.Count * 1.1f;
        }
        else if (totalCardWidth + (cardPadding * cardsPlayed.Count) > areaWidth)
        {
            cardPadding = (areaWidth - totalCardWidth) / cardsPlayed.Count;
        }

        for (int i = 1; i < cardsPlayed.Count + 1; i++)
        {
            GameObject cardInGame = Instantiate(instantiatedCard, enemyPlayZone.transform);
            float displacement = enemyHand.GetCardPositionInHand(i, cardsPlayed.Count, cardWidth, cardPadding);
            cardInGame.transform.localPosition = new Vector3(displacement, 0.1f, 0);

            if (!hiddenValues)
            {
                CardUpdater newCard = cardInGame.GetComponent<CardUpdater>();
                newCard.card = cardsPlayed[i - 1];
                newCard.UpdateCardValues();
            }
        }

        GameObject.Destroy(instantiatedCard.gameObject);
    }
}
